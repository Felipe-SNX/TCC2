from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from sqlalchemy.sql.expression import func
from app.api.dependencies import get_db
from app.schemas.resposta import RespostaGameCreate, RespostaResponse, RespostaPerguntaCreate, CredenciaisPaciente, CredenciaisPacienteResponse
from app.schemas.pergunta import PerguntaResponse
from app.crud.crud_resposta import create_resposta_from_game, create_resposta_pergunta
from app.models.schema import PacientePsicologo, Pergunta, Paciente

router = APIRouter()

@router.post("/respostas", response_model=RespostaResponse, status_code=status.HTTP_201_CREATED)
def salvar_resposta_do_jogo(resposta_in: RespostaGameCreate, db: Session = Depends(get_db)):
    """
    Endpoint dedicado a receber os dados vindos do Jogo Unity.
    - Recebe o id_paciente, id_pergunta, valor da resposta (1-5) e a cor.
    """
    # Verifica se o paciente existe
    paciente = db.query(Paciente).filter(Paciente.id == resposta_in.id_paciente).first()
    if not paciente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado.")
        
    # Verifica se a pergunta existe
    pergunta = db.query(Pergunta).filter(Pergunta.id == resposta_in.id_pergunta).first()
    if not pergunta:
        raise HTTPException(status_code=404, detail="Pergunta não encontrada.")

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

@router.get("/{id_paciente}/pergunta-aleatoria", response_model=PerguntaResponse)
def get_pergunta_aleatoria(id_paciente: str, db: Session = Depends(get_db)):
    """
    Retorna uma pergunta aleatória ativa criada pelo psicólogo vinculado ao paciente.
    """
    # Verifica se o paciente existe
    paciente = db.query(Paciente).filter(Paciente.id == id_paciente).first()
    if not paciente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado")

    # Busca o vínculo do paciente com um psicólogo
    vinculo = db.query(PacientePsicologo).filter(PacientePsicologo.id_paciente == id_paciente).first()
    if not vinculo:
        raise HTTPException(status_code=404, detail="Psicólogo não vinculado ao paciente")
        
    id_psicologo = vinculo.id_usuario
    
    # Busca uma pergunta aleatória do psicólogo que esteja ativa
    pergunta = db.query(Pergunta).filter(
        Pergunta.created_by == id_psicologo,
        Pergunta.ativo == True
    ).order_by(func.random()).first()
    
    if not pergunta:
        raise HTTPException(status_code=404, detail="Nenhuma pergunta ativa encontrada para este psicólogo")
        
    return pergunta

@router.post("/resposta-pergunta", response_model=RespostaResponse, status_code=status.HTTP_201_CREATED)
def salvar_resposta_com_pergunta(resposta_in: RespostaPerguntaCreate, db: Session = Depends(get_db)):
    """
    Salva a resposta de um paciente para uma pergunta específica.
    """
    # Verifica se o paciente existe
    paciente = db.query(Paciente).filter(Paciente.id == resposta_in.id_paciente).first()
    if not paciente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado")
        
    # Verifica se a pergunta existe
    pergunta = db.query(Pergunta).filter(Pergunta.id == resposta_in.id_pergunta).first()
    if not pergunta:
        raise HTTPException(status_code=404, detail="Pergunta não encontrada")

    resposta = create_resposta_pergunta(db=db, resposta_in=resposta_in)
    return resposta
