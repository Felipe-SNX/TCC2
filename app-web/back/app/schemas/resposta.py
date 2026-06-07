from pydantic import BaseModel, ConfigDict
from datetime import datetime
from typing import Optional

class RespostaBase(BaseModel):
    id_paciente: str
    currentLevel: str
    time: float
    tries: int
    response: int

class RespostaCreate(RespostaBase):
    pass

# Schema específico para receber os dados do Jogo Unity
class RespostaGameCreate(BaseModel):
    currentLevel: str
    time: float
    tries: int
    response: int
    email: str
    pin: str

class RespostaResponse(RespostaBase):
    id: str
    created_at: datetime

    model_config = ConfigDict(from_attributes=True)

