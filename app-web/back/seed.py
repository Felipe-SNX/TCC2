"""
seed.py — Seed centralizado do banco de dados.

Ordem de execução:
  1. Usuários (ADMIN e PSICOLOGO)
  2. Pacientes (com PIN único de 6 dígitos)
  3. Perguntas
"""

import sys
import os
import random
import string

# Garante que o diretório raiz do backend esteja no path
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from app.db.session import SessionLocal
from app.models.schema import Usuario, Paciente, PacientePsicologo, Pergunta
from app.core.security import get_password_hash


# ─────────────────────────────────────────────
# Helpers
# ─────────────────────────────────────────────

def gerar_pin_unico(db) -> str:
    """Gera um PIN numérico de 6 dígitos único na tabela de pacientes."""
    while True:
        pin = "".join(random.choices(string.digits, k=6))
        existe = db.query(Paciente).filter(Paciente.pin == pin).first()
        if not existe:
            return pin


# ─────────────────────────────────────────────
# 1. Usuários
# ─────────────────────────────────────────────

def seed_usuarios(db):
    print("\n📋 [1/3] Criando usuários...")

    users_to_create = [
        {
            "nome": "Administrador",
            "email": "admin@admin.com",
            "role": "ADMIN",
            "senha": "123",
        },
        {
            "nome": "Psicólogo",
            "email": "psico@psico.com",
            "role": "PSICOLOGO",
            "senha": "123",
        },
    ]

    for user_data in users_to_create:
        if db.query(Usuario).filter(Usuario.email == user_data["email"]).first():
            print(f"  ⚠️  Usuário {user_data['email']} já existe — pulando.")
            continue

        novo = Usuario(
            nome=user_data["nome"],
            email=user_data["email"],
            role=user_data["role"],
            senha=get_password_hash(user_data["senha"]),
        )
        db.add(novo)
        print(f"  ✅ {user_data['nome']} ({user_data['role']}) criado.")

    db.commit()
    print("  Usuários OK.")


# ─────────────────────────────────────────────
# 2. Pacientes
# ─────────────────────────────────────────────

def seed_pacientes(db):
    print("\n👥 [2/3] Criando pacientes...")

    psicologos = db.query(Usuario).filter(Usuario.role == "PSICOLOGO").all()
    if not psicologos:
        print("  ❌ Nenhum psicólogo encontrado — pulando criação de pacientes.")
        return

    nomes_masculinos = [
        "Gabriel", "Lucas", "Mateus", "João", "Pedro",
        "Felipe", "Enzo", "Guilherme", "Rafael", "Gustavo",
    ]
    nomes_femininos = [
        "Ana", "Julia", "Beatriz", "Maria", "Alice",
        "Laura", "Sophia", "Valentina", "Heloisa", "Manuela",
    ]
    sobrenomes = [
        "Silva", "Santos", "Oliveira", "Souza", "Rodrigues",
        "Ferreira", "Alves", "Pereira", "Lima", "Gomes",
    ]
    observacoes_templates = [
        "Apresenta sintomas de ansiedade leve em situações sociais.",
        "Demonstra interesse em atividades lúdicas durante as sessões.",
        "Relata melhora no sono após início do acompanhamento.",
        "Foco em desenvolvimento de inteligência emocional.",
        "Paciente participativo e comunicativo.",
        "Apresenta dificuldades de concentração em tarefas escolares.",
        "Em fase de observação quanto a oscilações de humor.",
        "Busca auxílio para lidar com luto recente.",
        "Trabalhando técnicas de relaxamento e mindfulness.",
        "Paciente demonstra evolução constante no tratamento.",
        "Relata dificuldades de relacionamento no ambiente de trabalho.",
        "Demonstra grande resiliência diante de desafios pessoais.",
        "Foco em melhorar a autoestima e autoconfiança.",
        "Paciente apresenta quadros ocasionais de estresse agudo.",
    ]

    total_criado = 0
    for psico in psicologos:
        print(f"  → Psicólogo: {psico.nome} ({psico.email})")
        for _ in range(40):
            sexo = random.choice(["M", "F"])
            nome = random.choice(nomes_masculinos if sexo == "M" else nomes_femininos)
            sobrenome = f"{random.choice(sobrenomes)} {random.choice(sobrenomes)}"
            nome_completo = f"{nome} {sobrenome}"

            # E-mail único
            email = f"{nome.lower()}.{sobrenome.split()[0].lower()}.{random.randint(100, 9999)}@exemplo.com"
            while db.query(Paciente).filter(Paciente.email == email).first():
                email = f"{nome.lower()}.{sobrenome.split()[0].lower()}.{random.randint(10000, 99999)}@exemplo.com"

            # PIN único de 6 dígitos
            pin = gerar_pin_unico(db)

            novo_paciente = Paciente(
                nome=nome_completo,
                idade=random.randint(6, 75),
                email=email,
                pin=pin,
                observacoes=random.choice(observacoes_templates),
                created_by=psico.id,
                updated_by=psico.id,
            )
            db.add(novo_paciente)
            db.flush()  # gera o ID antes de criar a associação

            relacao = PacientePsicologo(
                id_paciente=novo_paciente.id,
                id_usuario=psico.id,
            )
            db.add(relacao)
            total_criado += 1

    db.commit()
    print(f"  ✅ {total_criado} pacientes criados com sucesso.")


# ─────────────────────────────────────────────
# 3. Perguntas
# ─────────────────────────────────────────────

def seed_perguntas(db):
    print("\n❓ [3/3] Criando perguntas...")

    psicologos = db.query(Usuario).filter(Usuario.role == "PSICOLOGO").all()
    if not psicologos:
        print("  ❌ Nenhum psicólogo encontrado — pulando criação de perguntas.")
        return

    perguntas_textos = [
        "Como você se sente após observar a cor?",
        "Qual a intensidade da sensação de calma ao ver a cor?",
        "A cor aumenta sua energia?",
        "A cor melhora seu humor?",
        "A cor traz sensação de criatividade?",
        "Como a cor influencia seu estado de alerta?",
        "A cor gera sensação de leveza?",
        "A cor provoca sensação de introspecção?",
        "A cor tem efeito relaxante?",
        "A cor desperta sentimentos de tranquilidade?",
        "A cor aumenta sua motivação?",
        "A cor reduz seu nível de estresse?",
        "A cor traz sensação de estabilidade?",
        "A cor favorece a meditação?",
        "A cor aumenta a clareza mental?",
        "A cor afeta seu humor positivamente?",
        "A cor eleva sua criatividade?",
        "A cor influencia sua percepção de tempo?",
        "A cor ajuda na concentração?",
        "A cor reduz ansiedade?",
    ]

    alternativas_padrao = [
        {"texto": "Muito negativo", "valor": 1},
        {"texto": "Negativo",       "valor": 2},
        {"texto": "Neutro",         "valor": 3},
        {"texto": "Positivo",       "valor": 4},
        {"texto": "Muito positivo", "valor": 5},
    ]

    total_criado = 0
    for texto in perguntas_textos:
        # Verifica se a pergunta já existe (evita duplicatas em re-execuções)
        if db.query(Pergunta).filter(Pergunta.pergunta == texto).first():
            print(f"  ⚠️  Pergunta já existe — pulando: \"{texto[:50]}...\"")
            continue

        psico = random.choice(psicologos)
        nova_pergunta = Pergunta(
            pergunta=texto,
            alternativas=alternativas_padrao,
            created_by=psico.id,
            updated_by=psico.id,
        )
        db.add(nova_pergunta)
        total_criado += 1

    db.commit()
    print(f"  ✅ {total_criado} perguntas criadas com sucesso.")


# ─────────────────────────────────────────────
# Orquestrador principal
# ─────────────────────────────────────────────

def run_seed():
    print("=" * 50)
    print("🌱  Iniciando seed do banco de dados...")
    print("=" * 50)

    db = SessionLocal()
    try:
        seed_usuarios(db)
        seed_pacientes(db)
        seed_perguntas(db)
    except Exception as e:
        db.rollback()
        print(f"\n❌ Erro durante o seed: {e}")
        raise
    finally:
        db.close()

    print("\n" + "=" * 50)
    print("✅  Seed concluído com sucesso!")
    print("=" * 50)


if __name__ == "__main__":
    run_seed()
