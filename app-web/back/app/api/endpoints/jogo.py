from fastapi import APIRouter, Depends, HTTPException, Request, status
from sqlalchemy.orm import Session
import logging
from app.api.dependencies import get_db
from app.schemas.resposta import RespostaGameCreate, RespostaCreate
from app.crud.crud_resposta import create_resposta
from app.models.schema import Paciente
from app.core.limiter import limiter

logger = logging.getLogger(__name__)

router = APIRouter()

@router.post("/respostas", status_code=status.HTTP_201_CREATED)
@limiter.limit("20/minute")
def salvar_resposta_do_jogo(request: Request, resposta_in: RespostaGameCreate, db: Session = Depends(get_db)):
    """
    Endpoint dedicado a receber os dados vindos do Jogo Unity.
    - Verifica as credenciais do paciente (email e pin).
    - Salva a resposta no banco.
    """

    logger.info("Entrou no endpoint", exc_info=True)

    paciente = db.query(Paciente).filter(
        Paciente.email == resposta_in.email,
        Paciente.pin == resposta_in.pin
    ).first()

    logger.info("Paciente criado", exc_info=True)

    if not paciente:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="E-mail ou PIN incorretos."
        )

    try:
        nova_resposta = RespostaCreate(
            id_paciente=paciente.id,
            currentLevel=resposta_in.currentLevel,
            time=resposta_in.time,
            tries=resposta_in.tries,
            response=resposta_in.response,
            colectables=resposta_in.colectables
        )
        create_resposta(db=db, resposta=nova_resposta)
        return {"message": "Resposta salva com sucesso."}
    except Exception as e:
        db.rollback()
        logger.error("Falha ao persistir resposta do paciente: %s", e, exc_info=True)
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail="Erro interno ao salvar a resposta."
        )

