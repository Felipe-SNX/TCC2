from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.api.dependencies import get_db
from app.schemas.resposta import RespostaGameCreate, RespostaResponse
from app.crud.crud_resposta import create_resposta_from_game

router = APIRouter()

@router.post("/respostas", response_model=RespostaResponse, status_code=status.HTTP_201_CREATED)
def salvar_resposta_do_jogo(resposta_in: RespostaGameCreate, db: Session = Depends(get_db)):
    """
    Endpoint dedicado a receber os dados vindos do Jogo Unity.
    - Recebe o email_paciente, valor da resposta (1-5) e a cor.
    - O banco vincula pelo e-mail e salva.
    """
    resposta = create_resposta_from_game(db=db, resposta_in=resposta_in)
    if not resposta:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Paciente não encontrado para o e-mail fornecido."
        )
    return resposta
