# 📚 Documentação - Sistema de Upload de Fotos

## 📋 Índice

- [CLOUDFLARE_R2_SETUP.md](./CLOUDFLARE_R2_SETUP.md) - Guia completo de configuração do Cloudflare R2
- [API_TEST_EXAMPLES.md](./API_TEST_EXAMPLES.md) - Exemplos de teste da API

---

## 🚀 Início Rápido

### 1. Instalar Dependências

O pacote necessário já está instalado:
- ✅ `AWSSDK.S3` (versão 4.0.9)

### 2. Configurar Cloudflare R2

Siga o guia completo em: [CLOUDFLARE_R2_SETUP.md](./CLOUDFLARE_R2_SETUP.md)

**Resumo:**
1. Crie uma conta no [Cloudflare](https://dash.cloudflare.com/sign-up)
2. Crie um bucket R2 (ex: `crm-files`)
3. Gere credenciais de API (Access Key ID e Secret Access Key)
4. Configure acesso público ao bucket
5. Atualize o `appsettings.json`

### 3. Atualizar Configurações

Edite `appsettings.json`:

```json
"CloudflareR2": {
  "AccountId": "seu-account-id-aqui",
  "AccessKeyId": "sua-access-key-aqui",
  "SecretAccessKey": "sua-secret-key-aqui",
  "BucketName": "crm-files",
  "Region": "auto",
  "PublicUrl": "https://pub-xxxxxxxxx.r2.dev"
}
```

### 4. Testar

```bash
# Iniciar o servidor
dotnet run

# Testar upload via cURL
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -F "file=@foto.jpg"
```

Veja mais exemplos em: [API_TEST_EXAMPLES.md](./API_TEST_EXAMPLES.md)

---

## 📁 Estrutura do Código

### Controllers
- `UserController.cs` - Endpoint de upload

### Commands
- `UploadUserProfilePictureCommand.cs` - Lógica de negócio do upload

### Services
- `CloudflareR2Service.cs` - Implementação do serviço de storage no R2

### Interfaces
- `IFileStorageService.cs` - Contrato do serviço de storage

---

## 🔧 Funcionalidades

✅ Upload de fotos de perfil para Cloudflare R2  
✅ Validação de tipo de arquivo (JPEG, PNG, GIF, WebP)  
✅ Validação de tamanho (máximo 5MB)  
✅ Substituição automática de foto antiga  
✅ Nome de arquivo customizável  
✅ URLs públicas para acesso às imagens  

---

## 📊 Endpoint da API

### `POST /api/User/{userId}/profile-picture`

**Descrição**: Faz upload da foto de perfil do usuário

**Parâmetros:**
- `userId` (path) - GUID do usuário
- `file` (form-data) - Arquivo de imagem
- `customFileName` (form-data, opcional) - Nome customizado do arquivo

**Resposta (200 OK):**
```json
{
  "fileUrl": "https://pub-xxx.r2.dev/profile-pictures/abc123-foto.jpg",
  "fileName": "foto.jpg",
  "fileSize": 245678,
  "contentType": "image/jpeg",
  "uploadedAt": "2025-01-15T10:30:00Z"
}
```

**Erros:**
- `400 Bad Request` - Arquivo inválido ou muito grande
- `404 Not Found` - Usuário não encontrado
- `500 Internal Server Error` - Erro no upload

---

## 🔒 Segurança

⚠️ **IMPORTANTE**: Nunca comite credenciais no Git!

### Desenvolvimento
Use `appsettings.Development.json` (já está no `.gitignore`)

### Produção
Use variáveis de ambiente:
```bash
export CloudflareR2__AccountId="seu-account-id"
export CloudflareR2__AccessKeyId="sua-access-key"
export CloudflareR2__SecretAccessKey="sua-secret-key"
```

---

## 📈 Limites do Plano Gratuito R2

- ✅ **10 GB** de armazenamento
- ✅ **1 milhão** de operações de leitura/mês
- ✅ **10 milhões** de operações de escrita/mês
- ✅ **ZERO taxas** de saída de dados (egress gratuito!)

---

## 📚 Documentação Completa

- [Guia de Configuração Completo](./CLOUDFLARE_R2_SETUP.md)
- [Exemplos de Teste da API](./API_TEST_EXAMPLES.md)
- [Documentação Oficial Cloudflare R2](https://developers.cloudflare.com/r2/)

---

## ✅ Checklist de Configuração

- [ ] Criar conta no Cloudflare
- [ ] Criar bucket R2
- [ ] Gerar credenciais de API
- [ ] Configurar acesso público
- [ ] Atualizar `appsettings.json`
- [ ] Testar upload via Swagger/cURL
- [ ] Verificar imagem acessível pela URL pública

---

🎉 **Tudo pronto! Consulte os guias detalhados para mais informações.**