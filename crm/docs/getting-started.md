# Guia de Inicialização - CRM API

> Última atualização: 2025-12-03
> Versão do documento: 1.0

## Índice
- [Pré-requisitos](#pré-requisitos)
- [Passo-a-passo de instalação](#passo-a-passo-de-instalação)
- [Como rodar testes e checks](#como-rodar-testes-e-checks)
- [Checklist pós-setup](#checklist-pós-setup)
- [Troubleshooting](#troubleshooting)
- [Referências](#referências)
- [TODOs / Perguntas](#todos--perguntas)

## Pré-requisitos
- SO: Windows/Linux/Mac
- Docker e Docker Compose
- .NET 8 SDK
- MySQL 8

## Passo-a-passo de instalação
1. Clone o repositório:
   ```bash
   git clone <repo-url>
   cd api/crm
   ```
2. Configure variáveis de ambiente em `.env` ou `appsettings.Development.json`.
3. Inicie os serviços:
   ```bash
   docker-compose up -d
   ```
4. Instale dependências .NET:
   ```bash
   dotnet restore
   ```
5. Execute migrações e seed:
   ```bash
   dotnet run
   # ou
   ./start.sh
   ```
6. Acesse Swagger em `http://localhost:5000/swagger` (ou porta configurada).

## Como rodar testes e checks
- Testes unitários/integrados: TODO: Confirmar localização dos testes
- Linter/formatter: TODO: Confirmar comandos

## Checklist pós-setup
- Endpoints acessíveis via Swagger
- Banco de dados populado (seed)
- MinIO acessível em `http://localhost:9002`
- Usuário admin criado

## Troubleshooting
- Verifique logs do container com:
  ```bash
  docker-compose logs
  ```
- Erros de conexão MySQL: checar variáveis de ambiente e porta
- Erros de autenticação JWT: validar chave e issuer
- MinIO não sobe: checar permissões de volume

## Referências
- [docker-compose.yml](../docker-compose.yml)
- [appsettings.Development.json](../appsettings.Development.json)
- [start.sh](../start.sh)
- [setup-minio.sh](../setup-minio.sh)

## TODOs / Perguntas
- Adicionar comandos de teste
- Confirmar porta padrão da API
- Detalhar seed inicial
