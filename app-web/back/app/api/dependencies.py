from typing import Generator
from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from jose import jwt, JWTError
from sqlalchemy.orm import Session
from app.db.session import SessionLocal
from app.core.config import settings
from app.crud.crud_usuario import get_usuario_by_email
from app.schemas.token import TokenData

oauth2_scheme = OAuth2PasswordBearer(tokenUrl="/api/v1/auth/login")

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

def get_current_user(token: str = Depends(oauth2_scheme), db: Session = Depends(get_db)):
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Não foi possível validar as credenciais",
        headers={"WWW-Authenticate": "Bearer"},
    )
    try:
        payload = jwt.decode(token, settings.SECRET_KEY, algorithms=[settings.ALGORITHM])
        email: str = payload.get("sub")
        if email is None:
            raise credentials_exception
        token_data = TokenData(email=email)
    except JWTError:
        raise credentials_exception
        
    usuario = get_usuario_by_email(db, email=token_data.email)
    if usuario is None:
        raise credentials_exception
    return usuario

from app.models.schema import Usuario
from app.crud.crud_paciente import get_paciente

def verify_paciente_access(
    paciente_id: str,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(get_current_user)
) -> Usuario:
    """
    Dependência de segurança global:
    Garante que o usuário tenha acesso ao paciente específico pelo path param paciente_id.
    """
    if current_user.role not in ["ADMIN", "PSICOLOGO"]:
        raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado.")
    
    if current_user.role == "PSICOLOGO":
        paciente = get_paciente(db, paciente_id=paciente_id)
        if not paciente or paciente.created_by != current_user.id:
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado ou paciente não encontrado.")
            
    return current_user
