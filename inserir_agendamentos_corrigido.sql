-- ========================================
-- SCRIPT CORRIGIDO PARA INSERIR AGENDAMENTOS
-- ========================================

-- ========================================
-- 1. AGENDAMENTOS
-- ========================================
-- Campos: id, id_usuario, tipo_recorrencia, horario_agendamento, data_agendamento, observacao, status, dia_semana
-- Tipos de Recorrência: 1=NENHUM, 2=SEMANAL
-- Dias da Semana: 1=SEGUNDA, 2=TERCA, 3=QUARTA, 4=QUINTA, 5=SEXTA
-- Status: 1=ATIVO, 2=INATIVO

INSERT INTO public.agendamento (id, id_usuario, tipo_recorrencia, horario_agendamento, data_agendamento, observacao, status, dia_semana) VALUES
('3f1a89e4-7a10-4d6c-9e61-01b3f0c909f1', '0d73fdec-41ed-43c4-8597-d3e2eb4b8357', 1, '09:00:00', '2025-10-22', 'Avaliação inicial - Feliz', 1, 3),
('4e6c2b7a-8b0f-48d3-a61c-7d830fef1d44', '807c4250-443b-4b4e-8973-e47a04e59115', 2, '10:30:00', '2025-10-23', 'Atendimento psicológico - Vale Real', 1, 4),
('8d93a821-d1a9-4b64-b36a-2db15a9a4323', 'c2939859-32d0-4984-8bcd-fc4de84f47f7', 1, '11:00:00', '2025-10-24', 'Sessão de fisioterapia - Bom Princípio', 1, 5),
('22b8f59a-04d7-41aa-b11e-61adff7b9fd1', '8821d10b-ab41-436a-9358-57572cef39ae', 1, '14:00:00', '2025-10-22', 'Consulta pediátrica - Alto Feliz', 1, 3),
('ad1a2471-5e19-4b5a-8c8c-28a96ee9d771', '87c6c330-a23f-4be6-b9fc-601e03754473', 2, '13:30:00', '2025-10-25', 'Avaliação cardiológica - Tupandi', 1, 5),
('b3944a5b-84fc-4cc4-9f3a-b9118a26f4a3', '11be2358-eead-44e8-8055-386242fd2075', 1, '15:00:00', '2025-10-26', 'Sessão de psicologia - Vale Real', 1, 1),
('d92ce7aa-4632-4e43-bf22-b5a3728e09ff', 'acdaecde-6c02-4fe3-88ff-f561772ddecf', 1, '09:45:00', '2025-10-27', 'Encaminhamento social - Alto Feliz', 1, 1),
('e7e43c13-f01b-47f1-b0c2-f144b57d96e4', 'f7f86b8b-bed3-4175-b74b-35f4f4c5d8cf', 2, '10:15:00', '2025-10-28', 'Acompanhamento recorrente - Bom Princípio', 1, 2),
('f2437f33-7055-4f42-a06b-9a4a1ff1a0bb', '1d91790e-7c3c-4ac3-aadc-668fc8c12109', 1, '16:00:00', '2025-10-29', 'Atendimento de triagem - Feliz', 1, 3),
('fd9a88e1-9431-4b63-8f2a-5561f1a66d52', 'dbe528f4-56b6-4aa6-8f92-d34beb068b22', 2, '08:30:00', '2025-10-30', 'Revisão de caso clínico - Linha Nova', 1, 4);

-- ========================================
-- 2. AGENDAMENTO_ASSISTIDO (Tabela de junção)
-- ========================================
-- Campos: id, id_agendamento, id_assistido, status

INSERT INTO public.agendamento_assistido (id, id_agendamento, id_assistido, status) VALUES
-- Agendamento 1 (2 assistidos)
('a1d9c2f8-6a2a-4dcd-8f91-8b06f7a21b12', '3f1a89e4-7a10-4d6c-9e61-01b3f0c909f1', '0118f3c1-8e2f-43c0-97e8-10dfd0d8ba1a', 1),
('b3e6dcb7-9ef7-46e0-b0b2-9b48150a6b23', '3f1a89e4-7a10-4d6c-9e61-01b3f0c909f1', '03aeb138-f879-48f3-8635-d8c2377dc5a5', 1),

-- Agendamento 2 (1 assistido)
('c92ad589-1b58-4f72-90a1-0b541f776c3a', '4e6c2b7a-8b0f-48d3-a61c-7d830fef1d44', '0dae39bf-f0d9-4182-a15e-1c874f7472fb', 1),

-- Agendamento 3 (3 assistidos)
('d3e9326d-f4f5-4f42-b3b1-6ac64818d6b5', '8d93a821-d1a9-4b64-b36a-2db15a9a4323', '0b47e945-e439-4fd3-a2e0-b0cd2a7b91fb', 1),
('e1b2e478-4ed0-45a8-96c8-8df5ebae9122', '8d93a821-d1a9-4b64-b36a-2db15a9a4323', '0935a2c8-b68a-4685-893a-95a5d4e3d609', 1),
('f6acb297-c0ea-4d74-bf24-d94f32c56312', '8d93a821-d1a9-4b64-b36a-2db15a9a4323', '09b9e18c-23fe-4f1b-87db-bdb43532b40e', 1),

-- Agendamento 4 (1 assistido)
('a99db731-6c3f-4459-93b1-27e9b3c38e10', '22b8f59a-04d7-41aa-b11e-61adff7b9fd1', '08fe2177-bfbd-4547-a46b-ed4b8628d77e', 1),

-- Agendamento 5 (2 assistidos)
('b91d4e8a-d8c8-4a4e-bb5a-bfbc1927f2d3', 'ad1a2471-5e19-4b5a-8c8c-28a96ee9d771', '145a838e-2977-4739-812b-7af1eb107225', 1),
('c61ae7bb-4fd6-4d09-8829-91a6236d7a33', 'ad1a2471-5e19-4b5a-8c8c-28a96ee9d771', '2106996f-4055-4c1d-85bd-fef4814e2e9a', 1),

-- Agendamento 6 (1 assistido)
('d64d35f8-9cc3-4f4a-9df7-1b8d37935a0b', 'b3944a5b-84fc-4cc4-9f3a-b9118a26f4a3', '1d052f16-7d46-4f87-a085-7f67b95e14d8', 1),

-- Agendamento 7 (1 assistido)
('e72f9e26-40e8-43f0-bab8-87392f2299a2', 'd92ce7aa-4632-4e43-bf22-b5a3728e09ff', '215238cb-3a6f-4625-bea2-f1bcc08e7368', 1),

-- Agendamento 8 (3 assistidos)
('f41e7ad4-3bcd-4203-856f-1bdb9a7d8a76', 'e7e43c13-f01b-47f1-b0c2-f144b57d96e4', '16952610-8499-4327-9115-2fb593425b44', 1),
('a8e15de8-6a2e-4f54-8138-833b3d80a14a', 'e7e43c13-f01b-47f1-b0c2-f144b57d96e4', '296bcaa2-bbaa-461d-b07c-755dcb7e5a0a', 1),
('b1a7a6a6-41b1-49a1-a099-3b6f94284df9', 'e7e43c13-f01b-47f1-b0c2-f144b57d96e4', '220f880d-4286-43c7-8b26-1abb1801aa0c', 1),

-- Agendamento 9 (1 assistido)
('c19d4429-7d1f-4d53-9f7f-0f2e8bc123aa', 'f2437f33-7055-4f42-a06b-9a4a1ff1a0bb', '22c99a41-9da5-41f2-b831-85e80ecbcc2b', 1),

-- Agendamento 10 (2 assistidos)
('d12ee682-13f8-4709-88d8-9b381ecb3cf1', 'fd9a88e1-9431-4b63-8f2a-5561f1a66d52', '267c14de-9476-4629-afde-1508421a6da8', 1),
('e15f8f76-18f5-4a91-93a9-cb6e8c27c30c', 'fd9a88e1-9431-4b63-8f2a-5561f1a66d52', '290fef4c-6879-4d3b-a2f4-d6337155b568', 1);

-- ========================================
-- 3. ATENDIMENTOS
-- ========================================
-- Campos: id, id_agendamento, id_assistido, data_atendimento, presenca, avaliacao, observacao, status
-- Status Presença: 1=PRESENCA, 2=FALTA, 3=JUSTIFICADA

INSERT INTO public.atendimento (id, id_agendamento, id_assistido, data_atendimento, presenca, avaliacao, observacao, status) VALUES
('1001f3e2-1111-4c2b-9e01-23ffbcfa0f11', '3f1a89e4-7a10-4d6c-9e61-01b3f0c909f1', '0118f3c1-8e2f-43c0-97e8-10dfd0d8ba1a', '2025-10-22 09:00:00', 1, 'Ótima evolução inicial', 'Sem intercorrências.', 1),
('2002e4f3-2222-4c3b-8e02-33abccde1e22', '4e6c2b7a-8b0f-48d3-a61c-7d830fef1d44', '0dae39bf-f0d9-4182-a15e-1c874f7472fb', '2025-10-23 10:45:00', 1, 'Atendimento tranquilo', 'Assistido colaborativo.', 1),
('3003d5f4-3333-4c4c-7e03-44bacdff2e33', '8d93a821-d1a9-4b64-b36a-2db15a9a4323', '09b9e18c-23fe-4f1b-87db-bdb43532b40e', '2025-10-24 11:00:00', 1, 'Grupo engajado', 'Sessão produtiva.', 1),
('4004c6f5-4444-4c5d-6e04-55cadf001e44', '22b8f59a-04d7-41aa-b11e-61adff7b9fd1', '08fe2177-bfbd-4547-a46b-ed4b8628d77e', '2025-10-22 14:10:00', 1, 'Paciente estável', 'Consulta de rotina.', 1),
('5005b7f6-5555-4c6e-5e05-66dbf0121e55', 'ad1a2471-5e19-4b5a-8c8c-28a96ee9d771', '145a838e-2977-4739-812b-7af1eb107225', '2025-10-25 13:40:00', 2, 'Faltou', 'Não compareceu à avaliação.', 1),
('6006a8f7-6666-4c7f-4e06-77ecf1231e66', 'b3944a5b-84fc-4cc4-9f3a-b9118a26f4a3', '1d052f16-7d46-4f87-a085-7f67b95e14d8', '2025-10-26 15:05:00', 1, 'Melhoria cognitiva', 'Apresentou progresso significativo.', 1),
('7007a9f8-7777-4c8a-3e07-88fdf2341e77', 'e7e43c13-f01b-47f1-b0c2-f144b57d96e4', '16952610-8499-4327-9115-2fb593425b44', '2025-10-28 10:30:00', 1, 'Sessão grupal', 'Participou ativamente.', 1),
('8008b0f9-8888-4c9b-2e08-99eef3451e88', 'f2437f33-7055-4f42-a06b-9a4a1ff1a0bb', '22c99a41-9da5-41f2-b831-85e80ecbcc2b', '2025-10-29 16:10:00', 1, 'Encaminhado para fonoaudióloga', 'Triagem concluída com sucesso.', 1),
('9009c1fa-9999-4cac-1e09-aafff4561e99', 'fd9a88e1-9431-4b63-8f2a-5561f1a66d52', '267c14de-9476-4629-afde-1508421a6da8', '2025-10-30 08:45:00', 1, 'Caso clínico reavaliado', 'Revisão e acompanhamento feitos.', 1);

-- ========================================
-- VERIFICAÇÃO DOS DADOS INSERIDOS
-- ========================================

-- Verificar agendamentos inseridos
SELECT 
    a.id as agendamento_id,
    a.data_agendamento,
    a.horario_agendamento,
    a.observacao,
    COUNT(aa.id_assistido) as qtd_assistidos,
    COUNT(at.id) as qtd_atendimentos
FROM agendamento a
LEFT JOIN agendamento_assistido aa ON a.id = aa.id_agendamento
LEFT JOIN atendimento at ON a.id = at.id_agendamento
WHERE a.id IN (
    '3f1a89e4-7a10-4d6c-9e61-01b3f0c909f1',
    '4e6c2b7a-8b0f-48d3-a61c-7d830fef1d44',
    '8d93a821-d1a9-4b64-b36a-2db15a9a4323',
    '22b8f59a-04d7-41aa-b11e-61adff7b9fd1',
    'ad1a2471-5e19-4b5a-8c8c-28a96ee9d771',
    'b3944a5b-84fc-4cc4-9f3a-b9118a26f4a3',
    'd92ce7aa-4632-4e43-bf22-b5a3728e09ff',
    'e7e43c13-f01b-47f1-b0c2-f144b57d96e4',
    'f2437f33-7055-4f42-a06b-9a4a1ff1a0bb',
    'fd9a88e1-9431-4b63-8f2a-5561f1a66d52'
)
GROUP BY a.id, a.data_agendamento, a.horario_agendamento, a.observacao
ORDER BY a.data_agendamento, a.horario_agendamento;