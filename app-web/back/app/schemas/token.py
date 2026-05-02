from pydantic import BaseModel

class UsuarioBase(BaseModel):
    id: str
    nome: str
    email: str
    role: str

class Token(BaseModel):
    access_token: str
    token_type: str
    user: UsuarioBase

class TokenData(BaseModel):
    email: str | None = None

class LoginSchema(BaseModel):
    email: str
    password: str
