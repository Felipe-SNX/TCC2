import random
from sqlalchemy.orm import Session
from sqlalchemy import or_
from app.models.schema import Paciente
from app.schemas.paciente import PacienteCreate, PacienteUpdate

def get_paciente(db: Session, paciente_id: str):
    return db.query(Paciente).filter(Paciente.id == paciente_id).first()

def get_paciente_by_email(db: Session, email: str):
    return db.query(Paciente).filter(Paciente.email == email).first()

def _get_pacientes_query(db: Session, user_id: str = None, user_role: str = None, user_created_by: str = None, search: str = None):
    query = db.query(Paciente)
    
    from app.models.schema import Usuario
    
    if user_role == 'PSICOLOGO' and user_id:
        query = query.filter(Paciente.created_by == user_id)
    elif user_role == 'ADMIN' and user_created_by is not None:
        # Admin não-raiz: pacientes criados por ele OU pelos usuários que ele pode ver
        visible_user_ids = db.query(Usuario.id).filter(
            Usuario.created_by.in_([user_id, user_created_by])
        ).all()
        ids = [user_id] + [r[0] for r in visible_user_ids]
        query = query.filter(Paciente.created_by.in_(ids))
        
    if search:
        termo = f"%{search}%"
        query = query.filter(
            or_(Paciente.nome.ilike(termo), Paciente.email.ilike(termo))
        )
    return query

def get_pacientes(db: Session, skip: int = 0, limit: int = 100, user_id: str = None, user_role: str = None, user_created_by: str = None, search: str = None):
    return _get_pacientes_query(db, user_id, user_role, user_created_by, search).offset(skip).limit(limit).all()

def get_pacientes_count(db: Session, user_id: str = None, user_role: str = None, user_created_by: str = None, search: str = None):
    return _get_pacientes_query(db, user_id, user_role, user_created_by, search).count()

def _generate_unique_pin(db: Session) -> str:
    while True:
        pin = f"{random.randint(0, 999999):06d}"
        if not db.query(Paciente).filter(Paciente.pin == pin).first():
            return pin

def create_paciente(db: Session, paciente: PacienteCreate, user_id: str = None):
    db_paciente = Paciente(**paciente.model_dump(), created_by=user_id)
    db_paciente.pin = _generate_unique_pin(db)
    db.add(db_paciente)
    db.commit()
    db.refresh(db_paciente)
    return db_paciente

def update_paciente(db: Session, paciente_id: str, paciente_in: PacienteUpdate, user_id: str = None):
    db_paciente = db.query(Paciente).filter(Paciente.id == paciente_id).first()
    if not db_paciente:
        return None
    update_data = paciente_in.model_dump(exclude_unset=True)
    for field, value in update_data.items():
        setattr(db_paciente, field, value)
    if user_id:
        db_paciente.updated_by = user_id
    db.commit()
    db.refresh(db_paciente)
    return db_paciente

def delete_paciente(db: Session, paciente_id: str):
    db_paciente = db.query(Paciente).filter(Paciente.id == paciente_id).first()
    if not db_paciente:
        return False
    db.delete(db_paciente)
    db.commit()
    return True

def regenerate_pin(db: Session, paciente_id: str):
    db_paciente = db.query(Paciente).filter(Paciente.id == paciente_id).first()
    if not db_paciente:
        return None
    db_paciente.pin = _generate_unique_pin(db)
    db.commit()
    db.refresh(db_paciente)
    return db_paciente
