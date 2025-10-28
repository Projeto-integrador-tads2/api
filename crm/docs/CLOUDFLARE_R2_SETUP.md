# 📦 Guia Completo - Configuração Cloudflare R2 (S3 Gratuito)

## 🎯 O que é o Cloudflare R2?

O Cloudflare R2 é um serviço de armazenamento de objetos compatível com S3, **GRATUITO** até 10GB de armazenamento por mês, sem taxas de saída de dados.

---

## 📋 Passo a Passo de Configuração

### **ETAPA 1: Criar Conta no Cloudflare**

1. Acesse: https://dash.cloudflare.com/sign-up
2. Crie uma conta gratuita com seu email
3. Confirme seu email

---

### **ETAPA 2: Criar Bucket no R2**

1. Faça login no [Cloudflare Dashboard](https://dash.cloudflare.com/)
2. No menu lateral esquerdo, clique em **R2**
3. Clique no botão **Create bucket**
4. Configure o bucket:
   - **Bucket name**: `crm-files` (ou outro nome de sua preferência)
   - **Location**: `Automatic` (recomendado para melhor performance)
5. Clique em **Create bucket**

✅ **Bucket criado com sucesso!**

---

### **ETAPA 3: Gerar Credenciais de API (Access Keys)**

1. No dashboard do R2, clique em **Manage R2 API Tokens** (canto superior direito)
2. Clique em **Create API Token**
3. Configure o token:
   - **Token name**: `crm-upload-token`
   - **Permissions**: 
     - ✅ **Object Read & Write** (marque esta opção)
   - **Specify bucket(s)**: Selecione `Apply to specific buckets only` e escolha seu bucket `crm-files`
   - **TTL (Time to live)**: Deixe em branco (sem expiração) ou defina um prazo
4. Clique em **Create API Token**

5. **⚠️ IMPORTANTE**: Copie e guarde as seguintes informações (elas só aparecem UMA VEZ!):

```
Access Key ID:         abc123def456ghi789
Secret Access Key:     xyz789uvw456rst123abc
Jurisdiction-specific endpoint for S3 clients: https://1234567890abcdef.r2.cloudflarestorage.com
```

📌 **Salve estas credenciais em um local seguro!**

---

### **ETAPA 4: Configurar Acesso Público ao Bucket**

Para que as imagens fiquem acessíveis publicamente:

#### **Opção A: Usar Domínio Público do R2 (Mais Fácil)**

1. Vá em **R2** > Selecione seu bucket `crm-files`
2. Clique na aba **Settings**
3. Role até **Public access**
4. Clique em **Allow Access** ou **Enable public access**
5. Copie a **URL pública** gerada: `https://pub-xxxxxxxxxxxxxxxxxx.r2.dev`

#### **Opção B: Usar Domínio Personalizado (Avançado)**

1. Vá em **R2** > Selecione seu bucket
2. Clique na aba **Settings**
3. Em **Custom Domains**, clique em **Connect Domain**
4. Digite seu domínio (ex: `cdn.seusite.com`)
5. Siga as instruções para configurar o DNS

---

### **ETAPA 5: Atualizar Configurações no Projeto**

1. Abra o arquivo `appsettings.json` do projeto
2. Localize a seção `CloudflareR2`
3. Atualize com suas credenciais:

```json
"CloudflareR2": {
  "AccountId": "1234567890abcdef",
  "AccessKeyId": "abc123def456ghi789",
  "SecretAccessKey": "xyz789uvw456rst123abc",
  "BucketName": "crm-files",
  "Region": "auto",
  "PublicUrl": "https://pub-xxxxxxxxxxxxxxxxxx.r2.dev"
}
```

**Como preencher cada campo:**

- **AccountId**: É o código que aparece no endpoint. Ex: se o endpoint é `https://1234567890abcdef.r2.cloudflarestorage.com`, o AccountId é `1234567890abcdef`
- **AccessKeyId**: O Access Key ID que você copiou na Etapa 3
- **SecretAccessKey**: O Secret Access Key que você copiou na Etapa 3
- **BucketName**: O nome do bucket que você criou (ex: `crm-files`)
- **Region**: Deixe como `auto`
- **PublicUrl**: A URL pública do seu bucket (ex: `https://pub-xxxxxxxxxxxxxxxxxx.r2.dev`)

---

### **ETAPA 6: Proteger Credenciais (Segurança)**

⚠️ **NUNCA COMITE AS CREDENCIAIS NO GIT!**

#### **Para Desenvolvimento Local:**
Use `appsettings.Development.json`:

```json
{
  "CloudflareR2": {
    "AccountId": "seu-account-id-real",
    "AccessKeyId": "sua-access-key-real",
    "SecretAccessKey": "sua-secret-key-real",
    "BucketName": "crm-files",
    "Region": "auto",
    "PublicUrl": "https://pub-xxxxxxxxxxxxxxxxxx.r2.dev"
  }
}
```

#### **Para Produção:**
Use variáveis de ambiente:

```bash
export CloudflareR2__AccountId="seu-account-id"
export CloudflareR2__AccessKeyId="sua-access-key"
export CloudflareR2__SecretAccessKey="sua-secret-key"
export CloudflareR2__BucketName="crm-files"
export CloudflareR2__Region="auto"
export CloudflareR2__PublicUrl="https://pub-xxxxxxxxxxxxxxxxxx.r2.dev"
```

Ou no Docker Compose:

```yaml
environment:
  - CloudflareR2__AccountId=seu-account-id
  - CloudflareR2__AccessKeyId=sua-access-key
  - CloudflareR2__SecretAccessKey=sua-secret-key
  - CloudflareR2__BucketName=crm-files
  - CloudflareR2__Region=auto
  - CloudflareR2__PublicUrl=https://pub-xxxxxxxxxxxxxxxxxx.r2.dev
```

---

## 🧪 Testando a Configuração

### **1. Iniciar o Projeto**

```bash
cd tads2/crm
dotnet run
```

### **2. Testar Upload via Swagger**

1. Acesse: http://localhost:5000/swagger
2. Localize o endpoint `POST /api/User/{userId}/profile-picture`
3. Clique em **Try it out**
4. Preencha:
   - **userId**: O ID de um usuário existente no banco
   - **file**: Selecione uma imagem (PNG, JPG, GIF ou WebP)
   - **customFileName**: (opcional) Nome personalizado
5. Clique em **Execute**

### **3. Testar via cURL**

```bash
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/caminho/para/sua/imagem.jpg"
```

### **4. Resposta Esperada**

```json
{
  "fileUrl": "https://pub-xxxxxxxxxxxxxxxxxx.r2.dev/profile-pictures/abc123-imagem.jpg",
  "fileName": "imagem.jpg",
  "fileSize": 245678,
  "contentType": "image/jpeg",
  "uploadedAt": "2025-01-15T10:30:00Z"
}
```

---

## 📊 Estrutura de Arquivos no R2

Os arquivos são organizados da seguinte forma:

```
crm-files/
└── profile-pictures/
    ├── abc123-usuario1.jpg
    ├── def456-usuario2.png
    └── ghi789-usuario3.webp
```

---

## 🔧 Troubleshooting (Resolução de Problemas)

### ❌ **Erro: "Access Denied"**
**Solução**: Verifique se:
- As credenciais estão corretas no `appsettings.json`
- O token tem permissões de `Object Read & Write`
- O bucket existe e o nome está correto

### ❌ **Erro: "The bucket does not exist"**
**Solução**: 
- Confirme que o nome do bucket no `appsettings.json` está exatamente igual ao criado no Cloudflare
- Verifique se não há espaços ou caracteres especiais

### ❌ **Erro: "Invalid endpoint"**
**Solução**:
- Verifique se o `AccountId` está correto
- O formato deve ser: `https://ACCOUNT_ID.r2.cloudflarestorage.com`

### ❌ **Imagem não aparece (404)**
**Solução**:
- Verifique se o acesso público está habilitado no bucket
- Confirme se a `PublicUrl` no `appsettings.json` está correta
- Teste acessar a URL diretamente no navegador

---

## 📈 Limites do Plano Gratuito

✅ **10 GB** de armazenamento
✅ **1 milhão** de operações de leitura (Class A) por mês
✅ **10 milhões** de operações de escrita (Class B) por mês
✅ **ZERO taxas** de saída de dados (egress)

---

## 🔒 Boas Práticas de Segurança

1. ✅ **Nunca** comite credenciais no Git
2. ✅ Use `appsettings.Development.json` para desenvolvimento (já está no `.gitignore`)
3. ✅ Use variáveis de ambiente em produção
4. ✅ Crie tokens com permissões mínimas necessárias
5. ✅ Defina TTL (expiração) nos tokens quando possível
6. ✅ Rotacione as credenciais periodicamente

---

## 📚 Recursos Úteis

- [Documentação Oficial do Cloudflare R2](https://developers.cloudflare.com/r2/)
- [Compatibilidade com S3 API](https://developers.cloudflare.com/r2/api/s3/)
- [AWS SDK para .NET](https://docs.aws.amazon.com/sdk-for-net/)

---

## ✅ Checklist Final

Antes de ir para produção, verifique:

- [ ] Bucket criado no Cloudflare R2
- [ ] Credenciais de API geradas e testadas
- [ ] Acesso público configurado no bucket
- [ ] `appsettings.json` atualizado com as credenciais corretas
- [ ] Upload testado com sucesso via Swagger/Postman
- [ ] Imagens acessíveis pela URL pública
- [ ] Credenciais protegidas (não commitadas no Git)
- [ ] Variáveis de ambiente configuradas para produção

---

🎉 **Pronto! Seu sistema de upload está configurado e funcionando!**