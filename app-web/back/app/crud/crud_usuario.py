from sqlalchemy.orm import Session
from app.models.schema import Usuario
from app.schemas.usuario import UsuarioCreate, UsuarioRegister, UsuarioUpdate
from app.core.security import get_password_hash

def get_usuario(db: Session, usuario_id: str):
    return db.query(Usuario).filter(Usuario.id == usuario_id).first()

def get_usuario_by_email(db: Session, email: str):
    return db.query(Usuario).filter(Usuario.email == email).first()

def _get_usuarios_query(db: Session, user_id: str = None, user_role: str = None, user_created_by: str = None):
    query = db.query(Usuario)
    
    if user_role == 'ADMIN' and user_created_by is not None:
        # Admin não-raiz: vê usuários criados por ele e criados pelo pai (irmãos)
        query = query.filter(
            Usuario.created_by.in_([user_id, user_created_by]),
            Usuario.id != user_id
        )
    return query

def get_usuarios(db: Session, skip: int = 0, limit: int = 100, user_id: str = None, user_role: str = None, user_created_by: str = None):
    return _get_usuarios_query(db, user_id, user_role, user_created_by).offset(skip).limit(limit).all()

def get_usuarios_count(db: Session, user_id: str = None, user_role: str = None, user_created_by: str = None):
    return _get_usuarios_query(db, user_id, user_role, user_created_by).count()

def create_usuario(db: Session, usuario: UsuarioCreate, created_by_id: str = None):
    hashed_password = get_password_hash(usuario.senha)
    db_usuario = Usuario(
        nome=usuario.nome,
        email=usuario.email,
        role=usuario.role,
        senha=hashed_password,
        ativo=True,
        created_by=created_by_id
    )
    db.add(db_usuario)
    db.commit()
    db.refresh(db_usuario)
    return db_usuario

def create_usuario_registro(db: Session, usuario: UsuarioRegister):
    """Cria um usuário via auto-registro. Sempre ADMIN e sempre inicia ativo."""
    hashed_password = get_password_hash(usuario.senha)
    db_usuario = Usuario(
        nome=usuario.nome,
        email=usuario.email,
        role='ADMIN',
        senha=hashed_password,
        ativo=True
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

def update_senha_by_email(db: Session, email: str, nova_senha: str):
    """Atualiza a senha de um usuário pelo email. Usado na redefinição de senha."""
    db_usuario = db.query(Usuario).filter(Usuario.email == email).first()
    if not db_usuario:
        return None
    db_usuario.senha = get_password_hash(nova_senha)
    db.commit()
    db.refresh(db_usuario)
    return db_usuario
