#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Script para converter planilha APAE (CSV) em comandos SQL para inserção
na tabela assistido do sistema

INSTRUÇÕES DE USO:
1. Colocar o arquivo b1.csv na mesma pasta do script
2. Executar o script: python converter_csv_para_sql.py
3. O resultado será salvo em: assistidos_da_planilha.sql

DEPENDÊNCIAS NECESSÁRIAS:
pip install pandas

MAPEAMENTO DE COLUNAS (ajustar conforme a planilha real):
"""

import pandas as pd
import re
from datetime import datetime
import uuid
import os

# CONFIGURAÇÕES
ARQUIVO_CSV = 'b1.csv'  # Arquivo CSV
ARQUIVO_SQL_SAIDA = 'assistidos_da_planilha.sql'

# MAPEAMENTO DE CIDADES COM UUIDs EXATOS
MAPEAMENTO_CIDADES = {
    'antônio prado': '004a35e0-cad0-4bcf-aa4e-fd8d8f235c49',
    'ipê': '252b5a00-e687-4bcb-99b9-a62f175ea8b1',
    'são vendelino': '38297214-a87e-4d28-a5ee-5120925632e3',
    'pareci novo': '3a3398f8-eead-40ad-8fdd-9066260d8129',
    'farroupilha': '4a8a10c1-e285-4791-8734-6b54a16d2ba0',
    'maratá': '576c809c-7f86-4099-b685-c9a02ae4475f',
    'barão': '581da75c-9fae-45de-b85b-c52492fce24d',
    'são josé do hortêncio': '67544348-72d6-40ad-ae16-27c05b95b903',
    'são sebastião do caí': '7677f6d9-6119-40bf-b2d6-6c1e197ed806',
    'são marcos': '79732666-32f2-4b4c-a019-4d4961f5df28',
    'bom princípio': '7e5fc077-2a61-4854-a0fe-f2fccaeb6ec9',
    'brochier': '85163617-4f7c-44c1-80ad-bb6979e41328',
    'salvador do sul': '8a2bed83-4a8b-469c-af49-75a947477b85',
    'feliz': '9557b00a-2f75-46ac-bd56-ceec0cb32e40',
    'alto feliz': '9d8f9bb7-1ae6-4e11-9f5d-196ce6647083',
    'flores da cunha': 'a1cc1c0d-3bbc-4d6d-8b94-44776bc2bef9',
    'montenegro': 'acb186f7-1446-4a0e-b287-64dfc328ce48',
    'tupandi': 'afe9b6a7-c065-45bc-bb84-35c75bf78b07',
    'capela de santana': 'b42ec156-8fe5-4f75-a9b0-9cd1d6ab90fc',
    'são josé do sul': 'b49e7ff7-2208-4934-814a-b650cac7d502',
    'vale real': 'c19cae75-8172-467a-b9dd-6437b458d328',
    'nova roma do sul': 'daaa3a11-606e-48bb-855f-aeb4dd5c344e',
    'nova pádua': 'dc63439d-1717-48df-8a4f-2a08361f237a',
    'harmonia': 'e0a9c688-422f-45a6-9123-8d17cdbd7dcd',
    'linha nova': 'eb4aa874-72a4-4b43-8aa0-1a9762e0e665',
    'são pedro da serra': 'fa22bbca-90f5-47f4-a0a1-c643ceb6b69b'
}

# MAPEAMENTO DE COLUNAS (ajustar conforme a estrutura real da planilha)
COLUNAS_PLANILHA = {
    'nome': 'Nome',  # Coluna do nome na planilha
    'situacao': 'Situação',  # ATIVO/INATIVO
    'area_atend': 'Área atend',
    'num_prontuario': 'Nº pront',
    'data_entrada': 'Entrada',
    'data_saida': 'Saída',
    'cpf': 'CPF',
    'rg': 'RG',
    'data_nascimento': 'Dt Nasc',
    'sexo': 'Sexo',
    'diagnostico': 'Diagnóstico',
    'endereco': 'Endereço',
    'bairro': 'Bairro',
    'cep': 'Cep',
    'cidade': 'Cidade/UF',
    'telefone_residencial': 'Tel res',
    'telefone_recado': 'Tel rec',
    'nome_mae': 'Mãe',
    'nome_pai': 'Pai',
    'nome_responsavel': 'Responsável',
    'telefone_responsavel': 'Tel resp',
    'medicamentos': 'Medic',
    'alergia': 'Alerg',
    'comorbidade': 'Comorbidade',
    'convenio': 'Convênio',
    'uso_imagem': 'Uso imagem',
    'observacoes': 'Obs'
}

class ConversorPlanilhaSQL:
    def __init__(self):
        self.registros_processados = 0
        self.erros = []

    def limpar_string(self, valor):
        """Limpa e formata strings para SQL"""
        if pd.isna(valor) or valor == '' or str(valor).strip() == '':
            return 'null'
        
        # Remove caracteres especiais e limpa a string
        cleaned = str(valor).strip().upper()
        # Escapa aspas simples para SQL
        cleaned = cleaned.replace("'", "''")
        return f"'{cleaned}'"

    def converter_data(self, data_str):
        """Converte string de data para formato SQL"""
        if pd.isna(data_str) or data_str == '' or str(data_str).strip() == '':
            return 'null'
        
        try:
            # Tenta diferentes formatos de data
            formatos_data = ['%d/%m/%Y', '%Y-%m-%d', '%d-%m-%Y', '%d.%m.%Y']
            data_str = str(data_str).strip()
            
            for fmt in formatos_data:
                try:
                    data_parsed = datetime.strptime(data_str, fmt)
                    return f"'{data_parsed.strftime('%Y-%m-%d')}'::date"
                except ValueError:
                    continue
            
            # Se não conseguir parsear, retorna null
            self.erros.append(f"Data inválida: {data_str}")
            return 'null'
        except:
            return 'null'

    def converter_cpf(self, cpf_str):
        """Limpa e valida CPF"""
        if pd.isna(cpf_str) or str(cpf_str).strip() == '':
            return 'null'
        
        # Remove caracteres não numéricos
        cpf_clean = re.sub(r'[^\d]', '', str(cpf_str))
        
        # Verifica se tem 11 dígitos
        if len(cpf_clean) == 11:
            # Formata CPF
            formatted_cpf = f"{cpf_clean[:3]}.{cpf_clean[3:6]}.{cpf_clean[6:9]}-{cpf_clean[9:]}"
            return f"'{formatted_cpf}'"
        elif len(cpf_clean) > 0:
            # CPF incompleto, salva como string mesmo assim
            return f"'{cpf_str}'"
        
        return 'null'

    def converter_sexo(self, sexo_str):
        """Converte sexo para enum SexoEnum"""
        if pd.isna(sexo_str) or str(sexo_str).strip() == '':
            return 'null'
        
        sexo = str(sexo_str).strip().upper()
        if sexo == 'M' or sexo == 'MASCULINO':
            return '1'  # SexoEnum.M = 1
        elif sexo == 'F' or sexo == 'FEMININO':
            return '2'  # SexoEnum.F = 2
        
        return 'null'

    def converter_status(self, status_str):
        """Converte status para enum StatusEntidadeEnum"""
        if pd.isna(status_str) or str(status_str).strip() == '':
            return '1'  # Default ATIVO
        
        status = str(status_str).strip().upper()
        if status in ['ATIVO', 'ACTIVE', '1']:
            return '1'  # StatusEntidadeEnum.Ativo = 1
        elif status in ['INATIVO', 'INACTIVE', '0', '2']:
            return '2'  # StatusEntidadeEnum.Inativo = 2
        
        return '1'  # Default ATIVO

    def converter_boolean(self, bool_str):
        """Converte string para boolean SQL"""
        if pd.isna(bool_str) or str(bool_str).strip() == '':
            return 'false'
        
        valor = str(bool_str).strip().upper()
        if valor in ['SIM', 'TRUE', '1', 'S']:
            return 'true'
        elif valor in ['NÃO', 'NAO', 'FALSE', '0', 'N']:
            return 'false'
        
        return 'false'

    def extrair_medicamentos(self, medicamentos_str):
        """Extrai informações sobre medicamentos"""
        if pd.isna(medicamentos_str) or str(medicamentos_str).strip() == '':
            return 'false', 'null'
        
        med_str = str(medicamentos_str).strip().upper()
        
        if 'NÃO' in med_str or 'NAO' in med_str:
            return 'false', 'null'
        elif 'SIM' in med_str:
            # Tenta extrair quais medicamentos
            if '(' in med_str and ')' in med_str:
                medicamentos = med_str.split('(')[1].split(')')[0]
                return 'true', self.limpar_string(medicamentos)
            else:
                return 'true', 'null'
        
        # Se contém nomes de medicamentos
        if any(med in med_str for med in ['MG', 'ML', 'COMPRIMIDO', 'CAPSULA', 'GOTAS']):
            return 'true', self.limpar_string(medicamentos_str)
        
        return 'false', 'null'

    def determinar_tipo_deficiencia(self, diagnostico_str):
        """Determina o tipo de deficiência baseado no diagnóstico"""
        if pd.isna(diagnostico_str) or str(diagnostico_str).strip() == '':
            return '3'  # Default: INTELECTUAL (mais comum em APAE)
        
        diag = str(diagnostico_str).strip().upper()
        
        if 'VISUAL' in diag or 'CEGO' in diag or 'CEGUEIRA' in diag:
            return '1'  # VISUAL
        elif 'AUDITIV' in diag or 'SURDO' in diag or 'SURDEZ' in diag:
            return '2'  # AUDITIVA
        elif 'FISIC' in diag or 'MOTOR' in diag or 'PARALISIA' in diag:
            return '4'  # FISICA
        elif 'MULTIPLA' in diag or 'MÚLTIPLA' in diag:
            return '5'  # MULTIPLA
        else:
            return '3'  # INTELECTUAL (default)

    def processar_csv(self, arquivo_csv):
        """Processa o arquivo CSV e gera o SQL"""
        
        # Verifica se o arquivo existe
        if not os.path.exists(arquivo_csv):
            raise FileNotFoundError(f"Arquivo '{arquivo_csv}' não encontrado!")
        
        try:
            # Lê o arquivo CSV
            print(f"Lendo arquivo CSV: {arquivo_csv}")
            
            # Tenta diferentes codificações para CSV
            encodings = ['utf-8', 'latin1', 'iso-8859-1', 'cp1252', 'utf-16']
            df = None
            
            for encoding in encodings:
                try:
                    print(f"Tentando codificação: {encoding}")
                    # Usa sep=';' pois o arquivo usa ponto e vírgula como separador
                    # Pula a primeira linha que é um cabeçalho decorativo
                    df = pd.read_csv(arquivo_csv, encoding=encoding, sep=';', skiprows=1)
                    if not df.empty:
                        print(f"✅ Sucesso com codificação: {encoding}")
                        print(f"📊 Dados carregados: {len(df)} linhas, {len(df.columns)} colunas")
                        break
                except Exception as e:
                    print(f"❌ Falhou com {encoding}: {str(e)}")
                    continue
            
            if df is None or df.empty:
                raise Exception("Não foi possível ler o arquivo CSV com nenhuma codificação")
            
            print(f"CSV carregado com {len(df)} linhas e {len(df.columns)} colunas")
            print(f"Colunas encontradas: {list(df.columns)}")
            
            # Coleta todas as cidades únicas da planilha
            cidades_unicas = set()
            for index, row in df.iterrows():
                cidade, uf = self.extrair_cidade_uf(row.get('Cidade/UF', ''))
                if cidade and uf:
                    cidades_unicas.add((cidade, uf))
            
            # Gera o SQL
            sql_completo = self.gerar_sql_header()
            
            # Adiciona comentário sobre as cidades encontradas
            if cidades_unicas:
                sql_completo += "\n-- ============================================================================\n"
                sql_completo += "-- CIDADES ENCONTRADAS NA PLANILHA (assumindo que já existem no banco)\n"
                sql_completo += "-- ============================================================================\n"
                for cidade, uf in cidades_unicas:
                    sql_completo += f"-- {cidade}/{uf}\n"
                sql_completo += "\n"
            
            valores_sql = []
            for index, row in df.iterrows():
                try:
                    valor_sql = self.processar_linha(row, index)
                    if valor_sql:
                        valores_sql.append(valor_sql)
                        self.registros_processados += 1
                except Exception as e:
                    self.erros.append(f"Erro na linha {index}: {str(e)}")
                    continue
            
            # Junta todos os valores
            if valores_sql:
                sql_completo += ',\n'.join(valores_sql)
                sql_completo += ';\n\nCOMMIT;\n'
                sql_completo += self.gerar_sql_footer()
            else:
                sql_completo += "-- NENHUM REGISTRO VÁLIDO ENCONTRADO\nROLLBACK;"
            
            return sql_completo
            
        except Exception as e:
            print(f"❌ Erro ao ler arquivo CSV: {str(e)}")
            print("\n🔧 SOLUÇÕES POSSÍVEIS:")
            print("1. Verificar se o arquivo está na pasta correta")
            print("2. Verificar se o arquivo não está aberto no Excel")
            print("3. Tentar salvar novamente como CSV com codificação UTF-8")
            raise Exception(f"Erro ao processar CSV: {str(e)}")

    def processar_linha(self, row, index):
        """Processa uma linha do CSV"""
        # Extrai dados da linha (ajustar nomes das colunas conforme necessário)
        nome = row.get('Nome', row.get('nome', ''))
        if pd.isna(nome) or str(nome).strip() == '':
            return None  # Pula linhas sem nome
        
        # Extrai informações sobre medicamentos
        medicamentos_raw = row.get('Medic', row.get('medicamentos', ''))
        medicamentos_uso, medicamentos_quais = self.extrair_medicamentos(medicamentos_raw)
        
        # Extrai cidade e UF e mapeia para UUID
        cidade, uf = self.extrair_cidade_uf(row.get('Cidade/UF', ''))
        id_municipio_sql = 'null'
        if cidade and uf:
            # Normaliza o nome da cidade para busca no mapeamento
            cidade_normalizada = cidade.lower().strip()
            if cidade_normalizada in MAPEAMENTO_CIDADES:
                uuid_municipio = MAPEAMENTO_CIDADES[cidade_normalizada]
                id_municipio_sql = f"'{uuid_municipio}'"
            else:
                print(f"⚠️  Cidade não encontrada no mapeamento: '{cidade}' (será definida como NULL)")
                id_municipio_sql = 'null'

        # Monta o valor SQL
        sql_valor = f"""(
    gen_random_uuid(),
    {self.converter_status(row.get('Situação', row.get('situacao', '')))},
    {self.limpar_string(nome)},
    {self.converter_data(row.get('Dt Nasc', row.get('data_nascimento', '')))},
    {self.limpar_string(row.get('Endereço', row.get('endereco', '')))},
    {self.limpar_string(row.get('Bairro', row.get('bairro', '')))},
    {self.limpar_string(row.get('Cep', row.get('cep', '')))},
    {self.converter_cpf(row.get('CPF', row.get('cpf', '')))},
    {self.converter_sexo(row.get('Sexo', row.get('sexo', '')))},
    {self.determinar_tipo_deficiencia(row.get('Diagnóstico', row.get('diagnostico', '')))},
    {medicamentos_uso},
    {medicamentos_quais},
    {self.limpar_string(row.get('Mãe', row.get('nome_mae', '')))},
    {self.limpar_string(row.get('Pai', row.get('nome_pai', '')))},
    {self.limpar_string(row.get('Responsável', row.get('nome_responsavel', '')))},
    {self.limpar_string(row.get('Tel resp', row.get('telefone_responsavel', '')))},
    {self.converter_data(row.get('Entrada', row.get('data_entrada', '')))},
    {id_municipio_sql},
    {self.limpar_string(self.montar_observacoes(row))}
)"""
        
        return sql_valor

    def extrair_cidade_uf(self, cidade_uf_str):
        """Extrai nome da cidade e UF da string Cidade/UF"""
        if pd.isna(cidade_uf_str) or str(cidade_uf_str).strip() == '':
            return None, None
        
        cidade_uf = str(cidade_uf_str).strip().upper()
        
        if '/' in cidade_uf:
            partes = cidade_uf.split('/')
            cidade = partes[0].strip()
            uf = partes[1].strip() if len(partes) > 1 else 'RS'
        else:
            cidade = cidade_uf
            uf = 'RS'  # Default para RS
        
        return cidade, uf



    def montar_observacoes(self, row):
        """Monta o campo de observações com informações da planilha"""
        obs_parts = []
        
        # Adiciona informações disponíveis
        campos_obs = [
            ('RG', row.get('RG', '')),
            ('Prontuário', row.get('Nº pront', '')),
            ('Data Saída', row.get('Saída', '')),
            ('Tel Recado', row.get('Tel rec', '')),
            ('Cidade', row.get('Cidade/UF', '')),
            ('Convênio', row.get('Convênio', '')),
            ('Alergia', row.get('Alerg', '')),
            ('Comorbidade', row.get('Comorbidade', '')),
            ('Uso Imagem', row.get('Uso imagem', '')),
            ('Observações', row.get('Obs', ''))
        ]
        
        for campo, valor in campos_obs:
            if not pd.isna(valor) and str(valor).strip() != '':
                obs_parts.append(f"{campo}: {str(valor).strip()}")
        
        return '. '.join(obs_parts) if obs_parts else ''

    def gerar_sql_header(self):
        """Gera o cabeçalho do SQL"""
        return """-- Script SQL gerado automaticamente a partir da planilha APAE
-- IMPORTANTE: Execute em ambiente de teste primeiro!
-- Data de geração: {data}

BEGIN;

INSERT INTO assistido (
    id,
    status,
    nome,
    data_nascimento,
    endereco,
    bairro,
    cep,
    cpf,
    sexo,
    tipo_deficiencia,
    medicamentos_uso,
    medicamentos_quais,
    nome_mae,
    nome_pai,
    nome_responsavel,
    telefone_responsavel,
    data_cadastro,
    id_municipio,
    observacao
) VALUES
""".format(data=datetime.now().strftime('%Y-%m-%d %H:%M:%S'))

    def gerar_sql_footer(self):
        """Gera o rodapé do SQL com verificações"""
        return f"""
-- ============================================================================
-- VERIFICAÇÕES PÓS-INSERÇÃO
-- ============================================================================

-- Verificar registros inseridos
SELECT 
    COUNT(*) as total_inseridos,
    SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END) as ativos,
    SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END) as inativos
FROM assistido
WHERE nome IN (
    SELECT nome FROM assistido 
    ORDER BY id DESC 
    LIMIT {self.registros_processados}
);

-- Registros sem CPF
SELECT COUNT(*) as sem_cpf 
FROM assistido 
WHERE cpf IS NULL OR cpf = '';

-- Registros sem data de nascimento
SELECT COUNT(*) as sem_data_nascimento
FROM assistido 
WHERE data_nascimento IS NULL;

-- ============================================================================
-- RELATÓRIO DE PROCESSAMENTO
-- ============================================================================
-- Registros processados: {self.registros_processados}
-- Erros encontrados: {len(self.erros)}

{self.gerar_relatorio_erros()}
"""

    def gerar_relatorio_erros(self):
        """Gera relatório de erros"""
        if not self.erros:
            return "-- Nenhum erro encontrado durante o processamento."
        
        relatorio = "-- ERROS ENCONTRADOS:\n"
        for i, erro in enumerate(self.erros[:10]):  # Mostra apenas os primeiros 10 erros
            relatorio += f"-- {i+1}. {erro}\n"
        
        if len(self.erros) > 10:
            relatorio += f"-- ... e mais {len(self.erros) - 10} erros.\n"
        
        return relatorio

def main():
    """Função principal"""
    conversor = ConversorPlanilhaSQL()
    
    try:
        print("=== CONVERSOR DE PLANILHA APAE PARA SQL ===")
        print(f"Processando arquivo: {ARQUIVO_CSV}")
        
        sql_resultado = conversor.processar_csv(ARQUIVO_CSV)
        
        # Salva o arquivo SQL
        with open(ARQUIVO_SQL_SAIDA, 'w', encoding='utf-8') as f:
            f.write(sql_resultado)
        
        print(f"\n✅ Conversão concluída!")
        print(f"📄 Arquivo SQL salvo: {ARQUIVO_SQL_SAIDA}")
        print(f"📊 Registros processados: {conversor.registros_processados}")
        
        if conversor.erros:
            print(f"⚠️  Erros encontrados: {len(conversor.erros)}")
            print("📝 Verifique o arquivo SQL para detalhes dos erros")
        
    except FileNotFoundError:
        print(f"❌ Erro: Arquivo '{ARQUIVO_CSV}' não encontrado!")
        print("💡 Dica: Coloque o arquivo b1.csv na mesma pasta do script")
        print(f"� Pasta atual: {os.getcwd()}")
        print("📋 Arquivos CSV disponíveis:")
        for f in os.listdir('.'):
            if f.endswith('.csv'):
                print(f"   📄 {f}")
    except Exception as e:
        print(f"❌ Erro durante a conversão: {str(e)}")

if __name__ == "__main__":
    main()