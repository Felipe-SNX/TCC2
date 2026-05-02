from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.api.dependencies import get_db
from app.schemas.usuario import UsuarioCreate, UsuarioResponse
from app.crud import crud_usuario

router = APIRouter()

@router.post("/", response_model=UsuarioResponse, status_code=status.HTTP_201_CREATED)
def criar_usuario(usuario_in: UsuarioCreate, db: Session = Depends(get_db)):
    usuario_existente = crud_usuario.get_usuario_by_email(db, email=usuario_in.email)
    if usuario_existente:
        raise HTTPException(status_code=400, detail="Email já cadastrado.")
    return crud_usuario.create_usuario(db=db, usuario=usuario_in)

@router.get("/{usuario_id}", response_model=UsuarioResponse)
def obter_usuario(usuario_id: str, db: Session = Depends(get_db)):
    usuario = crud_usuario.get_usuario(db, usuario_id=usuario_id)
    if not usuario:
        raise HTTPException(status_code=404, detail="Usuário não encontrado.")
    return usuario
