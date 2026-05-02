from pydantic import BaseModel, EmailStr, ConfigDict
from typing import Optional, List
from enum import Enum

class RoleEnum(str, Enum):
    PSICOLOGO = 'PSICOLOGO'
    ADMIN = 'ADMIN'

class UsuarioBase(BaseModel):
    nome: str
    email: EmailStr
    role: RoleEnum = RoleEnum.PSICOLOGO

class UsuarioCreate(UsuarioBase):
    senha: str

class UsuarioUpdate(BaseModel):
    nome: Optional[str] = None
    email: Optional[EmailStr] = None
    role: Optional[RoleEnum] = None
    senha: Optional[str] = None

class UsuarioResponse(UsuarioBase):
    id: str

    model_config = ConfigDict(from_attributes=True)

class UsuarioPaginatedResponse(BaseModel):
    items: List[UsuarioResponse]
    total: int
