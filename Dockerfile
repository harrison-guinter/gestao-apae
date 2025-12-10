# Usa a versão mais recente e estável do Node 22
FROM node:22-alpine

WORKDIR /app

COPY package*.json ./

RUN npm install -g @angular/cli && npm install --legacy-peer-deps

# Copia o restante dos arquivos do projeto
COPY . .

EXPOSE 4200

CMD ["npx", "ng", "serve", "--host", "0.0.0.0", "--port", "4200"]
