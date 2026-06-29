from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    # Mudar em produção
    SECRET_KEY: str = "72d9e60298a01121d5a7114b74828f323a67dcf49c4c1a2d5e2cf94c03b12345"
    ALGORITHM: str = "HS256"
    ACCESS_TOKEN_EXPIRE_MINUTES: int = 60 * 24 * 7 # 7 dias de expiração
    DATABASE_URL: str = ""

    class Config:
        env_file = ".env"
        extra = "ignore"

settings = Settings()
