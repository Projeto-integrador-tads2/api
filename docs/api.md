# Documentação de API - CRM

> Última atualização: 2025-12-03
> Versão do documento: 1.0

## Índice
- [Tabela de Endpoints](#tabela-de-endpoints)
- [Esquema de autenticação](#esquema-de-autenticação)
- [Exemplos de requests/responses](#exemplos-de-requestsresponses)
- [Referências](#referências)
- [TODOs / Perguntas](#todos--perguntas)


## Tabela de Endpoints
| Método | Rota | Controlador/Serviço | Autenticação | Descrição |
|--------|------|---------------------|--------------|-----------|
| POST   | /api/auth/register         | AuthController         | JWT + Admin | Registrar usuário |
| POST   | /api/client/register       | ClientController       | JWT + Admin | Registrar cliente |
| POST   | /api/company/register      | CompanyController      | JWT         | Registrar empresa |
| POST   | /api/companycard/register  | CompanyCardController  | JWT         | Registrar card de empresa |
| PATCH  | /api/companycard/update/{companyCardId} | CompanyCardController | JWT | Atualizar card de empresa |
| POST   | /api/observation/register  | ObservationController  | JWT         | Registrar observação |
| PATCH  | /api/observation/update/{observationId} | ObservationController | JWT | Atualizar observação |
| POST   | /api/stepcolumn/register   | StepColumnController   | JWT         | Registrar coluna de etapa |
| PATCH  | /api/stepcolumn/update/{id}| StepColumnController   | JWT         | Atualizar coluna de etapa |
| POST   | /api/user/profile-picture  | UserController         | JWT         | Upload foto de perfil |
| POST   | /predict                   | IA (FastAPI)           | Interno     | Predição de probabilidade |
## Integração com API de IA (Machine Learning)

O sistema agora conta com uma API de IA (FastAPI, Python) para predição de probabilidade de sucesso de oportunidades.
A comunicação é feita internamente via Docker Compose, sem expor a IA externamente.

### Endpoint principal

- `POST /predict` (serviço de IA)

#### Exemplo de request
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

#### Exemplo de response
```json
{
  "result": "Muito Provável"
}
```

#### Significado dos campos
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

Para mais detalhes, consulte `ai/README.md`.

## Esquema de autenticação
- **JWT Bearer:**
  - Header: `Authorization: Bearer <token>`
  - Proteção por roles (ex.: Admin)
  - Middleware: [Authorize], [RequireRole]

## Exemplos de requests/responses
### Registrar usuário
```http
POST /api/auth/register
Authorization: Bearer <token>
Content-Type: application/json
{
  "name": "João Silva",
  "email": "joao@empresa.com",
  "password": "senha123",
  "phone": "11999999999"
}
```
**Response:**
```json
{
  "id": "uuid",
  "name": "João Silva",
  "email": "joao@empresa.com"
}
```

### Possíveis erros
- 401 Unauthorized
- 403 Forbidden
- 400 Bad Request (validação)
- 500 Internal Server Error

## Referências
- [AuthController.cs](../Controllers/AuthController.cs)
- [ClientController.cs](../Controllers/ClientController.cs)
- [CompanyController.cs](../Controllers/CompanyController.cs)
- [CompanyCardController.cs](../Controllers/CompanyCardController.cs)
- [ObservationController.cs](../Controllers/ObservationController.cs)
- [StepColumnController.cs](../Controllers/StepColumnController.cs)
- [UserController.cs](../Controllers/UserController.cs)

## TODOs / Perguntas
- Detalhar todos os parâmetros e respostas dos endpoints
- Confirmar existência de rate-limiting
- Adicionar exemplos de erros específicos
