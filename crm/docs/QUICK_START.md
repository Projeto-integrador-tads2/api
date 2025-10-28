# ⚡ Quick Start - Upload de Fotos com Cloudflare R2

## 🎯 Objetivo
Configurar o upload de fotos de perfil usando o Cloudflare R2 (S3 gratuito) em **5 minutos**.

---

## 📦 O que você precisa

- ✅ Conta no Cloudflare (gratuita)
- ✅ 5 minutos do seu tempo
- ✅ Uma imagem para testar

---

## 🚀 Configuração em 5 Passos

### **PASSO 1: Criar Conta e Bucket (2 min)**

```
1. Acesse: https://dash.cloudflare.com/sign-up
2. Crie sua conta gratuita
3. No menu lateral → R2
4. Create bucket → Nome: "crm-files" → Create
```

✅ **Bucket criado!**

---

### **PASSO 2: Gerar Credenciais (1 min)**

```
1. Na tela do R2 → "Manage R2 API Tokens"
2. Create API Token
3. Nome: "crm-upload-token"
4. Permissions: ✅ Object Read & Write
5. Create API Token
```

**⚠️ COPIE E SALVE (só aparecem uma vez!):**
```
Access Key ID:        abc123xyz789...
Secret Access Key:    xyz789abc123...
Endpoint:             https://1234567890abc.r2.cloudflarestorage.com
```

---

### **PASSO 3: Configurar Acesso Público (30 seg)**

```
1. R2 → Seu bucket "crm-files"
2. Settings → Public Access
3. Allow Access
4. Copie a URL pública: https://pub-xxxxxxxxx.r2.dev
```

---

### **PASSO 4: Atualizar Projeto (1 min)**

Abra `appsettings.Development.json` e preencha:

```json
{
  "CloudflareR2": {
    "AccountId": "1234567890abc",          ← Do endpoint (antes do .r2.cloudflarestorage.com)
    "AccessKeyId": "abc123xyz789...",      ← Access Key ID
    "SecretAccessKey": "xyz789abc123...",  ← Secret Access Key
    "BucketName": "crm-files",             ← Nome do seu bucket
    "Region": "auto",                       ← Deixe assim
    "PublicUrl": "https://pub-xxx.r2.dev"  ← URL pública do bucket
  }
}
```

**Exemplo Real:**
```json
{
  "CloudflareR2": {
    "AccountId": "e1f2a3b4c5d6",
    "AccessKeyId": "a1b2c3d4e5f6g7h8i9j0",
    "SecretAccessKey": "x9y8z7w6v5u4t3s2r1q0p9o8n7m6l5k4",
    "BucketName": "crm-files",
    "Region": "auto",
    "PublicUrl": "https://pub-123abc456def.r2.dev"
  }
}
```

---

### **PASSO 5: Testar! (30 seg)**

```bash
# Inicie o servidor
dotnet run

# Em outro terminal, teste o upload
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -F "file=@foto.jpg"
```

**Ou use o script de teste:**
```bash
./test-upload.sh {userId} foto.jpg
```

**Ou teste no Swagger:**
```
1. Abra: http://localhost:5000/swagger
2. POST /api/User/{userId}/profile-picture
3. Try it out
4. Selecione uma imagem
5. Execute
```

---

## ✅ Resposta de Sucesso

```json
{
  "fileUrl": "https://pub-xxx.r2.dev/profile-pictures/abc123-foto.jpg",
  "fileName": "foto.jpg",
  "fileSize": 245678,
  "contentType": "image/jpeg",
  "uploadedAt": "2025-01-15T10:30:00Z"
}
```

🎉 **Funcionou!** Abra a `fileUrl` no navegador para ver sua imagem!

---

## 🔍 Verificações Rápidas

### ✅ Está funcionando se:
- O upload retorna status 200
- Você recebe uma `fileUrl`
- A imagem abre no navegador
- Vê o arquivo no R2 Dashboard

### ❌ Não está funcionando se:

**Erro: "Access Denied"**
```
→ Verifique as credenciais no appsettings.json
→ Confirme que o token tem permissões corretas
```

**Erro: "The bucket does not exist"**
```
→ Confirme o nome do bucket (case-sensitive!)
→ Verifique se criou o bucket no dashboard
```

**Erro: "Arquivo não fornecido"**
```
→ Certifique-se de enviar o campo "file"
→ Use Content-Type: multipart/form-data
```

**Imagem não aparece (404)**
```
→ Habilite acesso público no bucket
→ Verifique a PublicUrl no appsettings.json
```

---

## 📝 Informações Importantes

### Tipos de arquivo aceitos:
- ✅ JPEG (.jpg, .jpeg)
- ✅ PNG (.png)
- ✅ GIF (.gif)
- ✅ WebP (.webp)

### Limites:
- 📦 **Tamanho máximo**: 5 MB por arquivo
- 💾 **Storage gratuito**: 10 GB/mês
- 🚀 **Operações**: 1M leituras + 10M escritas/mês
- 🌐 **Saída de dados**: ILIMITADA e GRÁTIS

---

## 🔐 Segurança

**⚠️ NUNCA comite credenciais no Git!**

O arquivo `appsettings.Development.json` já está no `.gitignore`.

Para produção, use variáveis de ambiente:

```bash
# Linux/Mac
export CloudflareR2__AccountId="seu-account-id"
export CloudflareR2__AccessKeyId="sua-access-key"
export CloudflareR2__SecretAccessKey="sua-secret-key"
export CloudflareR2__BucketName="crm-files"
export CloudflareR2__PublicUrl="https://pub-xxx.r2.dev"

# Windows PowerShell
$env:CloudflareR2__AccountId="seu-account-id"
$env:CloudflareR2__AccessKeyId="sua-access-key"
$env:CloudflareR2__SecretAccessKey="sua-secret-key"
```

---

## 📚 Próximos Passos

Agora que está funcionando:

1. 📖 Leia o guia completo: [CLOUDFLARE_R2_SETUP.md](./CLOUDFLARE_R2_SETUP.md)
2. 🧪 Veja mais exemplos: [API_TEST_EXAMPLES.md](./API_TEST_EXAMPLES.md)
3. 🎨 Integre no seu frontend
4. 🔒 Configure autenticação JWT
5. 🚀 Faça deploy em produção

---

## 🆘 Precisa de Ajuda?

Consulte a documentação detalhada:
- [Guia Completo de Configuração](./CLOUDFLARE_R2_SETUP.md)
- [Exemplos de Teste](./API_TEST_EXAMPLES.md)
- [Documentação Cloudflare R2](https://developers.cloudflare.com/r2/)

---

## 🎯 Resumo Visual

```
┌─────────────────────────────────────────────┐
│  1. Cloudflare → R2 → Create Bucket         │
│     ↓                                        │
│  2. Manage API Tokens → Create Token        │
│     ↓                                        │
│  3. Settings → Enable Public Access         │
│     ↓                                        │
│  4. Copiar credenciais → appsettings.json   │
│     ↓                                        │
│  5. dotnet run → Testar upload              │
│     ↓                                        │
│  ✅ FUNCIONANDO!                             │
└─────────────────────────────────────────────┘
```

---

**🎉 Pronto! Você tem upload de fotos gratuito e ilimitado!**

*Criado: Janeiro 2025 | CRM API v1.0*