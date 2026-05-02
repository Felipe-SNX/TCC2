from sqlalchemy.orm import Session
from app.models.schema import Usuario
from app.schemas.usuario import UsuarioCreate
from app.core.security import get_password_hash

def get_usuario(db: Session, usuario_id: str):
    return db.query(Usuario).filter(Usuario.id == usuario_id).first()

def get_usuario_by_email(db: Session, email: str):
    return db.query(Usuario).filter(Usuario.email == email).first()

def get_usuarios(db: Session, skip: int = 0, limit: int = 100):
    return db.query(Usuario).offset(skip).limit(limit).all()

def get_usuarios_count(db: Session):
    return db.query(Usuario).count()

def create_usuario(db: Session, usuario: UsuarioCreate):
    hashed_password = get_password_hash(usuario.senha)
    db_usuario = Usuario(
        nome=usuario.nome,
        email=usuario.email,
        role=usuario.role,
        senha=hashed_password
    )
    db.add(db_usuario)
    db.commit()
    db.refresh(db_usuario)
    return db_usuario

