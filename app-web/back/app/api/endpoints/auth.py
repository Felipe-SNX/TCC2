from datetime import timedelta
from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.api.dependencies import get_db
from app.core.security import verify_password, create_access_token
from app.core.config import settings
from app.crud.crud_usuario import get_usuario_by_email
from app.schemas.token import Token, LoginSchema

router = APIRouter()

@router.post("/psychologist", response_model=Token)
def login_for_access_token(login_data: LoginSchema, db: Session = Depends(get_db)):
    usuario = get_usuario_by_email(db, email=login_data.email)
    if not usuario:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="E-mail ou senha incorretos",
            headers={"WWW-Authenticate": "Bearer"},
        )
    if not verify_password(login_data.password, usuario.senha):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="E-mail ou senha incorretos",
            headers={"WWW-Authenticate": "Bearer"},
        )
    
    if usuario.role not in ['PSICOLOGO', 'ADMIN']:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Acesso negado. Apenas psicólogos podem acessar este painel.",
        )

    access_token_expires = timedelta(minutes=settings.ACCESS_TOKEN_EXPIRE_MINUTES)
    access_token = create_access_token(
        data={"sub": usuario.email, "role": usuario.role}, expires_delta=access_token_expires
    )
    return {"access_token": access_token, "token_type": "bearer"}
