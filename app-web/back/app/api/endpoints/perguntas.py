from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from typing import List
from app.api.dependencies import get_db
from app.schemas.pergunta import PerguntaCreate, PerguntaResponse
from app.crud import crud_pergunta

router = APIRouter()

@router.post("/", response_model=PerguntaResponse, status_code=status.HTTP_201_CREATED)
def criar_pergunta(pergunta_in: PerguntaCreate, db: Session = Depends(get_db)):
    return crud_pergunta.create_pergunta(db=db, pergunta=pergunta_in)

@router.get("/", response_model=List[PerguntaResponse])
def listar_perguntas(skip: int = 0, limit: int = 100, db: Session = Depends(get_db)):
    return crud_pergunta.get_perguntas(db=db, skip=skip, limit=limit)
