-- Script SQL para inserir dados da planilha APAE na tabela assistido
-- Baseado na análise da planilha b1.xls
-- IMPORTANTE: Ajustar os UUIDs conforme necessário para as chaves estrangeiras

-- Primeiro, vamos criar UUIDs para usar como IDs (substitua por IDs reais do seu sistema)
-- Para municipios, você precisa ter os municipios já cadastrados
-- Para convenios, você precisa ter os convenios já cadastrados

-- Schema da tabela assistido (conforme ApiBaseModel + campos específicos):
-- id (uuid) - PK
-- status (smallint) - 1=Ativo, 2=Inativo
-- nome (varchar) - obrigatório
-- data_nascimento (date)
-- endereco, bairro, cep, cpf, naturalidade (varchar)
-- sexo (smallint) - 1=M, 2=F
-- tipo_deficiencia (smallint) - 1=VISUAL, 2=AUDITIVA, 3=INTELECTUAL, 4=FISICA, 5=MULTIPLA
-- medicamentos_uso (boolean)
-- medicamentos_quais, nome_mae, nome_pai, nome_responsavel, telefone_responsavel (varchar)
-- id_municipio, id_convenio (uuid - FK)
-- data_cadastro (date)
-- observacao (text)
-- E muitos outros campos específicos...

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
    id_municipio,
    observacao,
    status
) VALUES
-- Registro 1: Adair Boeni
(
    gen_random_uuid(),
    'adair boeni',
    '1980-12-16'::date,
    'rua willy reichert, 295',
    'matiel',
    '95770-000',
    '967.736.770-68',
    1, -- Masculino (M)
    3, -- INTELECTUAL (baseado no contexto APAE)
    true, -- Usa medicamentos
    'carbamazebina 200mg - clorbromazina 100mg - citalopran 20mg',
    'jacinta boeni',
    'paulo boeni',
    'jacinta boeni',
    '51-3637-1699',
    '2010-06-01'::date, -- Data de entrada
    (SELECT id FROM municipio WHERE nome = 'FELIZ' AND uf = 'RS' LIMIT 1), -- Feliz/RS
    'Data saída: 19/12/2014. Situação: INATIVO. Tel rec: 51-9774-9646. RG: 77040748306. CNS: 000.0000.0000.0000. Escolaridade: NÃO. Alergia: NÃO. Comorbidade: NÃO. Uso imagem: NÃO',
    2 -- INATIVO (StatusEntidadeEnum.Inativo = 2)
),

-- Registro 2: Adriano Barth
(
    gen_random_uuid(),
    'adriano barth',
    '1984-08-02'::date,
    'travessa linha tamandaŕi, 280',
    'linha tamandaŕi',
    '95765-000',
    '008.052.790-67',
    1, -- Masculino (M)
    3, -- INTELECTUAL
    false, -- Não usa medicamentos
    null,
    'irena lúcia barth',
    'décio barth',
    'irena lúcia barth',
    '51-9952-76049',
    '1994-06-01'::date, -- Data de entrada
    (SELECT id FROM municipio WHERE nome = 'BOM PRINCÍPIO' AND uf = 'RS' LIMIT 1), -- Bom Princípio/RS
    'Situação: ATIVO. RG: 1076829082. CNS: 704.2097.9550.1386. Diagnóstico: intelectual. Convenio: bom principio (assistencia social). Escolaridade: NÃO. Alergia: NÃO. Comorbidade: NÃO. Uso imagem: SIM. Obs: eja renda familia: r$ 2.600,00. Tel mae: 51-9964-10288',
    1 -- ATIVO (StatusEntidadeEnum.Ativo = 1)
),

-- Registro 3: Álcio Diovan Zambelli Pretto
(
    gen_random_uuid(),
    'álcio diovan zambelli pretto',
    '2003-10-20'::date,
    null, -- Endereço não informado na planilha visível
    null,
    null,
    null, -- CPF não informado
    1, -- Masculino (M)
    3, -- INTELECTUAL (baseado no contexto APAE)
    false, -- Não especificado, assumindo false
    null,
    null, -- Nome da mãe não informado na parte visível
    null, -- Nome do pai não informado na parte visível
    null, -- Responsável não informado
    null, -- Telefone não informado
    '2011-01-02'::date, -- Data de entrada
    null, -- Cidade não informada
    'Situação: INATIVO. Data saída: 02/05/2013. RG: 1121129901. CNS: 000.0000.0000.0000. Número prontuário: 35472',
    2 -- INATIVO (StatusEntidadeEnum.Inativo = 2)
);

-- INSTRUÇÕES ADICIONAIS:
-- 1. Este script insere apenas os primeiros registros visíveis da planilha
-- 2. Para inserir todos os 217 registros, seria necessário processar a planilha completa
-- 3. Ajuste os seguintes campos conforme seu sistema:
--    - id_municipio: Relacionar com a tabela de municípios existente
--    - id_convenio: Relacionar com a tabela de convênios existente
--    - Campos de enum: Verificar se os valores estão corretos
-- 4. Campos que podem precisar de ajuste:
--    - naturalidade: Não estava claro na planilha
--    - cid: Código CID-10 quando disponível
--    - Campos boolean: Converter "SIM"/"NÃO" para true/false
--    - Telefones: Padronizar formato

-- SCRIPT PARA PROCESSAR DADOS ADICIONAIS DA PLANILHA:
-- Use este template para adicionar mais registros:

/*
TEMPLATE PARA NOVOS REGISTROS:
(
    gen_random_uuid(),
    'NOME_DO_ASSISTIDO',
    'YYYY-MM-DD'::date, -- data_nascimento
    'ENDERECO_COMPLETO',
    'BAIRRO',
    'CEP',
    'CPF',
    SEXO_ENUM, -- 1=M, 2=F
    TIPO_DEFICIENCIA_ENUM, -- 1=VISUAL, 2=AUDITIVA, 3=INTELECTUAL, 4=FISICA, 5=MULTIPLA
    MEDICAMENTOS_USO_BOOLEAN,
    'MEDICAMENTOS_QUAIS',
    'NOME_MAE',
    'NOME_PAI',
    'NOME_RESPONSAVEL',
    'TELEFONE_RESPONSAVEL',
    'YYYY-MM-DD'::date, -- data_cadastro (entrada)
    'OBSERVACOES_GERAIS',
    STATUS_ENUM, -- 0=INATIVO, 1=ATIVO
    NOW(),
    NOW()
),
*/

-- COMANDOS ÚTEIS PARA VERIFICAÇÃO:
-- SELECT COUNT(*) FROM assistido; -- Verificar quantos registros foram inseridos
-- SELECT nome, cpf, data_nascimento FROM assistido ORDER BY nome; -- Verificar dados inseridos
-- SELECT * FROM assistido WHERE cpf IS NOT NULL; -- Verificar registros com CPF

-- LIMPEZA (se necessário):
-- DELETE FROM assistido WHERE created_at > NOW() - INTERVAL '1 hour'; -- Remove registros criados na última hora