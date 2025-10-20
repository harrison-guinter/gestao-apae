-- Script SQL completo para inserir dados da planilha APAE na tabela assistido
-- Baseado no schema real do sistema (ApiBaseModel + campos específicos)
-- IMPORTANTE: Execute este script em um ambiente de teste primeiro!

-- ============================================================================
-- INFORMAÇÕES DO SCHEMA
-- ============================================================================
-- Tabela: assistido
-- Herda de ApiBaseModel: 
--   - id (uuid) PRIMARY KEY
--   - status (smallint) - StatusEntidadeEnum: 1=Ativo, 2=Inativo
--
-- Enums importantes:
--   - SexoEnum: 1=M, 2=F
--   - TipoDeficienciaEnum: 1=VISUAL, 2=AUDITIVA, 3=INTELECTUAL, 4=FISICA, 5=MULTIPLA
--   - StatusEntidadeEnum: 1=Ativo, 2=Inativo
-- ============================================================================

BEGIN;

-- Verificar se existe algum assistido com os mesmos CPFs antes de inserir
-- (evitar duplicatas)
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM assistido 
        WHERE cpf IN ('967.736.770-68', '008.052.790-67')
    ) THEN
        RAISE NOTICE 'AVISO: Já existem assistidos com os CPFs da planilha. Verifique duplicatas!';
    END IF;
END $$;

-- Inserção dos dados da planilha
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

-- ============================================================================
-- REGISTRO 1: Adair Boeni (INATIVO)
-- ============================================================================
(
    gen_random_uuid(),
    2, -- INATIVO (StatusEntidadeEnum.Inativo = 2)
    'ADAIR BOENI',
    '1980-12-16'::date,
    'RUA WILLY REICHERT, 295',
    'MATIEL',
    '95770-000',
    '967.736.770-68',
    1, -- MASCULINO (SexoEnum.M = 1)
    3, -- INTELECTUAL (TipoDeficienciaEnum.INTELECTUAL = 3)
    true, -- Usa medicamentos
    'CARBAMAZEBINA 200MG, CLORPROMAZINA 100MG, CITALOPRAM 20MG',
    'JACINTA BOENI',
    'PAULO BOENI',
    'JACINTA BOENI',
    '(51) 3637-1699',
    '2010-06-01'::date,
    (SELECT id FROM municipio WHERE nome = 'FELIZ' AND uf = 'RS' LIMIT 1), -- Feliz/RS
    'SITUAÇÃO: INATIVO. DATA SAÍDA: 19/12/2014. TEL RECADO: (51) 9774-9646. RG: 77040748306. CNS: 000.0000.0000.0000. PRONTUÁRIO: 00000. CIDADE: FELIZ/RS. ESCOLARIDADE: NÃO. MEDICAÇÃO: SIM. ALERGIA: NÃO. COMORBIDADE: NÃO. CONVÊNIO: NÃO. USO IMAGEM: NÃO'
),

-- ============================================================================
-- REGISTRO 2: Adriano Barth (ATIVO)
-- ============================================================================
(
    gen_random_uuid(),
    1, -- ATIVO (StatusEntidadeEnum.Ativo = 1)
    'ADRIANO BARTH',
    '1984-08-02'::date,
    'TRAVESSA LINHA TAMANDARI, 280',
    'LINHA TAMANDARI',
    '95765-000',
    '008.052.790-67',
    1, -- MASCULINO (SexoEnum.M = 1)
    3, -- INTELECTUAL (TipoDeficienciaEnum.INTELECTUAL = 3)
    false, -- Não usa medicamentos
    null,
    'IRENA LÚCIA BARTH',
    'DÉCIO BARTH',
    'IRENA LÚCIA BARTH',
    '(51) 9952-76049',
    '1994-06-01'::date,
    (SELECT id FROM municipio WHERE nome = 'BOM PRINCÍPIO' AND uf = 'RS' LIMIT 1), -- Bom Princípio/RS
    'SITUAÇÃO: ATIVO. RG: 1076829082. CNS: 704.2097.9550.1386. PRONTUÁRIO: 00025. DIAGNÓSTICO: INTELECTUAL. CIDADE: BOM PRINCÍPIO/RS. TEL MÃE: (51) 9964-10288. CONVÊNIO: BOM PRINCÍPIO (ASSISTÊNCIA SOCIAL). DATA CONVENIO: 06/12/2023. ESCOLARIDADE: EJA. ALERGIA: NÃO. COMORBIDADE: NÃO. USO IMAGEM: SIM. RENDA FAMÍLIA: R$ 2.600,00'
),

-- ============================================================================
-- REGISTRO 3: Álcio Diovan Zambelli Pretto (INATIVO)
-- ============================================================================
(
    gen_random_uuid(),
    2, -- INATIVO (StatusEntidadeEnum.Inativo = 2)
    'ÁLCIO DIOVAN ZAMBELLI PRETTO',
    '2003-10-20'::date,
    null, -- Endereço não informado
    null, -- Bairro não informado
    null, -- CEP não informado
    null, -- CPF não informado
    1, -- MASCULINO (SexoEnum.M = 1)
    3, -- INTELECTUAL (TipoDeficienciaEnum.INTELECTUAL = 3)
    false, -- Medicamentos não especificado
    null,
    null, -- Nome da mãe não informado
    null, -- Nome do pai não informado
    null, -- Responsável não informado
    null, -- Telefone não informado
    '2011-01-02'::date,
    null, -- Cidade não informada na planilha
    'SITUAÇÃO: INATIVO. DATA SAÍDA: 02/05/2013. RG: 1121129901. CNS: 000.0000.0000.0000. PRONTUÁRIO: 35472'
);

-- ============================================================================
-- TEMPLATE PARA INSERIR MAIS REGISTROS DA PLANILHA
-- ============================================================================
/*
Para adicionar mais registros, use este template:

(
    gen_random_uuid(),
    [STATUS], -- 1=Ativo, 2=Inativo
    '[NOME_COMPLETO_MAIUSCULO]',
    '[YYYY-MM-DD]'::date, -- data_nascimento
    '[ENDERECO_COMPLETO]',
    '[BAIRRO]',
    '[CEP]',
    '[CPF_FORMATADO]', -- XXX.XXX.XXX-XX
    [SEXO], -- 1=M, 2=F
    [TIPO_DEFICIENCIA], -- 1=VISUAL, 2=AUDITIVA, 3=INTELECTUAL, 4=FISICA, 5=MULTIPLA
    [MEDICAMENTOS_USO], -- true/false
    '[MEDICAMENTOS_QUAIS]',
    '[NOME_MAE]',
    '[NOME_PAI]',
    '[NOME_RESPONSAVEL]',
    '[TELEFONE_RESPONSAVEL]',
    '[YYYY-MM-DD]'::date, -- data_cadastro (entrada)
    '[OBSERVACOES_GERAIS]'
),
*/

COMMIT;

-- ============================================================================
-- VERIFICAÇÕES PÓS-INSERÇÃO
-- ============================================================================

-- Verificar quantos registros foram inseridos
SELECT 
    COUNT(*) as total_assistidos,
    SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END) as ativos,
    SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END) as inativos
FROM assistido;

-- Verificar os registros inseridos
SELECT 
    nome,
    cpf,
    data_nascimento,
    CASE WHEN status = 1 THEN 'ATIVO' ELSE 'INATIVO' END as situacao,
    data_cadastro
FROM assistido
WHERE nome IN ('ADAIR BOENI', 'ADRIANO BARTH', 'ÁLCIO DIOVAN ZAMBELLI PRETTO')
ORDER BY nome;

-- ============================================================================
-- SCRIPTS AUXILIARES PARA COMPLETAR OS DADOS
-- ============================================================================

-- 1. ATUALIZAR MUNICÍPIOS BASEADO NA COLUNA CIDADE/UF DA PLANILHA
-- Vincular assistidos com os municípios existentes
UPDATE assistido 
SET id_municipio = (
    SELECT id FROM municipio 
    WHERE nome = 'FELIZ' AND uf = 'RS'
    LIMIT 1
)
WHERE observacao ILIKE '%FELIZ/RS%' AND id_municipio IS NULL;

UPDATE assistido 
SET id_municipio = (
    SELECT id FROM municipio 
    WHERE nome = 'BOM PRINCÍPIO' AND uf = 'RS'
    LIMIT 1
)
WHERE observacao ILIKE '%BOM PRINCÍPIO/RS%' AND id_municipio IS NULL;

-- 2. ATUALIZAR CONVÊNIOS (quando disponível)
/*
UPDATE assistido 
SET id_convenio = (
    SELECT id FROM convenio 
    WHERE nome ILIKE '%BOM PRINCÍPIO%' OR nome ILIKE '%ASSISTÊNCIA SOCIAL%'
    LIMIT 1
)
WHERE observacao ILIKE '%BOM PRINCÍPIO%' AND id_convenio IS NULL;
*/

-- 3. NORMALIZAR TELEFONES (opcional)
/*
UPDATE assistido 
SET telefone_responsavel = REGEXP_REPLACE(telefone_responsavel, '[^0-9]', '', 'g')
WHERE telefone_responsavel IS NOT NULL;
*/

-- ============================================================================
-- OBSERVAÇÕES SOBRE MUNICÍPIOS
-- ============================================================================
-- IMPORTANTE: Este script assume que todos os municípios já foram inseridos
-- no banco de dados. Se algum município da planilha não existir, 
-- o campo id_municipio ficará NULL para esses registros.
-- Execute o script inserir_cidades_municipios.sql separadamente se necessário.

-- ============================================================================
-- INSTRUÇÕES PARA PROCESSAR TODA A PLANILHA (217 REGISTROS)
-- ============================================================================
/*
1. CONVERTER PLANILHA PARA CSV:
   - Abrir b1.xls no Excel/LibreOffice
   - Salvar como CSV (separado por vírgulas)
   
2. PROCESSAR COM SCRIPT PYTHON:
   - Usar o script processar_planilha_apae.py
   - Ajustar caminhos e mapeamentos
   
3. CAMPOS IMPORTANTES PARA MAPEAR:
   - Nome → nome (obrigatório)
   - Data Nasc → data_nascimento
   - Sexo → sexo (1=M, 2=F)
   - Situação → status (1=Ativo, 2=Inativo)
   - Endereço → endereco, bairro, cep
   - CPF → cpf (validar formato)
   - Telefones → telefone_responsavel
   - Mãe/Pai → nome_mae, nome_pai
   - Medicamentos → medicamentos_uso, medicamentos_quais
   - Diagnóstico → tipo_deficiencia
   - Entrada → data_cadastro
   - Cidade/UF → id_municipio (vincular com tabela municipio)
   - Observações → observacao

4. VALIDAÇÕES NECESSÁRIAS:
   - CPF único (evitar duplicatas)
   - Datas válidas
   - Telefones formatados
   - Nomes normalizados (maiúsculo)
   - Status correto (1 ou 2)
   - Cidade válida (inserir na tabela municipio se não existir)

5. PROCESSO DE VINCULAÇÃO COM CIDADES:
   - Extrair cidade e UF da coluna "Cidade/UF"
   - Verificar se a cidade existe na tabela municipio
   - Vincular assistido com o id_municipio correspondente
   - Se a cidade não existir, o id_municipio ficará NULL
*/