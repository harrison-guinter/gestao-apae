# Sistema de Gestão APAE

Sistema completo de gestão para APAE, com backend em ASP.NET Core e frontend em Angular.

## 📋 Pré-requisitos

### Backend (.NET)

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Conta no [Supabase](https://supabase.com/) (banco de dados PostgreSQL)

### Frontend (Angular)

- [Node.js](https://nodejs.org/) (versão 18.x ou superior)
- [npm](https://www.npmjs.com/) (geralmente instalado com Node.js)
- [Angular CLI](https://angular.io/cli) (versão 19.x)

---

## 🚀 Configuração do Ambiente

### 1. Backend (SistemaApae.Api)

#### 1.1. Verificar instalação do .NET

```powershell
dotnet --version
```

Se não estiver instalado, baixe em: https://dotnet.microsoft.com/download/dotnet/9.0

#### 1.2. Configurar variáveis de ambiente

Crie um arquivo `appsettings.Development.json` na pasta `SistemaApae.Api` com o seguinte conteúdo:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Supabase": {
    "Url": "SUA_URL_DO_SUPABASE",
    "AnonKey": "SUA_CHAVE_ANONIMA_DO_SUPABASE",
    "ServiceRoleKey": "OPCIONAL_CHAVE_SERVICE_ROLE_SE_USAR_OPERACOES_PRIVILEGIADAS"
  },
  "JWT": {
    "Key": "SUA_CHAVE_SECRETA_JWT_AQUI_MINIMO_32_CARACTERES",
    "Issuer": "SistemaApae"
  },
  "EmailSettings": {
    "SmtpServer": "smtp-relay.brevo.com",
    "Port": 587,
    "SenderName": "Sistema APAE",
    "SenderEmail": "no-reply@seudominio.com",
    "Username": "SEU_USUARIO_SMTP_BREVO",
    "Password": "SUA_SENHA_SMTP_BREVO",
    "EnableSsl": true
  }
}
```

**Importante:**

- Substitua os valores de `Supabase:Url` e `Supabase:Key` com as credenciais do seu projeto no Supabase
- Gere uma chave JWT forte (mínimo 32 caracteres aleatórios)
- Configure o SMTP para envio de e-mails (se usar Gmail, precisa gerar uma senha de app)

#### 🔑 Como gerar a JWT_KEY (chave secreta JWT)
Use uma chave aleatória forte (32 bytes ou mais). Algumas formas práticas:

- OpenSSL (Linux/macOS/Windows com OpenSSL instalado):

```bash
openssl rand -base64 32
```

- Node.js (qualquer SO):

```bash
node -e "console.log(require('crypto').randomBytes(32).toString('base64'))"
```

- PowerShell (Windows):

```powershell
$bytes = New-Object 'System.Byte[]' 32
(New-Object System.Security.Cryptography.RNGCryptoServiceProvider).GetBytes($bytes)
[Convert]::ToBase64String($bytes)
```

Copie o valor gerado e configure:
- Em desenvolvimento: no `appsettings.Development.json` em `JWT:Key`
- Em produção: no `./docker-compose.yml` em `JWT_KEY`

#### 1.3. Restaurar dependências

```powershell
cd SistemaApae.Api
dotnet restore
```

#### 1.4. Executar o backend

```powershell
dotnet run
```

O backend estará disponível em: `http://localhost:8080` ou `https://localhost:8080`

Para verificar a API, acesse o Swagger: `https://localhost:8080/docs`

---

### ✉️ SMTP (Brevo recomendado)

Para envio de e-mails transacionais (criação de usuário e recuperação de senha), recomendamos usar a Brevo.

- Site: [`https://www.brevo.com/pt/`](https://www.brevo.com/pt/)

#### Como obter as credenciais na Brevo
1. Crie uma conta na Brevo e acesse o painel: [`https://www.brevo.com/pt/`](https://www.brevo.com/pt/)
2. Valide um remetente (endereço de e-mail) ou um domínio:
   - Menu “Remetentes e IPs” → “Remetentes” → “Adicionar um remetente” (ou valide seu domínio).
3. Gere a senha SMTP:
   - Menu “SMTP & API” → “SMTP” → Gere/visualize a “Senha SMTP” (chave).
4. Anote os dados de conexão:
   - Servidor: `smtp-relay.brevo.com`
   - Usuário: fornecido pela Brevo (ex.: `xxxxxxxx@smtp-brevo.com`)
   - Senha: sua “Senha SMTP” gerada no passo anterior

Referência: [`https://www.brevo.com/pt/`](https://www.brevo.com/pt/)

#### Como configurar no sistema
Passo a passo desde as credenciais até o ambiente:

1) Produção — cadastrar credenciais via `docker-compose.yml`

- Produção — docker-compose (arquivo: `./docker-compose.yml`)

```yaml
# docker-compose.yml (exemplo)
version: "3.8"
services:
  api:
    build:
      context: .
      dockerfile: SistemaApae.Api/Dockerfile
    environment:
      SMTP_SERVER_EMAIL: "smtp-relay.brevo.com"
      PORT_EMAIL: "2525"
      USE_SSL_EMAIL: "true"                  # true para TLS/SSL
      SENDER_EMAIL: "no-reply@seudominio.com"
      SENDER_NAME_EMAIL: "Sistema APAE"
      USERNAME_EMAIL: "SEU_USUARIO_SMTP_BREVO"   # ex.: xxxxxxxx@smtp-brevo.com
      PASSWORD_EMAIL: "SUA_SENHA_SMTP_BREVO"
    # ports, networks, etc...
```

Após ajustar, suba os containers:

```bash
docker compose up -d --build
```

2) Desenvolvimento — cadastrar credenciais no `appsettings.Development.json`

Desenvolvimento local — `appsettings.Development.json`:

```json
"EmailSettings": {
  "SmtpServer": "smtp-relay.brevo.com",
  "Port": 2525,
  "SenderName": "Sistema APAE",
  "SenderEmail": "no-reply@seudominio.com",
  "Username": "SEU_USUARIO_SMTP_BREVO",
  "Password": "SUA_SENHA_SMTP_BREVO",
  "EnableSsl": true
}
```

3) Testar o envio
- O sistema envia e-mails nas ações de “Criar usuário” e “Esqueci minha senha”.
- Garanta que o remetente usado (`SenderEmail`) esteja validado na Brevo.
- Em caso de falha, revise host/porta/SSL e usuário/senha SMTP.

Observações:
- Use um remetente validado na Brevo (e-mail ou domínio).
- Não versione credenciais sensíveis no repositório.
---

### 2. Frontend (SistemaApae.App)

#### 2.1. Verificar instalação do Node.js

```powershell
node --version
npm --version
```

Se não estiver instalado, baixe em: https://nodejs.org/

#### 2.2. Instalar Angular CLI globalmente

```powershell
npm install -g @angular/cli@19
```

Verificar instalação:

```powershell
ng version
```

#### 2.3. Instalar dependências do projeto

```powershell
cd SistemaApae.App
npm install
```

#### 2.4. Configurar ambiente de desenvolvimento

Edite o arquivo `src/environments/environment.ts` e configure a URL da API local:

```typescript
export const environment = {
  production: false,
  apiUrl: "http://localhost:5000/api/",
};
```

**Nota:** Ajuste a porta conforme a configuração do seu backend.

#### 2.5. Executar o frontend

```powershell
ng serve
```

Ou use o script do package.json:

```powershell
npm start
```

O frontend estará disponível em: `http://localhost:4200`

---

## 🔧 Comandos Úteis

### Backend

```powershell
# Restaurar dependências
dotnet restore

# Compilar o projeto
dotnet build

# Executar o projeto
dotnet run

# Executar em modo watch (recarrega ao salvar)
dotnet watch run

# Criar build de produção
dotnet publish -c Release
```

### Frontend

```powershell
# Instalar dependências
npm install

# Executar em modo desenvolvimento
ng serve

# Executar e abrir no navegador
ng serve --open

# Executar em porta customizada
ng serve --port 4300

# Compilar para produção
ng build --configuration production

# Executar testes
ng test

# Verificar problemas de lint
ng lint
```

---

## 🗄️ Configuração do Banco de Dados (Supabase)

1. Crie uma conta em [Supabase](https://supabase.com/)
2. Crie um novo projeto
3. Acesse as configurações do projeto e copie:
   - **Project URL** (`Supabase:Url` / `SUPABASE_URL`)
   - **Anon/Public API Key** (`Supabase:AnonKey` / `SUPABASE_ANON_KEY`)
   - (Opcional) **Service Role Key** (`Supabase:ServiceRoleKey` / `SUPABASE_SERVICE_ROLE_KEY`) — apenas se você precisar executar operações privilegiadas do servidor
4. Execute os scripts SQL fornecidos na pasta raiz do projeto para criar as tabelas e popular o banco de dados:

   - `data_base.sql` - Cria a base de dados e adiciona o usuario administrador (email: admin@apae, senha: JDbggsev3Ogv)
   - `municipio_rows.sql` - Insere os municípios
   - `inserir_cidades_municipios.sql` - Insere o vinculo entre os municípios e os assistidos
   - `assistidos_da_planilha.sql` - Insere os assistidos

   **Como executar os scripts:**

   - Acesse o painel do Supabase
   - Vá em **SQL Editor**
   - Copie e cole o conteúdo de cada arquivo SQL
   - Execute os scripts na ordem listada acima

### Como configurar Supabase no sistema
Passo a passo desde as chaves até o ambiente:

1) Produção — cadastrar credenciais via `docker-compose.yml`

- Produção — docker-compose (arquivo: `./docker-compose.yml`)

```yaml
# docker-compose.yml (exemplo)
version: "3.8"
services:
  api:
    build:
      context: .
      dockerfile: SistemaApae.Api/Dockerfile
    environment:
      SUPABASE_URL: "https://SEU_PROJETO.supabase.co"
      SUPABASE_ANON_KEY: "SUA_CHAVE_ANONIMA_DO_SUPABASE"
      # Opcional: apenas se precisar de operações de servidor com privilégios
      SUPABASE_SERVICE_ROLE_KEY: "SUA_SERVICE_ROLE_KEY"
    # ports, networks, etc...
```

Após ajustar, suba os containers:

```bash
docker compose up -d --build
```

2) Desenvolvimento — cadastrar credenciais no `appsettings.Development.json`

- Desenvolvimento local — `appsettings.Development.json`

Adicione/ajuste a seção abaixo no arquivo `SistemaApae.Api/appsettings.Development.json`:

```json
"Supabase": {
  "Url": "https://SEU_PROJETO.supabase.co",
  "AnonKey": "SUA_CHAVE_ANONIMA_DO_SUPABASE",
  "ServiceRoleKey": "OPCIONAL_CHAVE_SERVICE_ROLE_SE_USAR_OPERACOES_PRIVILEGIADAS"
}
```

3) Testar a conexão com o Supabase

Com o backend rodando, acesse:

```bash
curl -X GET http://localhost:5000/api/public/supabase-status
```

Se tudo estiver correto, você verá `Connected: true` na resposta.

Observações:
- Não versione credenciais sensíveis no repositório.
- Use a `ServiceRoleKey` apenas no backend/servidor e somente quando necessário.
- Garanta que a URL e as chaves correspondam ao seu projeto no Supabase.

---

## 📁 Estrutura do Projeto

```
gestao-apae/
├── SistemaApae.Api/          # Backend (ASP.NET Core)
│   ├── Controllers/          # Endpoints da API
│   ├── Models/              # Modelos de dados
│   ├── Repositories/        # Camada de acesso a dados
│   ├── Services/            # Lógica de negócio
│   └── Program.cs           # Configuração da aplicação
│
├── SistemaApae.App/         # Frontend (Angular)
│   ├── src/
│   │   ├── app/            # Componentes e módulos
│   │   ├── environments/   # Configurações de ambiente
│   │   └── styles/         # Estilos globais
│   └── angular.json        # Configuração do Angular
│
└── README.md               # Este arquivo
```

---

## 🔐 Segurança

- **Nunca** commite o arquivo `appsettings.Development.json` com dados sensíveis
- Adicione `appsettings.Development.json` ao `.gitignore`
- Use variáveis de ambiente em produção
- Mantenha as chaves JWT e credenciais do Supabase em segurança

---

## 🐛 Problemas Comuns

### Backend não inicia

- Verifique se a porta 5000/5001 não está em uso
- Confirme se o .NET 9.0 SDK está instalado
- Verifique as configurações do `appsettings.Development.json`

### Frontend não compila

- Execute `npm install` novamente
- Limpe o cache: `npm cache clean --force`
- Verifique se a versão do Node.js é compatível (18.x+)
- Delete a pasta `node_modules` e execute `npm install` novamente

### Erro de CORS

- Certifique-se de que o backend está configurado para aceitar requisições do frontend
- Verifique a URL da API no arquivo `environment.ts`

### Erro de conexão com Supabase

- Verifique se as credenciais estão corretas
- Confirme se o projeto no Supabase está ativo
- Teste a conexão diretamente pelo console do Supabase
