from sqlalchemy.orm import Session
from app.models.schema import Pergunta
from app.schemas.pergunta import PerguntaCreate, PerguntaUpdate

def get_pergunta(db: Session, pergunta_id: str):
    return db.query(Pergunta).filter(Pergunta.id == pergunta_id).first()

def get_perguntas(db: Session, skip: int = 0, limit: int = 100):
    return db.query(Pergunta).offset(skip).limit(limit).all()

def create_pergunta(db: Session, pergunta: PerguntaCreate, user_id: str = None):
    db_pergunta = Pergunta(**pergunta.model_dump(), created_by=user_id)
    db.add(db_pergunta)
    db.commit()
    db.refresh(db_pergunta)
    return db_pergunta

def update_pergunta(db: Session, pergunta_id: str, pergunta: PerguntaUpdate, user_id: str = None):
    db_pergunta = get_pergunta(db, pergunta_id)
    if not db_pergunta:
        return None
        
    update_data = pergunta.model_dump(exclude_unset=True)
    for key, value in update_data.items():
        setattr(db_pergunta, key, value)
        
    db_pergunta.updated_by = user_id
    db.commit()
    db.refresh(db_pergunta)
    return db_pergunta
