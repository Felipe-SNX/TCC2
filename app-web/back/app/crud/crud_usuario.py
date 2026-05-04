from sqlalchemy.orm import Session
from app.models.schema import Usuario
from app.schemas.usuario import UsuarioCreate, UsuarioRegister, UsuarioUpdate
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
        senha=hashed_password,
        ativo=True  # Criado por admin → ativo imediatamente
    )
    db.add(db_usuario)
    db.commit()
    db.refresh(db_usuario)
    return db_usuario

def create_usuario_registro(db: Session, usuario: UsuarioRegister):
    """Cria um usuário via auto-registro. Sempre PSICOLOGO e sempe inicia inativo."""
    hashed_password = get_password_hash(usuario.senha)
    db_usuario = Usuario(
        nome=usuario.nome,
        email=usuario.email,
        role='PSICOLOGO',
        senha=hashed_password,
        ativo=False
    )
    db.add(db_usuario)
    db.commit()
    db.refresh(db_usuario)
    return db_usuario

def update_usuario(db: Session, usuario_id: str, usuario_in: UsuarioUpdate):
    db_usuario = db.query(Usuario).filter(Usuario.id == usuario_id).first()
    if not db_usuario:
        return None
    update_data = usuario_in.model_dump(exclude_unset=True)
    if "senha" in update_data and update_data["senha"]:
        update_data["senha"] = get_password_hash(update_data["senha"])
    for field, value in update_data.items():
        setattr(db_usuario, field, value)
    db.commit()
    db.refresh(db_usuario)
    return db_usuario

def toggle_ativo(db: Session, usuario_id: str):
    """Inverte o status ativo/inativo do usuário."""
    db_usuario = db.query(Usuario).filter(Usuario.id == usuario_id).first()
    if not db_usuario:
        return None
    db_usuario.ativo = not db_usuario.ativo
    db.commit()
    db.refresh(db_usuario)
    return db_usuario

def delete_usuario(db: Session, usuario_id: str):
    db_usuario = db.query(Usuario).filter(Usuario.id == usuario_id).first()
    if not db_usuario:
        return False
    db.delete(db_usuario)
    db.commit()
    return True
