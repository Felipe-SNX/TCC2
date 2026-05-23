from pydantic import BaseModel, EmailStr, ConfigDict
from typing import Optional, List
from datetime import datetime

class PacienteBase(BaseModel):
    nome: str
    idade: int
    email: EmailStr
    observacoes: Optional[str] = None

class PacienteCreate(PacienteBase):
    pass

class PacienteUpdate(BaseModel):
    nome: Optional[str] = None
    idade: Optional[int] = None
    email: Optional[EmailStr] = None
    observacoes: Optional[str] = None

class PacienteResponse(PacienteBase):
    id: str
    pin: Optional[str] = None
    created_at: datetime
    updated_at: datetime
    created_by: Optional[str] = None
    updated_by: Optional[str] = None

    model_config = ConfigDict(from_attributes=True)

class PacientePaginatedResponse(BaseModel):
    items: List[PacienteResponse]
    total: int
