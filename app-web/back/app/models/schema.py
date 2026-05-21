import uuid
from datetime import datetime
from sqlalchemy import Column, String, Integer, Text, ForeignKey, DateTime, Enum, JSON, Boolean
from app.db.base import Base

class Usuario(Base):
    __tablename__ = "usuarios"
    
    id = Column(String(36), primary_key=True, default=lambda: str(uuid.uuid4()))
    role = Column(Enum('PSICOLOGO', 'ADMIN', name="role_enum"), nullable=False, default='PSICOLOGO')
    nome = Column(String(255), nullable=False)
    email = Column(String(255), unique=True, nullable=False)
    senha = Column(String(255), nullable=False)
    ativo = Column(Boolean, nullable=False, default=False)

class Paciente(Base):
    __tablename__ = "pacientes"
    
    id = Column(String(36), primary_key=True, default=lambda: str(uuid.uuid4()))
    nome = Column(String(255), nullable=False)
    idade = Column(Integer, nullable=False)
    email = Column(String(255), unique=True, nullable=False)
    observacoes = Column(Text, nullable=True)
    created_at = Column(DateTime, default=datetime.utcnow)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    created_by = Column(String(36), ForeignKey("usuarios.id"))
    updated_by = Column(String(36), ForeignKey("usuarios.id"))

class PacientePsicologo(Base):
    __tablename__ = "paciente_psicologo"
    
    id_paciente = Column(String(36), ForeignKey("pacientes.id"), primary_key=True)
    id_usuario = Column(String(36), ForeignKey("usuarios.id"), primary_key=True)

class Pergunta(Base):
    __tablename__ = "perguntas"
    
    id = Column(String(36), primary_key=True, default=lambda: str(uuid.uuid4()))
    pergunta = Column(String(255), nullable=False)
    alternativas = Column(JSON, nullable=False)
    ativo = Column(Boolean, nullable=False, default=True)
    created_at = Column(DateTime, default=datetime.utcnow)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    created_by = Column(String(36), ForeignKey("usuarios.id"))
    updated_by = Column(String(36), ForeignKey("usuarios.id"))

class Resposta(Base):
    __tablename__ = "respostas"
    
    id = Column(String(36), primary_key=True, default=lambda: str(uuid.uuid4()))
    id_paciente = Column(String(36), ForeignKey("pacientes.id"), nullable=False)
    id_pergunta = Column(String(36), ForeignKey("perguntas.id"), nullable=True)
    resposta = Column(Integer, nullable=False)
    cor = Column(String(50), nullable=True)
    created_at = Column(DateTime, default=datetime.utcnow)