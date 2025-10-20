-- Script SQL para vincular assistidos com municípios existentes
-- Tabela: municipio (id uuid, nome varchar, cep_inicio varchar, cep_fim varchar, uf varchar)

-- ============================================================================
-- VINCULAÇÃO DE ASSISTIDOS COM MUNICÍPIOS EXISTENTES
-- ============================================================================

BEGIN;

-- ============================================================================
-- VERIFICAÇÕES DOS MUNICÍPIOS EXISTENTES
-- ============================================================================

-- Listar todos os municípios cadastrados
SELECT 
    id,
    nome,
    cep_inicio,
    cep_fim,
    uf,
    CASE WHEN status = 1 THEN 'ATIVO' ELSE 'INATIVO' END as status
FROM municipio 
WHERE uf = 'RS'
ORDER BY nome;

-- Contar total de municípios
SELECT COUNT(*) as total_municipios FROM municipio WHERE uf = 'RS';

-- ============================================================================
-- SCRIPT PARA VINCULAR ASSISTIDOS EXISTENTES COM MUNICÍPIOS
-- ============================================================================

-- Atualizar assistidos baseado no CEP (se disponível)
UPDATE assistido 
SET id_municipio = m.id
FROM municipio m
WHERE assistido.cep IS NOT NULL 
  AND assistido.cep != ''
  AND assistido.cep BETWEEN m.cep_inicio AND m.cep_fim
  AND assistido.id_municipio IS NULL;

-- Atualizar assistidos baseado no texto da observação
UPDATE assistido 
SET id_municipio = (SELECT id FROM municipio WHERE nome = 'Feliz' AND uf = 'RS')
WHERE (observacao ILIKE '%FELIZ%' OR endereco ILIKE '%FELIZ%') 
  AND id_municipio IS NULL;

UPDATE assistido 
SET id_municipio = (SELECT id FROM municipio WHERE nome = 'Bom Princípio' AND uf = 'RS')
WHERE (observacao ILIKE '%BOM PRINCÍPIO%' OR observacao ILIKE '%BOM PRINCIPIO%' OR endereco ILIKE '%BOM PRINCÍPIO%') 
  AND id_municipio IS NULL;

UPDATE assistido 
SET id_municipio = (SELECT id FROM municipio WHERE nome = 'Caxias do Sul' AND uf = 'RS')
WHERE (observacao ILIKE '%CAXIAS%' OR endereco ILIKE '%CAXIAS%') 
  AND id_municipio IS NULL;

UPDATE assistido 
SET id_municipio = (SELECT id FROM municipio WHERE nome = 'Farroupilha' AND uf = 'RS')
WHERE (observacao ILIKE '%FARROUPILHA%' OR endereco ILIKE '%FARROUPILHA%') 
  AND id_municipio IS NULL;

-- Verificar assistidos sem município vinculado
SELECT 
    COUNT(*) as assistidos_sem_municipio
FROM assistido 
WHERE id_municipio IS NULL;

-- Listar assistidos com município vinculado
SELECT 
    a.nome as assistido,
    m.nome as municipio,
    a.cep,
    a.endereco
FROM assistido a
INNER JOIN municipio m ON a.id_municipio = m.id
ORDER BY m.nome, a.nome;