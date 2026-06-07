from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.api.dependencies import get_db
from app.schemas.resposta import RespostaGameCreate, RespostaResponse, CredenciaisPaciente, CredenciaisPacienteResponse
from app.crud.crud_resposta import create_resposta_from_game
from app.models.schema import Paciente

router = APIRouter()

@router.post("/respostas", response_model=RespostaResponse, status_code=status.HTTP_201_CREATED)
def salvar_resposta_do_jogo(resposta_in: RespostaGameCreate, db: Session = Depends(get_db)):
    """
    Endpoint dedicado a receber os dados vindos do Jogo Unity.
    - Recebe o id_paciente, valor da resposta (1-5) e a cor.
    """
    paciente = db.query(Paciente).filter(Paciente.id == resposta_in.id_paciente).first()
    if not paciente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado.")

    try:
        resposta = create_resposta_from_game(db=db, resposta_in=resposta_in)
        return resposta
    except Exception as e:
        db.rollback()
        raise HTTPException(status_code=500, detail=f"Erro ao salvar a resposta: {str(e)}")

@router.post("/verificar-credenciais", response_model=CredenciaisPacienteResponse, status_code=status.HTTP_200_OK)
def verificar_credenciais_paciente(dados: CredenciaisPaciente, db: Session = Depends(get_db)):
    """
    Verifica se um paciente existe no sistema através do e-mail e PIN.
    Retorna o id_paciente caso os dados estejam corretos.
    """
    paciente = db.query(Paciente).filter(
        Paciente.email == dados.email,
        Paciente.pin == dados.pin
    ).first()
    if not paciente:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="E-mail ou PIN incorretos."
        )
    return {"id_paciente": paciente.id}
