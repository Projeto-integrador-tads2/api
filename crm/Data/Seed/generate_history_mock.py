import csv
import random
import uuid
from datetime import datetime, timedelta

stages = ["Análise de Perfil", "Conversa com o Cliente", "Negociação", "Fechamento"]
sectors = ["Serviços", "Comércio", "Indústria", "Tecnologia", "Saúde", "Educação"]
origins = ["Indicação", "Site", "Evento", "Ligação", "Email", "Rede Social"]
responsibles = ["Carlos", "Ana", "João", "Mariana", "Pedro", "Fernanda", "Lucas", "Juliana"]
priority_levels = ["Baixa", "Média", "Alta"]
target_labels = ["pouco provavel", "provavel", "muito provavel"]
def random_company_name():

    prefix = random.choice(["Alpha", "Beta", "Delta", "Omega", "Prime", "Max", "Global", "Next"])
    suffix = random.choice(["Tech", "Solutions", "Group", "Consult", "Corp", "Systems", "Services"])
    return f"{prefix}{suffix}"

def choose_priority():
    # 20% alta, 50% média, 30% baixa
    return random.choices(priority_levels, weights=[0.3, 0.5, 0.2])[0]

def get_target(priority, next_stage):
    # Lógica para target categórico com acentos
    if next_stage == "Fechamento":
        if priority == "Alta":
            return "muito provável"
        elif priority == "Média":
            return random.choices(["provável", "muito provável"], weights=[0.6, 0.4])[0]
        else:
            return random.choices(["pouco provável", "provável"], weights=[0.7, 0.3])[0]
    elif next_stage == "Negociação":
        if priority == "Alta":
            return random.choices(["provável", "muito provável"], weights=[0.5, 0.5])[0]
        elif priority == "Média":
            return "provável"
        else:
            return random.choices(["pouco provável", "provável"], weights=[0.8, 0.2])[0]
    else:
        if priority == "Alta":
            return random.choices(["provável", "muito provável"], weights=[0.7, 0.3])[0]
        elif priority == "Média":
            return random.choices(["pouco provável", "provável"], weights=[0.5, 0.5])[0]
        else:
            return "pouco provável"

def generate_card():
    card_id = str(uuid.uuid4())
    value = random.randint(5000, 50000)
    sector = random.choice(sectors)
    priority = choose_priority()
    created_date = datetime(2025, random.randint(1, 11), random.randint(1, 28))
    total_moves = random.randint(2, 8)
    moves = []
    current_date = created_date
    current_stage = stages[0]
    for i in range(total_moves):
        next_stage_idx = min(stages.index(current_stage) + random.choice([1, 1, 1, 0]), len(stages)-1)
        next_stage = stages[next_stage_idx]
        days_in_stage = random.randint(1, 15)
        current_date += timedelta(days=days_in_stage)
        target = get_target(priority, next_stage)
        moves.append([
            str(uuid.uuid4()), current_stage, next_stage,
            current_date.strftime("%Y-%m-%d"), i+1,
            (current_date - created_date).days, days_in_stage, value, sector, priority, target
        ])
        current_stage = next_stage
        # Simula fechamento ou perda
        if next_stage == "Fechamento" and target == "muito provável":
            break
    return moves

rows = []
for _ in range(600):  # 600 cards, cada um com 2-5 moves, total ~2000 linhas
    rows.extend(generate_card())

with open("history_mock.csv", "w", newline="", encoding="utf-8") as f:
    writer = csv.writer(f)
    writer.writerow([
        "history_id", "from_stage", "to_stage", "moved_at",
        "total_moves", "days_since_creation", "current_stage_duration", "value", "sector", "prioridade", "target"
    ])
    writer.writerows(rows)

print(f"Arquivo 'history_mock.csv' gerado com {len(rows)} linhas.")
