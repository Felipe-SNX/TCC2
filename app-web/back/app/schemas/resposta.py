from pydantic import BaseModel, ConfigDict, EmailStr, Field
from datetime import datetime

class RespostaBase(BaseModel):
    id_paciente: str
    currentLevel: str
    time: float = Field(ge=0.0)
    tries: int = Field(ge=0)
    response: int = Field(ge=1, le=5)
    colectables: int = Field(ge=0)

class RespostaCreate(RespostaBase):
    pass

class RespostaGameCreate(BaseModel):
    email: EmailStr
    pin: str = Field(min_length=6, max_length=6)
    currentLevel: str
    time: float = Field(ge=0.0)
    tries: int = Field(ge=0)
    response: int = Field(ge=1, le=5, description="Estado emocional do paciente (1 a 5)")
    colectables: int = Field(ge=0, description="Quantidade de coletáveis obtidos")

class RespostaResponse(RespostaBase):
    id: str
    created_at: datetime

    model_config = ConfigDict(from_attributes=True)

