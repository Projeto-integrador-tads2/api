from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
from enum import Enum
import pickle
import numpy as np

# Carregue o modelo e scaler
with open("model_rf.pkl", "rb") as f:
    model = pickle.load(f)
with open("scaler.pkl", "rb") as f:
    scaler = pickle.load(f)


# Defina todas as categorias possíveis (use exatamente as do treino!)
from_stages = ["Análise de Perfil", "Conversa com o Cliente", "Fechamento", "Negociação"]
to_stages = ["Análise de Perfil", "Conversa com o Cliente", "Fechamento", "Negociação"]
sectors = ["Comércio", "Educação", "Indústria", "Saúde", "Serviços", "Tecnologia"]
prioridades = ["Alta", "Baixa", "Média"]

# Enum para as classes previstas
class PredictionClass(str, Enum):
    muito_provavel = "Muito Provável"
    provavel = "Provável"
    pouco_provavel = "Pouco Provável"

app = FastAPI(
    title="API de Predição de Probabilidade",
    description="Recebe dados de oportunidades e retorna a probabilidade de sucesso.",
    version="1.0.0"
)

class PredictResponse(BaseModel):
    result: PredictionClass = Field(..., description="Classe prevista pelo modelo.")

    class Config:
        schema_extra = {
            "example": {
                "result": PredictionClass.muito_provavel
            }
        }

class PredictRequest(BaseModel):
    from_stage: str = Field(..., description="Estágio de origem", example="Análise de Perfil")
    to_stage: str = Field(..., description="Estágio de destino", example="Fechamento")
    total_moves: int = Field(..., ge=0, description="Total de movimentações", example=3)
    days_since_creation: int = Field(..., ge=0, description="Dias desde a criação", example=15)
    current_stage_duration: int = Field(..., ge=0, description="Dias no estágio atual", example=5)
    value: float = Field(..., ge=0, description="Valor da oportunidade", example=10000.0)
    sector: str = Field(..., description="Setor do cliente", example="Tecnologia")
    prioridade: str = Field(..., description="Prioridade da oportunidade", example="Alta")

    def validate_fields(self):
        errors = []
        if self.from_stage not in from_stages:
            errors.append(f"from_stage deve ser um dos: {from_stages}")
        if self.to_stage not in to_stages:
            errors.append(f"to_stage deve ser um dos: {to_stages}")
        if self.sector not in sectors:
            errors.append(f"sector deve ser um dos: {sectors}")
        if self.prioridade not in prioridades:
            errors.append(f"prioridade deve ser um dos: {prioridades}")
        if errors:
            raise ValueError("; ".join(errors))

def make_onehot(value, possibles):
    return [int(value == p) for p in possibles]

@app.post(
    "/predict",
    summary="Predição de probabilidade",
    response_description="Classe prevista",
    response_model=PredictResponse
)
def predict(req: PredictRequest):
    # Validação dos campos categóricos
    try:
        req.validate_fields()
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))

    # One-hot manual
    x_onehot = []
    x_onehot += make_onehot(req.from_stage, from_stages)
    x_onehot += make_onehot(req.to_stage, to_stages)
    x_onehot += make_onehot(req.sector, sectors)
    x_onehot += make_onehot(req.prioridade, prioridades)

    # Numéricos escalados (reshape para scaler)
    num_data = np.array([[req.total_moves, req.days_since_creation, req.current_stage_duration, req.value]])
    num_scaled = scaler.transform(num_data)[0].tolist()

    # Vetor final
    final_features = np.array(x_onehot + num_scaled).reshape(1, -1)

    # Predição
    pred = model.predict(final_features)

    CLASS_MAPPING = {
        0: PredictionClass.muito_provavel,
        1: PredictionClass.pouco_provavel,
        2: PredictionClass.provavel,
    }

    return {
        "result": CLASS_MAPPING[int(pred[0])],
    }