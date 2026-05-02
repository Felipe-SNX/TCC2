from sqlalchemy.orm import Session
from app.models.schema import Paciente, PacientePsicologo
from app.schemas.paciente import PacienteCreate, PacienteUpdate

def get_paciente(db: Session, paciente_id: str):
    return db.query(Paciente).filter(Paciente.id == paciente_id).first()

def get_paciente_by_email(db: Session, email: str):
    return db.query(Paciente).filter(Paciente.email == email).first()

def _get_pacientes_query(db: Session, user_id: str = None, user_role: str = None):
    query = db.query(Paciente)
    if user_role == 'PSICOLOGO' and user_id:
        query = query.outerjoin(PacientePsicologo, PacientePsicologo.id_paciente == Paciente.id)\
                     .filter((Paciente.created_by == user_id) | (PacientePsicologo.id_usuario == user_id))
    return query

def get_pacientes(db: Session, skip: int = 0, limit: int = 100, user_id: str = None, user_role: str = None):
    return _get_pacientes_query(db, user_id, user_role).offset(skip).limit(limit).all()

def get_pacientes_count(db: Session, user_id: str = None, user_role: str = None):
    return _get_pacientes_query(db, user_id, user_role).count()

def create_paciente(db: Session, paciente: PacienteCreate, user_id: str = None):
    # Converte o modelo Pydantic para um dicionário
    db_paciente = Paciente(**paciente.model_dump(), created_by=user_id)
    db.add(db_paciente)
    db.commit()
    db.refresh(db_paciente)
    return db_paciente
