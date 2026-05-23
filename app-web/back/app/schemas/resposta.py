from pydantic import BaseModel, ConfigDict
from datetime import datetime
from typing import Optional

class RespostaBase(BaseModel):
    id_paciente: str
    resposta: int
    cor: Optional[str] = None
    id_pergunta: Optional[str] = None

class RespostaCreate(RespostaBase):
    pass

# Schema específico para receber os dados do Jogo Unity
class RespostaGameCreate(BaseModel):
    id_paciente: str
    id_pergunta: str
    resposta: int
    cor: str

class CredenciaisPaciente(BaseModel):
    email: str
    pin: str

class CredenciaisPacienteResponse(BaseModel):
    id_paciente: str

class RespostaResponse(RespostaBase):
    id: str
    created_at: datetime

    model_config = ConfigDict(from_attributes=True)

class RespostaPerguntaCreate(BaseModel):
    id_paciente: str
    id_pergunta: str
    resposta: int
