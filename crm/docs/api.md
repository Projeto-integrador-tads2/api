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
| Método | Rota | Controlador | Autenticação | Descrição |
|--------|------|-------------|--------------|-----------|
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
