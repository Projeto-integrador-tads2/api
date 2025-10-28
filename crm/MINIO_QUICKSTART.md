# ⚡ MinIO - Storage S3 Local (SEM CARTÃO!)

## 🎯 O que é o MinIO?

MinIO é um servidor de storage **open-source** compatível com S3 que roda **localmente no Docker**.

✅ **100% gratuito** - sem cartão de crédito  
✅ **Roda local** - perfeito para desenvolvimento  
✅ **Compatível S3** - mesmo código funciona no Cloudflare R2/AWS S3  
✅ **Interface web** - fácil de usar  

---

## 🚀 Setup em 3 Passos (2 minutos)

### **PASSO 1: Iniciar Docker**

```bash
# Linux
sudo systemctl start docker

# Ou abra o Docker Desktop (Windows/Mac)
```

### **PASSO 2: Rodar o Script de Setup**

```bash
cd tads2/crm
./setup-minio.sh
```

O script vai:
- ✅ Iniciar o MinIO no Docker
- ✅ Criar o bucket `crm-files`
- ✅ Configurar acesso público
- ✅ Mostrar as informações de acesso

### **PASSO 3: Acessar o Console**

Abra no navegador: **http://localhost:9003**

```
Usuário: minioadmin
Senha:   minioadmin123
```

**✅ Pronto! MinIO configurado!**

---

## 🧪 Testar Upload

### 1. Iniciar a API

```bash
dotnet run
```

### 2. Testar no Swagger

```
http://localhost:5000/swagger

POST /api/User/{userId}/profile-picture
- userId: qualquer GUID válido do banco
- file: selecione uma imagem
- Execute
```

### 3. Ver o Resultado

A resposta terá algo como:

```json
{
  "fileUrl": "http://localhost:9002/crm-files/profile-pictures/abc123-foto.jpg",
  "fileName": "foto.jpg",
  "fileSize": 245678,
  "contentType": "image/jpeg",
  "uploadedAt": "2025-01-15T10:30:00Z"
}
```

**Copie a URL e abra no navegador** - sua imagem estará lá! 🎉

---

## 📊 Informações de Acesso

| Item | Valor |
|------|-------|
| **Console Web** | http://localhost:9003 |
| **API Endpoint** | http://localhost:9002 |
| **Usuário** | minioadmin |
| **Senha** | minioadmin123 |
| **Bucket** | crm-files |

---

## 🔧 Comandos Úteis

### Iniciar MinIO

```bash
docker-compose -f docker-compose.minio.yml up -d
```

### Parar MinIO

```bash
docker-compose -f docker-compose.minio.yml down
```

### Ver Logs

```bash
docker-compose -f docker-compose.minio.yml logs -f
```

### Reiniciar MinIO

```bash
docker-compose -f docker-compose.minio.yml restart
```

### Ver Status

```bash
docker ps | grep minio
```

---

## 🪣 Gerenciar Buckets (MinIO Client)

### Instalar MinIO Client (mc)

```bash
# Linux
wget https://dl.min.io/client/mc/release/linux-amd64/mc
chmod +x mc
sudo mv mc /usr/local/bin/

# Mac
brew install minio/stable/mc
```

### Configurar Alias

```bash
mc alias set local http://localhost:9002 minioadmin minioadmin123
```

### Comandos Úteis

```bash
# Listar buckets
mc ls local

# Criar bucket
mc mb local/novo-bucket

# Listar arquivos no bucket
mc ls local/crm-files

# Ver arquivos recursivamente
mc ls --recursive local/crm-files

# Copiar arquivo
mc cp foto.jpg local/crm-files/

# Remover arquivo
mc rm local/crm-files/profile-pictures/arquivo.jpg

# Configurar acesso público
mc anonymous set download local/crm-files
```

---

## 🌐 Acessar Arquivos

### URL Padrão

```
http://localhost:9002/{bucket}/{pasta}/{arquivo}
```

### Exemplo

Se você fez upload para `profile-pictures/user123.jpg`:

```
http://localhost:9002/crm-files/profile-pictures/user123.jpg
```

---

## 🎨 Interface Web (Console)

### Acessar

```
http://localhost:9003
```

### Funcionalidades

- ✅ Ver todos os buckets
- ✅ Navegar pelos arquivos
- ✅ Fazer upload manual
- ✅ Baixar arquivos
- ✅ Deletar arquivos
- ✅ Ver estatísticas
- ✅ Gerenciar permissões

---

## 🔄 Migrar para Cloudflare R2 depois

Quando quiser usar o Cloudflare R2 (produção), basta alterar `appsettings.json`:

### Desenvolvimento (MinIO)

```json
{
  "CloudflareR2": {
    "ServiceURL": "http://localhost:9002",
    "AccessKeyId": "minioadmin",
    "SecretAccessKey": "minioadmin123",
    "BucketName": "crm-files",
    "PublicUrl": "http://localhost:9002/crm-files"
  }
}
```

### Produção (Cloudflare R2)

```json
{
  "CloudflareR2": {
    "AccountId": "seu-account-id",
    "AccessKeyId": "sua-access-key",
    "SecretAccessKey": "sua-secret-key",
    "BucketName": "crm-files",
    "PublicUrl": "https://pub-xxxxxxxxx.r2.dev"
  }
}
```

**O código não muda!** Só a configuração. 🎉

---

## 🐛 Problemas Comuns

### ❌ Porta 9002 ou 9003 já em uso

Altere as portas no `docker-compose.minio.yml`:

```yaml
ports:
  - "9004:9000" # API
  - "9005:9001" # Console
```

E atualize no `appsettings.Development.json`

### ❌ Docker não está rodando

```bash
sudo systemctl start docker
# ou abra o Docker Desktop
```

### ❌ Erro "bucket does not exist"

Crie o bucket manualmente:

```bash
mc alias set local http://localhost:9002 minioadmin minioadmin123
mc mb local/crm-files
mc anonymous set download local/crm-files
```

### ❌ Imagem não aparece (403)

Configure acesso público:

```bash
mc anonymous set download local/crm-files
```

---

## 📦 Incluir no Docker Compose Completo

Para subir MinIO junto com MySQL e API:

```bash
# Usar o docker-compose principal
docker-compose up -d

# MinIO já está incluído!
```

Acesse:
- API: http://localhost:5000
- MinIO Console: http://localhost:9003
- MySQL: localhost:3306

---

## 💾 Persistência de Dados

Os arquivos do MinIO ficam salvos em um volume Docker:

```bash
# Ver volumes
docker volume ls

# Ver informações do volume
docker volume inspect crm_minio_data

# Backup do volume
docker run --rm -v crm_minio_data:/data -v $(pwd):/backup alpine tar czf /backup/minio-backup.tar.gz /data

# Restaurar backup
docker run --rm -v crm_minio_data:/data -v $(pwd):/backup alpine tar xzf /backup/minio-backup.tar.gz -C /
```

---

## 🚀 Vantagens do MinIO para Desenvolvimento

| Recurso | MinIO | Cloudflare R2 |
|---------|-------|---------------|
| Custo | **Grátis** | Grátis até 10GB |
| Setup | **2 minutos** | 10 minutos |
| Cartão | **Não precisa** | Precisa |
| Velocidade local | **Instantâneo** | Depende da internet |
| Testes offline | **Sim** | Não |
| Compatibilidade | **S3 API** | S3 API |

---

## 📚 Recursos

- [Documentação MinIO](https://min.io/docs/)
- [MinIO Client (mc)](https://min.io/docs/minio/linux/reference/minio-mc.html)
- [Docker Hub - MinIO](https://hub.docker.com/r/minio/minio)

---

## ✅ Checklist

- [ ] Docker instalado e rodando
- [ ] Executei `./setup-minio.sh`
- [ ] Acessei o console: http://localhost:9003
- [ ] Vi o bucket `crm-files` criado
- [ ] Testei upload via Swagger
- [ ] Consegui ver a imagem no navegador

---

🎉 **Pronto! Você tem storage S3 local funcionando sem cartão de crédito!**

*MinIO é perfeito para desenvolvimento. Quando for para produção, migre para Cloudflare R2 ou AWS S3 simplesmente alterando as configurações.*

---

**Criado: Janeiro 2025 | CRM API v1.0**