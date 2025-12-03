# Componentes do Projeto CRM API

> Última atualização: 2025-12-03
> Versão do documento: 1.0

## Índice
- [Componentes do Projeto CRM API](#componentes-do-projeto-crm-api)
  - [Índice](#índice)
  - [Lista de componentes](#lista-de-componentes)
  - [Dependências internas](#dependências-internas)
  - [Guideline para componentes](#guideline-para-componentes)
  - [Referências](#referências)
  - [TODOs / Perguntas](#todos--perguntas)

## Lista de componentes
| Nome | Caminho | Responsabilidade | Principais métodos/Endpoints |
|------|--------|-----------------|------------------------------|
| AuthController | Controllers/AuthController.cs | Autenticação e registro de usuários | POST /api/auth/register |
| ClientController | Controllers/ClientController.cs | Gestão de clientes | POST /api/client/register |
| CompanyController | Controllers/CompanyController.cs | Gestão de empresas | POST /api/company/register |
| CompanyCardController | Controllers/CompanyCardController.cs | Gestão de cards de empresa | POST /api/companycard/register |
| ObservationController | Controllers/ObservationController.cs | Observações em cards | POST /api/observation/register |
| StepColumnController | Controllers/StepColumnController.cs | Gestão de colunas de etapas | POST /api/stepcolumn/register |
| UserController | Controllers/UserController.cs | Upload de foto de perfil | POST /api/user/profile-picture |
| CloudflareR2Service | Services/CloudflareR2Service.cs | Armazenamento de arquivos | Upload/Download |
| CurrentUserService | Services/CurrentUserService.cs | Identificação do usuário | GetCurrentUserId |
| AppDbContext | Contexts/AppDbContext.cs | Contexto do banco de dados | DbSets, Migrations |
| DatabaseSeeder | Data/Seed/DatabaseSeeder.cs | Seed inicial do banco | SeedAsync |

## Dependências internas
- Controllers dependem de Commands, Queries, Dtos, Services.
- Services dependem de configurações e interfaces.
- Models são usados por Commands, Queries e Services.
- Middleware aplica filtros nas respostas das APIs.

## Guideline para componentes
- Adicionar novo componente: criar pasta/arquivo em `Controllers/`, `Services/` ou `Models/` conforme tipo.
- Seguir padrão de injeção de dependência via construtor.
- Documentar endpoints e métodos públicos.
- Adicionar testes unitários e de integração.

## Referências
- [Controllers](../Controllers/)
- [Services](../Services/)
- [Models](../Models/)
- [Dtos](../Dtos/)
- [Commands](../Commands/)
- [Queries](../Queries/)

## TODOs / Perguntas
- Confirmar existência de componentes UI/front-end
- Detalhar contratos de interfaces públicas
- Adicionar exemplos de uso para cada serviço
