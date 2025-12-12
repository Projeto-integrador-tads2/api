# Configuração do Projeto CRM API

> Última atualização: 2025-12-03
> Versão do documento: 1.0

## Índice
- [Arquivos de configuração](#arquivos-de-configuração)
- [Variáveis de ambiente](#variáveis-de-ambiente)
- [Scripts de build e run](#scripts-de-build-e-run)
- [Boas práticas de segredos](#boas-práticas-de-segredos)
- [Exemplo de .env.example](#exemplo-de-envexample)
- [Referências](#referências)
- [TODOs / Perguntas](#todos--perguntas)

## Arquivos de configuração
- `appsettings.json`
- `appsettings.Development.json`
- `docker-compose.yml`
- `Dockerfile`

## Variáveis de ambiente
| Nome | Propósito | Formato/Exemplo | Segurança |
|------|-----------|-----------------|-----------|
| ConnectionStrings:DefaultConnection | Conexão com MySQL | Server=localhost;Database=crm;User=crmuser;Password=crmpass;Port=3306; | NÃO SECRETO |
| Jwt:Key | Chave de assinatura JWT | <jwt-key-placeholder> | SECRETO |
| Jwt:Issuer | Emissor do JWT | CrmApi | NÃO SECRETO |
| Jwt:Audience | Público do JWT | CrmApiUsers | NÃO SECRETO |
| Smtp:Host | SMTP para envio de email | smtp.gmail.com | NÃO SECRETO |
| Smtp:User | Usuário SMTP | seu-email@gmail.com | SECRETO |
| Smtp:Pass | Senha SMTP | <smtp-password-placeholder> | SECRETO |
| CloudflareR2:AccessKeyId | Chave de acesso MinIO/R2 | <access-key-placeholder> | SECRETO |
| CloudflareR2:SecretAccessKey | Chave secreta MinIO/R2 | <secret-key-placeholder> | SECRETO |
| CloudflareR2:BucketName | Nome do bucket | crm-files | NÃO SECRETO |
| CloudflareR2:Region | Região do bucket | auto/us-east-1 | NÃO SECRETO |
| CloudflareR2:PublicUrl | URL pública dos arquivos | https://pub-xxxxxxxxx.r2.dev | NÃO SECRETO |

## Scripts de build e run
- `dotnet build`
- `dotnet run`
- `docker-compose up -d`
- `start.sh` (Linux)
- `setup-minio.sh` (Linux)

## Boas práticas de segredos
- Nunca versionar segredos reais
- Usar placeholders em exemplos
- Preferir variáveis de ambiente e vaults
- TODO: Confirmar uso de secrets manager

## Exemplo de .env.example
```env
# MySQL
MYSQL_ROOT_PASSWORD=root
MYSQL_DATABASE=crm
MYSQL_USER=crmuser
MYSQL_PASSWORD=crmpass

# JWT
JWT_KEY=<jwt-key-placeholder>
JWT_ISSUER=CrmApi
JWT_AUDIENCE=CrmApiUsers

# SMTP
SMTP_HOST=smtp.gmail.com
SMTP_USER=<smtp-user-placeholder>
SMTP_PASS=<smtp-password-placeholder>

# MinIO/CloudflareR2
R2_ACCESS_KEY_ID=<access-key-placeholder>
R2_SECRET_ACCESS_KEY=<secret-key-placeholder>
R2_BUCKET_NAME=crm-files
R2_REGION=auto
R2_PUBLIC_URL=https://pub-xxxxxxxxx.r2.dev
```

## Referências
- [appsettings.json](../appsettings.json)
- [docker-compose.yml](../docker-compose.yml)
- [Dockerfile](../Dockerfile)

## TODOs / Perguntas
- Confirmar uso de secrets manager
- Adicionar .env.example real ao repositório
