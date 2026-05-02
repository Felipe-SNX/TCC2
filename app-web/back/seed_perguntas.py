import sys
import os
import random

# Adiciona a raiz do backend no path para evitar erro de modulo "app" não encontrado
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from app.db.session import SessionLocal
from app.models.schema import Usuario, Pergunta

def seed_perguntas():
    db = SessionLocal()
    try:
        psicologos = db.query(Usuario).filter(Usuario.role == "PSICOLOGO").all()
        if not psicologos:
            print("Nenhum psicólogo encontrado para associar as perguntas!")
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
            "A cor reduz ansiedade?"
        ]

        # Alternativas padrão (escala de 1 a 5)
        alternativas_padrao = [
            {"texto": "Muito negativo", "valor": 1},
            {"texto": "Negativo", "valor": 2},
            {"texto": "Neutro", "valor": 3},
            {"texto": "Positivo", "valor": 4},
            {"texto": "Muito positivo", "valor": 5}
        ]

        total_criado = 0
        for texto in perguntas_textos:
            psico = random.choice(psicologos)
            nova_pergunta = Pergunta(
                pergunta=texto,
                alternativas=alternativas_padrao,
                created_by=psico.id,
                updated_by=psico.id
            )
            db.add(nova_pergunta)
            total_criado += 1
        
        db.commit()
        print(f"\nSucesso! {total_criado} perguntas criadas e associadas a psicólogos.")
    except Exception as e:
        db.rollback()
        print(f"Erro ao criar as perguntas: {e}")
    finally:
        db.close()

if __name__ == "__main__":
    seed_perguntas()
