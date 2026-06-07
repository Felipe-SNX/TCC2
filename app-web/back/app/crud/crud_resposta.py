from sqlalchemy.orm import Session
from app.models.schema import Resposta
from app.schemas.resposta import RespostaCreate, RespostaGameCreate

def get_respostas_by_paciente(db: Session, paciente_id: str, skip: int = 0, limit: int = 100):
    return db.query(Resposta).filter(Resposta.id_paciente == paciente_id).offset(skip).limit(limit).all()

def create_resposta(db: Session, resposta: RespostaCreate):
    db_resposta = Resposta(**resposta.model_dump())
    db.add(db_resposta)
    db.commit()
    db.refresh(db_resposta)
    return db_resposta

def create_resposta_from_game(db: Session, resposta_in: RespostaGameCreate):
    db_resposta = Resposta(
        id_paciente=resposta_in.id_paciente,
        resposta=resposta_in.resposta,
        cor=resposta_in.cor,
    )
    db.add(db_resposta)
    db.commit()
    db.refresh(db_resposta)
    return db_resposta
