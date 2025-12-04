Tutorial de instalação do Ambiente.

Para este ambiente foi utilizado um sistema Linux Ubuntu Server 24.04LTS.

Passo 1: Instalar o docker, conforme orientado no link https://docs.docker.com/engine/install/ubuntu/

Passo 2: Baixar a pasta do sistema conforme repositório, ex: GitHub, GitLabs, etc.

Passo 3: Acessar o diretório ./gestao-apae/SistemaApae.App

Passo 4: Buildar a imagem docker utilizada para rodar o front-end através do comando "docker build -t nome_da_imagem:tag", onde a tag pode ser substituído pela versão, ex: latest, o arquivo contendo os parâmetros necessários se chama Dockerfile.

Passo 5: Após buildar a imagem, basta executar o comando "docker compose up -d" para subir o ambiente, os parâmetros estão descritos no arquivo docker-compose.yml

Passo 6: Liberar portas 8080 e 4200 no VPS.

Passo 7: Acessar o sistema através do ip do servidor + porta 4200
