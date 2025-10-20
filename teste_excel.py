#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Script alternativo para processar planilha APAE com melhor compatibilidade
Este script tenta diferentes métodos para ler arquivos Excel problemáticos
"""

import pandas as pd
import os
import sys

def tentar_ler_excel(arquivo):
    """Tenta ler o arquivo Excel com diferentes métodos"""
    
    print(f"📄 Tentando ler arquivo: {arquivo}")
    
    # Lista de engines e configurações para tentar
    tentativas = [
        {'engine': 'xlrd', 'nome': 'xlrd (arquivos .xls antigos)'},
        {'engine': 'openpyxl', 'nome': 'openpyxl (arquivos .xlsx modernos)'},
        {'engine': 'xlrd', 'nome': 'xlrd com header personalizado', 'header': 0},
        {'engine': 'openpyxl', 'nome': 'openpyxl com header personalizado', 'header': 0},
    ]
    
    for i, config in enumerate(tentativas, 1):
        try:
            print(f"🔄 Tentativa {i}: {config['nome']}")
            
            # Parâmetros base
            params = {'engine': config['engine']}
            
            # Adiciona header se especificado
            if 'header' in config:
                params['header'] = config['header']
            
            # Tenta ler o arquivo
            df = pd.read_excel(arquivo, **params)
            
            if not df.empty:
                print(f"✅ Sucesso com {config['nome']}!")
                print(f"📊 Dados: {len(df)} linhas x {len(df.columns)} colunas")
                print(f"🏷️  Colunas: {list(df.columns)[:5]}..." if len(df.columns) > 5 else f"🏷️  Colunas: {list(df.columns)}")
                return df
                
        except Exception as e:
            print(f"❌ Falhou: {str(e)}")
            continue
    
    # Se todas as tentativas falharam
    print("\n❌ TODAS AS TENTATIVAS FALHARAM!")
    print("\n🔧 SOLUÇÕES:")
    print("1. Abrir o arquivo no Excel e salvar como .xlsx")
    print("2. Salvar como CSV e processar como CSV")
    print("3. Verificar se o arquivo não está corrompido")
    print("4. Instalar/atualizar dependências:")
    print("   pip install --upgrade pandas openpyxl xlrd")
    
    return None

def main():
    arquivo = 'b1.xls'
    
    print("=== LEITOR DE EXCEL COMPATÍVEL ===")
    
    # Verifica se o arquivo existe
    if not os.path.exists(arquivo):
        print(f"❌ Arquivo '{arquivo}' não encontrado!")
        print(f"📁 Pasta atual: {os.getcwd()}")
        print(f"📋 Arquivos disponíveis:")
        for f in os.listdir('.'):
            if f.endswith(('.xls', '.xlsx', '.csv')):
                print(f"   📄 {f}")
        return
    
    # Tenta ler o arquivo
    df = tentar_ler_excel(arquivo)
    
    if df is not None:
        print("\n🎉 ARQUIVO LIDO COM SUCESSO!")
        
        # Mostra preview dos dados
        print("\n📋 Preview dos dados:")
        print(df.head())
        
        # Salva como CSV para facilitar processamento posterior
        csv_name = arquivo.replace('.xls', '_convertido.csv')
        df.to_csv(csv_name, index=False, encoding='utf-8')
        print(f"\n💾 Arquivo salvo como CSV: {csv_name}")
        print("🔄 Agora você pode processar o CSV com mais facilidade!")
        
    else:
        print("\n💡 DICA: Tente abrir o arquivo no Excel e salvar como .xlsx ou .csv")

if __name__ == "__main__":
    main()