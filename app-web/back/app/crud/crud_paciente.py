from sqlalchemy.orm import Session
from app.models.schema import Paciente
from app.schemas.paciente import PacienteCreate, PacienteUpdate

def get_paciente(db: Session, paciente_id: str):
    return db.query(Paciente).filter(Paciente.id == paciente_id).first()

def get_paciente_by_email(db: Session, email: str):
    return db.query(Paciente).filter(Paciente.email == email).first()

def get_pacientes(db: Session, skip: int = 0, limit: int = 100):
    return db.query(Paciente).offset(skip).limit(limit).all()

def create_paciente(db: Session, paciente: PacienteCreate, user_id: str = None):
    # Converte o modelo Pydantic para um dicionário
    db_paciente = Paciente(**paciente.model_dump(), created_by=user_id)
    db.add(db_paciente)
    db.commit()
    db.refresh(db_paciente)
    return db_paciente
