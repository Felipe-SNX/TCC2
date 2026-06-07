import uuid
from datetime import datetime
from sqlalchemy import Column, String, Integer, Float, Text, ForeignKey, DateTime, Enum, Boolean
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
    pin = Column(String(6), unique=True, nullable=True)
    observacoes = Column(Text, nullable=True)
    created_at = Column(DateTime, default=datetime.utcnow)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    created_by = Column(String(36), ForeignKey("usuarios.id"))
    updated_by = Column(String(36), ForeignKey("usuarios.id"))

class Resposta(Base):
    __tablename__ = "respostas"
    
    id = Column(String(36), primary_key=True, default=lambda: str(uuid.uuid4()))
    id_paciente = Column(String(36), ForeignKey("pacientes.id"), nullable=False)
    currentLevel = Column(String(50), nullable=False)
    time = Column(Float, nullable=False)
    tries = Column(Integer, nullable=False)
    response = Column(Integer, nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow)