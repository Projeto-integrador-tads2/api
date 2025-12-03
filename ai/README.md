# API de IA — Predição de Probabilidade de Sucesso

Esta API, desenvolvida em FastAPI, recebe dados de oportunidades de negócio e retorna a probabilidade de sucesso de acordo com um modelo de machine learning treinado.

## Como funciona

A API expõe um endpoint principal:

- `POST /predict` — Recebe um JSON com informações da oportunidade e retorna a classe prevista (Muito Provável, Provável, Pouco Provável).

### Exemplo de requisição

```json
{
    "from_stage": "Conversa com o Cliente",
    "to_stage": "Negociação",
    "total_moves": 3,
    "days_since_creation": 15,
    "current_stage_duration": 5,
    "value": 12000,
    "sector": "Serviços",
    "prioridade": "Média"
}
```

### Significado de cada campo do JSON

| Campo                   | Descrição                                                                 |
|------------------------ |--------------------------------------------------------------------------|
| `from_stage`            | Último stepColumn de origem (ex: "Conversa com o Cliente")               |
| `to_stage`              | Último stepColumn para o qual foi movido (ex: "Negociação")              |
| `total_moves`           | Total de movimentações do card entre etapas                               |
| `days_since_creation`   | Total de dias desde a criação do card no sistema                          |
| `current_stage_duration`| Total de dias no stepColumn atual                                         |
| `value`                 | Valor da oportunidade (card)                                             |
| `sector`                | Setor do cliente (ex: "Serviços") — **(ainda não existe no back-end)**   |
| `prioridade`            | Prioridade da oportunidade (ex: "Média") — **(ainda não existe no back-end)** |

## Como rodar localmente

1. Instale as dependências:
   ```bash
   pip install -r requirements.txt
   ```
2. Execute a API:
   ```bash
   uvicorn app:app --reload
   ```

Acesse a documentação interativa em: [http://localhost:8000/docs](http://localhost:8000/docs)

## Observações
- Os campos `sector` e `prioridade` ainda não existem no back-end principal, mas são necessários para a predição.
- O modelo e scaler devem estar presentes como `model_rf.pkl` e `scaler.pkl` na pasta do projeto.
- Para exemplos de uso, consulte a documentação automática do FastAPI em `/docs`.
