from fastapi import APIRouter, Depends, HTTPException, status, Query
from sqlalchemy.orm import Session
from app.api.dependencies import get_db, get_current_user, RoleChecker
from app.schemas.usuario import UsuarioCreate, UsuarioUpdate, UsuarioResponse, UsuarioPaginatedResponse
from app.models.schema import Usuario
from app.crud import crud_usuario

router = APIRouter()
allow_admin = RoleChecker(["ADMIN"])

@router.post("/", response_model=UsuarioResponse, status_code=status.HTTP_201_CREATED)
def criar_usuario(
    usuario_in: UsuarioCreate,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_admin)
):
    usuario_existente = crud_usuario.get_usuario_by_email(db, email=usuario_in.email)
    if usuario_existente:
        raise HTTPException(status_code=400, detail="Email já cadastrado.")
    return crud_usuario.create_usuario(db=db, usuario=usuario_in)

@router.get("/", response_model=UsuarioPaginatedResponse)
def listar_usuarios(
    page: int = Query(1, ge=1),
    items_per_page: int = Query(25, ge=1, le=100),
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_admin)
):
    skip = (page - 1) * items_per_page
    items = crud_usuario.get_usuarios(db=db, skip=skip, limit=items_per_page)
    total = crud_usuario.get_usuarios_count(db=db)
    return {"items": items, "total": total}

@router.get("/{usuario_id}", response_model=UsuarioResponse)
def obter_usuario(
    usuario_id: str,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_admin)
):
    usuario = crud_usuario.get_usuario(db, usuario_id=usuario_id)
    if not usuario:
        raise HTTPException(status_code=404, detail="Usuário não encontrado.")
    return usuario

@router.put("/{usuario_id}", response_model=UsuarioResponse)
def atualizar_usuario(
    usuario_id: str,
    usuario_in: UsuarioUpdate,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_admin)
):
    usuario = crud_usuario.update_usuario(db=db, usuario_id=usuario_id, usuario_in=usuario_in)
    if not usuario:
        raise HTTPException(status_code=404, detail="Usuário não encontrado.")
    return usuario

@router.delete("/{usuario_id}", status_code=status.HTTP_204_NO_CONTENT)
def excluir_usuario(
    usuario_id: str,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_admin)
):
    if current_user.id == usuario_id:
        raise HTTPException(status_code=400, detail="Não é possível excluir o próprio usuário.")
    deleted = crud_usuario.delete_usuario(db=db, usuario_id=usuario_id)
    if not deleted:
        raise HTTPException(status_code=404, detail="Usuário não encontrado.")
