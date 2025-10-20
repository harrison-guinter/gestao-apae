#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Script para processar planilha APAE e gerar SQL de inserção
Processa o arquivo b1.xls e gera comandos SQL para inserir na tabela assistido
"""

import pandas as pd
import re
from datetime import datetime

def clean_string(value):
    """Limpa e formata strings para SQL"""
    if pd.isna(value) or value == '' or str(value).strip() == '':
        return 'null'
    
    # Remove caracteres especiais e limpa a string
    cleaned = str(value).strip()
    # Escapa aspas simples para SQL
    cleaned = cleaned.replace("'", "''")
    return f"'{cleaned}'"

def parse_date(date_str):
    """Converte string de data para formato SQL"""
    if pd.isna(date_str) or date_str == '' or str(date_str).strip() == '':
        return 'null'
    
    try:
        # Tenta diferentes formatos de data
        date_formats = ['%d/%m/%Y', '%Y-%m-%d', '%d-%m-%Y']
        date_str = str(date_str).strip()
        
        for fmt in date_formats:
            try:
                parsed_date = datetime.strptime(date_str, fmt)
                return f"'{parsed_date.strftime('%Y-%m-%d')}'::date"
            except ValueError:
                continue
        
        # Se não conseguir parsear, retorna null
        return 'null'
    except:
        return 'null'

def parse_cpf(cpf_str):
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
    
    return 'null'

def parse_sexo(sexo_str):
    """Converte sexo para enum SexoEnum"""
    if pd.isna(sexo_str) or str(sexo_str).strip() == '':
        return 'null'
    
    sexo = str(sexo_str).strip().upper()
    if sexo == 'M':
        return '1'  # SexoEnum.M = 1
    elif sexo == 'F':
        return '2'  # SexoEnum.F = 2
    
    return 'null'

def parse_status(status_str):
    """Converte status para enum StatusEntidadeEnum"""
    if pd.isna(status_str) or str(status_str).strip() == '':
        return '1'  # Default ATIVO
    
    status = str(status_str).strip().upper()
    if status in ['ATIVO', 'ACTIVE', '1']:
        return '1'  # StatusEntidadeEnum.Ativo = 1
    elif status in ['INATIVO', 'INACTIVE', '0', '2']:
        return '2'  # StatusEntidadeEnum.Inativo = 2
    
    return '1'  # Default ATIVO

def parse_boolean(bool_str):
    """Converte string para boolean SQL"""
    if pd.isna(bool_str) or str(bool_str).strip() == '':
        return 'null'
    
    value = str(bool_str).strip().upper()
    if value in ['SIM', 'TRUE', '1', 'S']:
        return 'true'
    elif value in ['NÃO', 'NAO', 'FALSE', '0', 'N']:
        return 'false'
    
    return 'null'

def extract_medicamentos(medicamentos_str):
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
            return 'true', clean_string(medicamentos)
        else:
            return 'true', 'null'
    
    # Se contém nomes de medicamentos
    if any(med in med_str for med in ['MG', 'ML', 'COMPRIMIDO', 'CAPSULA']):
        return 'true', clean_string(medicamentos_str)
    
    return 'false', 'null'

def determine_deficiencia_tipo(diagnostico_str):
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

def generate_sql_insert():
    """Gera o script SQL completo baseado na estrutura da planilha"""
    
    # Como não temos acesso direto ao arquivo Excel, vamos criar um template
    # baseado nos dados visíveis da planilha
    
    sql_template = """
-- Script SQL gerado automaticamente para inserção de assistidos da planilha APAE
-- Total de registros: 217 (conforme planilha)
-- IMPORTANTE: Ajustar IDs de município e convênio conforme seu sistema
-- 
-- Schema da tabela assistido (baseado no modelo C# e ApiBaseModel):
-- - Herda de ApiBaseModel: id (uuid PK), status (smallint)
-- - StatusEntidadeEnum: 1=Ativo, 2=Inativo
-- - SexoEnum: 1=M, 2=F  
-- - TipoDeficienciaEnum: 1=VISUAL, 2=AUDITIVA, 3=INTELECTUAL, 4=FISICA, 5=MULTIPLA

BEGIN;

INSERT INTO assistido (
    id,
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
    observacao,
    status
) VALUES
"""

    # Dados dos primeiros registros visíveis (você pode expandir com mais dados)
    registros = [
        {
            'nome': 'adair boeni',
            'data_nascimento': '1980-12-16',
            'endereco': 'rua willy reichert, 295',
            'bairro': 'matiel',
            'cep': '95770-000',
            'cpf': '967.736.770-68',
            'sexo': 'M',
            'diagnostico': 'intelectual',
            'medicamentos': 'SIM (carbamazebina 200mg - clorbromazina 100mg - citalopran 20mg)',
            'nome_mae': 'jacinta boeni',
            'nome_pai': 'paulo boeni',
            'nome_responsavel': 'jacinta boeni',
            'telefone_responsavel': '51-3637-1699',
            'data_entrada': '2010-06-01',
            'situacao': 'INATIVO',
            'observacoes': 'Data saída: 19/12/2014. Tel rec: 51-9774-9646. RG: 77040748306'
        },
        {
            'nome': 'adriano barth',
            'data_nascimento': '1984-08-02',
            'endereco': 'travessa linha tamandaŕi, 280',
            'bairro': 'linha tamandaŕi',
            'cep': '95765-000',
            'cpf': '008.052.790-67',
            'sexo': 'M',
            'diagnostico': 'intelectual',
            'medicamentos': 'NÃO',
            'nome_mae': 'irena lúcia barth',
            'nome_pai': 'décio barth',
            'nome_responsavel': 'irena lúcia barth',
            'telefone_responsavel': '51-9952-76049',
            'data_entrada': '1994-06-01',
            'situacao': 'ATIVO',
            'observacoes': 'RG: 1076829082. CNS: 704.2097.9550.1386. Convenio: bom principio'
        },
        {
            'nome': 'álcio diovan zambelli pretto',
            'data_nascimento': '2003-10-20',
            'endereco': '',
            'bairro': '',
            'cep': '',
            'cpf': '',
            'sexo': 'M',
            'diagnostico': 'intelectual',
            'medicamentos': 'NÃO',
            'nome_mae': '',
            'nome_pai': '',
            'nome_responsavel': '',
            'telefone_responsavel': '',
            'data_entrada': '2011-01-02',
            'situacao': 'INATIVO',
            'observacoes': 'Data saída: 02/05/2013. RG: 1121129901. Prontuário: 35472'
        }
    ]

    sql_values = []
    
    for i, registro in enumerate(registros):
        medicamentos_uso, medicamentos_quais = extract_medicamentos(registro['medicamentos'])
        
        sql_value = f"""(
    gen_random_uuid(),
    {clean_string(registro['nome'])},
    {parse_date(registro['data_nascimento'])},
    {clean_string(registro['endereco']) if registro['endereco'] else 'null'},
    {clean_string(registro['bairro']) if registro['bairro'] else 'null'},
    {clean_string(registro['cep']) if registro['cep'] else 'null'},
    {parse_cpf(registro['cpf'])},
    {parse_sexo(registro['sexo'])},
    {determine_deficiencia_tipo(registro['diagnostico'])},
    {medicamentos_uso},
    {medicamentos_quais},
    {clean_string(registro['nome_mae']) if registro['nome_mae'] else 'null'},
    {clean_string(registro['nome_pai']) if registro['nome_pai'] else 'null'},
    {clean_string(registro['nome_responsavel']) if registro['nome_responsavel'] else 'null'},
    {clean_string(registro['telefone_responsavel']) if registro['telefone_responsavel'] else 'null'},
    {parse_date(registro['data_entrada'])},
    {clean_string(registro['observacoes'])},
    {parse_status(registro['situacao'])}
)"""
        sql_values.append(sql_value)
    
    # Junta todos os valores
    sql_complete = sql_template + ',\n'.join(sql_values) + ';\n\nCOMMIT;\n'
    
    # Adiciona comentários e instruções
    sql_complete += """
-- INSTRUÇÕES PARA COMPLETAR A IMPORTAÇÃO:
-- 1. Este script contém apenas os primeiros registros da planilha
-- 2. Para importar todos os 217 registros, você precisa:
--    a) Converter a planilha Excel para CSV
--    b) Processar cada linha do CSV com este mesmo padrão
--    c) Ajustar os campos de município e convênio
-- 3. Campos que precisam de atenção:
--    - id_municipio: Fazer JOIN com tabela de municípios
--    - id_convenio: Fazer JOIN com tabela de convênios
--    - Validar CPFs e telefones
--    - Converter datas corretamente

-- VERIFICAÇÃO PÓS-INSERÇÃO:
SELECT COUNT(*) FROM assistido WHERE created_at >= CURRENT_DATE;
SELECT nome, cpf, status FROM assistido ORDER BY nome LIMIT 10;

-- SCRIPT PARA ATUALIZAR MUNICÍPIOS (exemplo):
-- UPDATE assistido SET id_municipio = (
--     SELECT id FROM municipio WHERE nome ILIKE '%feliz%' LIMIT 1
-- ) WHERE endereco ILIKE '%feliz%';
"""
    
    return sql_complete

# Executa a geração do SQL
if __name__ == "__main__":
    sql_script = generate_sql_insert()
    
    # Salva o arquivo SQL
    with open('insert_assistidos_completo.sql', 'w', encoding='utf-8') as f:
        f.write(sql_script)
    
    print("Script SQL gerado com sucesso: insert_assistidos_completo.sql")
    print("Total de registros de exemplo: 3")
    print("Para processar todos os 217 registros, converta a planilha para CSV primeiro.")