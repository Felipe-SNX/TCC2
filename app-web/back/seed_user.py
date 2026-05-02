import sys
import os

# Adiciona a raiz do backend no path para evitar erro de modulo "app" não encontrado
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from app.db.session import SessionLocal
from app.models.schema import Usuario
from app.core.security import get_password_hash

def seed_user():
    db = SessionLocal()
    try:
        email = "admin@chromotherapy.com"
        if db.query(Usuario).filter(Usuario.email == email).first():
            print("O usuário já existe no banco de dados!")
            return
        
        novo_usuario = Usuario(
            nome="Psicólogo Teste",
            email=email,
            role="PSICOLOGO",
            senha=get_password_hash("admin123")
        )
        db.add(novo_usuario)
        db.commit()
        print(f"Usuário criado com sucesso!\nE-mail: {email}\nSenha: admin123")
    except Exception as e:
        print(f"Erro ao criar o usuário: {e}")
    finally:
        db.close()

if __name__ == "__main__":
    seed_user()
