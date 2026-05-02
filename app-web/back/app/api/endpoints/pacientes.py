from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from typing import List
from app.api.dependencies import get_db
from app.schemas.paciente import PacienteCreate, PacienteResponse
from app.schemas.resposta import RespostaResponse
from app.crud import crud_paciente, crud_resposta

router = APIRouter()

@router.post("/", response_model=PacienteResponse, status_code=status.HTTP_201_CREATED)
def criar_paciente(paciente_in: PacienteCreate, db: Session = Depends(get_db)):
    paciente_existente = crud_paciente.get_paciente_by_email(db, email=paciente_in.email)
    if paciente_existente:
        raise HTTPException(status_code=400, detail="Email já cadastrado.")
    return crud_paciente.create_paciente(db=db, paciente=paciente_in)

@router.get("/", response_model=List[PacienteResponse])
def listar_pacientes(skip: int = 0, limit: int = 100, db: Session = Depends(get_db)):
    return crud_paciente.get_pacientes(db=db, skip=skip, limit=limit)

@router.get("/{paciente_id}", response_model=PacienteResponse)
def obter_paciente(paciente_id: str, db: Session = Depends(get_db)):
    paciente = crud_paciente.get_paciente(db, paciente_id=paciente_id)
    if not paciente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado.")
    return paciente

@router.get("/{paciente_id}/respostas", response_model=List[RespostaResponse])
def listar_respostas_do_paciente(paciente_id: str, db: Session = Depends(get_db)):
    """
    Endpoint importante para o psicólogo ver o histórico de respostas do paciente
    através do dashboard Nuxt.
    """
    return crud_resposta.get_respostas_by_paciente(db, paciente_id=paciente_id)
