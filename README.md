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
    "Key": "SUA_CHAVE_ANONIMA_DO_SUPABASE"
  },
  "JWT": {
    "Key": "SUA_CHAVE_SECRETA_JWT_AQUI_MINIMO_32_CARACTERES",
    "Issuer": "SistemaApae"
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "SEU_EMAIL@gmail.com",
    "SmtpPassword": "SUA_SENHA_DE_APP",
    "FromEmail": "SEU_EMAIL@gmail.com",
    "FromName": "Sistema APAE"
  }
}
```

**Importante:**

- Substitua os valores de `Supabase:Url` e `Supabase:Key` com as credenciais do seu projeto no Supabase
- Gere uma chave JWT forte (mínimo 32 caracteres aleatórios)
- Configure o SMTP para envio de e-mails (se usar Gmail, precisa gerar uma senha de app)

#### 1.3. Restaurar dependências

```powershell
cd SistemaApae.Api
dotnet restore
```

#### 1.4. Executar o backend

```powershell
dotnet run
```

O backend estará disponível em: `http://localhost:5000` ou `https://localhost:5001`

Para verificar a API, acesse o Swagger: `https://localhost:5001/swagger`

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
   - **Project URL** (Supabase:Url)
   - **anon/public key** (Supabase:Key)
4. Execute os scripts SQL fornecidos na pasta raiz do projeto para criar as tabelas e popular o banco de dados:

   - `data_base.sql` - Cria a base de dados e adiciona o usuario administrador (email: admin@apae, senha: JDbggsev3Ogv)
   - `inserir_cidades_municipios.sql` - Insere os municípios
   - `assistidos_da_planilha.sql` - Insere os assistidos

   **Como executar os scripts:**

   - Acesse o painel do Supabase
   - Vá em **SQL Editor**
   - Copie e cole o conteúdo de cada arquivo SQL
   - Execute os scripts na ordem listada acima

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
