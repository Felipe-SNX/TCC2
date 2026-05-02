import os
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from dotenv import load_dotenv

# Carrega as variáveis do .env na raiz do backend
load_dotenv()

# Pega a URL do banco de dados (mesma lógica usada no Alembic)
SQLALCHEMY_DATABASE_URL = os.getenv("DATABASE_URL")

if not SQLALCHEMY_DATABASE_URL:
    raise ValueError("A variável DATABASE_URL não foi encontrada. Verifique o arquivo .env.")

# Cria a engine do SQLAlchemy
engine = create_engine(SQLALCHEMY_DATABASE_URL)

# Cria a classe base de sessões
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
