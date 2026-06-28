from datetime import timedelta
from fastapi import APIRouter, Depends, HTTPException, status
from fastapi.responses import JSONResponse
from sqlalchemy.orm import Session
from app.api.dependencies import get_db
from app.core.security import verify_password, create_access_token, create_password_reset_token, verify_password_reset_token
from app.core.config import settings
from app.crud.crud_usuario import get_usuario_by_email, create_usuario_registro, update_senha_by_email
from app.schemas.token import Token, LoginSchema, EsqueciSenhaSchema, RedefinirSenhaSchema
from app.schemas.usuario import UsuarioRegister, UsuarioResponse

router = APIRouter()

@router.post("/login", response_model=Token)
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
            detail="Acesso negado. Apenas psicólogos e administradores podem acessar este painel.",
        )

    if not usuario.ativo:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Sua conta está inativa. Entre em contato com um administrador para ativá-la.",
        )

    access_token_expires = timedelta(minutes=settings.ACCESS_TOKEN_EXPIRE_MINUTES)
    access_token = create_access_token(
        data={"sub": usuario.email, "role": usuario.role}, expires_delta=access_token_expires
    )
    return {
        "access_token": access_token, 
        "token_type": "bearer",
        "user": {
            "id": usuario.id,
            "nome": usuario.nome,
            "email": usuario.email,
            "role": usuario.role
        }
    }

@router.post("/registrar", response_model=UsuarioResponse, status_code=status.HTTP_201_CREATED)
def registrar_usuario(usuario_in: UsuarioRegister, db: Session = Depends(get_db)):
    """Registro público de novo usuário. Sempre como ADMIN e ativo=True."""
    existente = get_usuario_by_email(db, email=usuario_in.email)
    if existente:
        raise HTTPException(status_code=400, detail="Este e-mail já está cadastrado.")
    return create_usuario_registro(db=db, usuario=usuario_in)

@router.post("/esqueci-senha")
def esqueci_senha(data: EsqueciSenhaSchema, db: Session = Depends(get_db)):
    """
    Sempre responde com sucesso para não revelar se o email existe no sistema.
    Em background, verifica e gera o token de reset.
    """
    usuario = get_usuario_by_email(db, email=data.email)

    if usuario:
        reset_token = create_password_reset_token(email=usuario.email)
        print(f"[DEBUG] Token de redefinição para {usuario.email}: {reset_token}")

    return JSONResponse(
        status_code=200,
        content={"message": "Solicitação recebida com sucesso."}
    )

@router.post("/redefinir-senha")
def redefinir_senha(data: RedefinirSenhaSchema, db: Session = Depends(get_db)):
    """Redefine a senha do usuário usando o token de recuperação."""
    email = verify_password_reset_token(data.token)
    if not email:
        raise HTTPException(
            status_code=400,
            detail="Token inválido ou expirado. Solicite uma nova redefinição de senha."
        )

    usuario = update_senha_by_email(db=db, email=email, nova_senha=data.nova_senha)
    if not usuario:
        raise HTTPException(status_code=404, detail="Usuário não encontrado.")

    return JSONResponse(
        status_code=200,
        content={"message": "Senha redefinida com sucesso."}
    )
