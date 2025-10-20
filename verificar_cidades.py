#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import pandas as pd
import re

def extrair_cidade_uf(cidade_uf_str):
    """Extrai cidade e UF da string no formato 'Cidade/UF'"""
    if not cidade_uf_str or pd.isna(cidade_uf_str):
        return None, None
    
    # Remove espaços em branco
    cidade_uf_str = str(cidade_uf_str).strip()
    
    # Separa por / 
    if '/' in cidade_uf_str:
        partes = cidade_uf_str.split('/')
        if len(partes) >= 2:
            cidade = partes[0].strip()
            uf = partes[1].strip().upper()
            return cidade, uf
    
    return None, None

# Carrega o CSV
print("Carregando CSV...")
df = pd.read_csv('b1.csv', encoding='latin1', sep=';', skiprows=1)

# Extrai todas as cidades únicas
cidades_unicas = set()
for _, row in df.iterrows():
    nome = str(row.get('Nome', '')).strip()
    if not nome or nome == 'nan' or len(nome) < 2:
        continue  # Pula linhas sem nome
        
    cidade, uf = extrair_cidade_uf(row.get('Cidade/UF', ''))
    if cidade and uf:
        cidades_unicas.add((cidade.lower().strip(), uf))

print(f"\nCidades encontradas no CSV ({len(cidades_unicas)}):")
for cidade, uf in sorted(cidades_unicas):
    print(f"  {cidade} / {uf}")

# Mapeamento atual
MAPEAMENTO_CIDADES = {
    'antônio prado': '004a35e0-cad0-4bcf-aa4e-fd8d8f235c49',
    'ipê': '252b5a00-e687-4cbب-99b9-a62f175ea8b1',
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

print(f"\nCidades não mapeadas:")
nao_mapeadas = []
for cidade, uf in sorted(cidades_unicas):
    if cidade not in MAPEAMENTO_CIDADES:
        nao_mapeadas.append((cidade, uf))
        print(f"  ❌ {cidade} / {uf}")

print(f"\nTotal não mapeadas: {len(nao_mapeadas)}")