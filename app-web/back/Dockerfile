# Usa uma imagem oficial do Python como base
FROM python:3.12-slim

# Define o diretório de trabalho dentro da caixinha
WORKDIR /app

# Copia o arquivo de dependências primeiro (otimização de cache do Docker)
COPY requirements.txt .

# Instala as dependências Python
RUN pip install --no-cache-dir -r requirements.txt

# Copia todo o restante do código do backend
COPY . .

# Expõe a porta que o uvicorn vai usar
EXPOSE 8000

# O comando de inicialização fica no docker-compose (para poder rodar migrations antes)
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
