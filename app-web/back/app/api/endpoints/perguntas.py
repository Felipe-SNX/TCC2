from fastapi import APIRouter, Depends, HTTPException, status, Query
from sqlalchemy.orm import Session
from app.api.dependencies import get_db, get_current_user, RoleChecker
from app.schemas.pergunta import PerguntaCreate, PerguntaUpdate, PerguntaResponse, PerguntaPaginatedResponse
from app.models.schema import Usuario
from app.crud import crud_pergunta

router = APIRouter()
allow_psicologo_admin = RoleChecker(["ADMIN", "PSICOLOGO"])

@router.post("/", response_model=PerguntaResponse, status_code=status.HTTP_201_CREATED)
def criar_pergunta(
    pergunta_in: PerguntaCreate,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    return crud_pergunta.create_pergunta(db=db, pergunta=pergunta_in, user_id=current_user.id)

@router.get("/", response_model=PerguntaPaginatedResponse)
def listar_perguntas(
    page: int = Query(1, ge=1),
    items_per_page: int = Query(25, ge=1, le=100),
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    skip = (page - 1) * items_per_page
    items = crud_pergunta.get_perguntas(db=db, skip=skip, limit=items_per_page)
    total = crud_pergunta.get_perguntas_count(db=db)
    return {"items": items, "total": total}

@router.get("/{pergunta_id}", response_model=PerguntaResponse)
def obter_pergunta(
    pergunta_id: str,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    pergunta = crud_pergunta.get_pergunta(db, pergunta_id=pergunta_id)
    if not pergunta:
        raise HTTPException(status_code=404, detail="Pergunta não encontrada.")
    return pergunta

@router.put("/{pergunta_id}", response_model=PerguntaResponse)
def atualizar_pergunta(
    pergunta_id: str,
    pergunta_in: PerguntaUpdate,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    pergunta = crud_pergunta.update_pergunta(db=db, pergunta_id=pergunta_id, pergunta=pergunta_in, user_id=current_user.id)
    if not pergunta:
        raise HTTPException(status_code=404, detail="Pergunta não encontrada.")
    return pergunta

@router.delete("/{pergunta_id}", status_code=status.HTTP_204_NO_CONTENT)
def excluir_pergunta(
    pergunta_id: str,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    deleted = crud_pergunta.delete_pergunta(db=db, pergunta_id=pergunta_id)
    if not deleted:
        raise HTTPException(status_code=404, detail="Pergunta não encontrada.")
