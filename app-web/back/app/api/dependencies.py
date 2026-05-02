from typing import Generator
from app.db.session import SessionLocal

def get_db() -> Generator:
    """
    Função de dependência do FastAPI para criar e fechar a sessão do banco de dados 
    automaticamente a cada requisição.
    """
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()
