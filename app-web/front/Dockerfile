# Usa uma imagem oficial do Node como base
FROM node:20-slim

# Define o diretório de trabalho dentro da caixinha
WORKDIR /app

# Copia os arquivos de dependências primeiro (otimização de cache)
COPY package.json package-lock.json ./

# Instala as dependências do Node
RUN npm install

# Copia todo o restante do código do frontend
COPY . .

# Expõe a porta que o Nuxt dev server vai usar
EXPOSE 3000

# Inicia o servidor de desenvolvimento do Nuxt
CMD ["npm", "run", "dev"]
