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
        users_to_create = [
            {
                "nome": "Administrador",
                "email": "admin@admin.com",
                "role": "ADMIN",
                "senha": "123"
            },
            {
                "nome": "Psicólogo",
                "email": "psico@psico.com",
                "role": "PSICOLOGO",
                "senha": "123"
            }
        ]

        for user_data in users_to_create:
            if db.query(Usuario).filter(Usuario.email == user_data["email"]).first():
                print(f"O usuário {user_data['email']} já existe no banco de dados!")
                continue
            
            novo_usuario = Usuario(
                nome=user_data["nome"],
                email=user_data["email"],
                role=user_data["role"],
                senha=get_password_hash(user_data["senha"])
            )
            db.add(novo_usuario)
            print(f"Usuário {user_data['nome']} ({user_data['role']}) preparado para criação.")
        
        db.commit()
        print("Usuários criados com sucesso!")
    except Exception as e:
        db.rollback()
        print(f"Erro ao criar os usuários: {e}")
    finally:
        db.close()

if __name__ == "__main__":
    seed_user()
