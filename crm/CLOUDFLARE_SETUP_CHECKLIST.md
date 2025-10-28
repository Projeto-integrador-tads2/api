# ✅ Checklist de Configuração - Cloudflare R2 para Upload de Fotos

## 📋 Use este checklist para garantir que tudo está configurado corretamente

---

## ETAPA 1: Conta e Bucket Cloudflare

### 1.1 Criar Conta Cloudflare
- [ ] Acessei https://dash.cloudflare.com/sign-up
- [ ] Criei minha conta gratuita
- [ ] Confirmei meu email
- [ ] Fiz login no dashboard

### 1.2 Criar Bucket R2
- [ ] Cliquei em **R2** no menu lateral
- [ ] Cliquei em **Create bucket**
- [ ] Defini o nome do bucket: `crm-files` (ou outro)
- [ ] Escolhi região: **Automatic**
- [ ] Cliquei em **Create bucket**
- [ ] Confirmei que o bucket aparece na lista

**✅ Bucket criado com sucesso!**

---

## ETAPA 2: Credenciais de API

### 2.1 Gerar API Token
- [ ] Cliquei em **Manage R2 API Tokens**
- [ ] Cliquei em **Create API Token**
- [ ] Defini o nome: `crm-upload-token`
- [ ] Selecionei permissão: **Object Read & Write**
- [ ] Escolhi: **Apply to specific buckets only**
- [ ] Selecionei meu bucket
- [ ] Cliquei em **Create API Token**

### 2.2 Copiar e Salvar Credenciais
⚠️ **IMPORTANTE**: Essas informações só aparecem UMA VEZ!

- [ ] Copiei o **Access Key ID**
- [ ] Copiei o **Secret Access Key**
- [ ] Copiei o **Endpoint** (ex: `https://abc123.r2.cloudflarestorage.com`)
- [ ] Salvei em um local seguro (senha manager, arquivo local, etc)

**Minhas Credenciais:**
```
Access Key ID:        _______________________________________
Secret Access Key:    _______________________________________
Endpoint:             https://_____________.r2.cloudflarestorage.com
Account ID:           _____________ (extraído do endpoint)
```

**✅ Credenciais salvas com segurança!**

---

## ETAPA 3: Acesso Público

### 3.1 Habilitar Acesso Público ao Bucket
- [ ] Acessei meu bucket no dashboard
- [ ] Cliquei na aba **Settings**
- [ ] Rolei até **Public Access**
- [ ] Cliquei em **Allow Access** ou **Enable public access**
- [ ] Confirmei a ação

### 3.2 Obter URL Pública
- [ ] Copiei a URL pública gerada (ex: `https://pub-xxxxxxxxx.r2.dev`)
- [ ] Salvei essa URL

**Minha URL Pública:**
```
https://pub-________________________.r2.dev
```

**✅ Acesso público configurado!**

---

## ETAPA 4: Configuração do Projeto

### 4.1 Verificar Pacote Instalado
- [ ] Confirmei que o pacote `AWSSDK.S3` está instalado
- [ ] Se não, executei: `dotnet add package AWSSDK.S3`

### 4.2 Atualizar appsettings.Development.json
- [ ] Abri o arquivo `appsettings.Development.json`
- [ ] Localizei a seção `CloudflareR2`
- [ ] Preenchi o **AccountId** (da etapa 2.2)
- [ ] Preenchi o **AccessKeyId** (da etapa 2.2)
- [ ] Preenchi o **SecretAccessKey** (da etapa 2.2)
- [ ] Preenchi o **BucketName** (da etapa 1.2)
- [ ] Preenchi a **PublicUrl** (da etapa 3.2)
- [ ] Salvei o arquivo

**Exemplo de configuração:**
```json
{
  "CloudflareR2": {
    "AccountId": "abc123def456",
    "AccessKeyId": "sua-access-key-id-real",
    "SecretAccessKey": "sua-secret-key-real",
    "BucketName": "crm-files",
    "Region": "auto",
    "PublicUrl": "https://pub-xxxxxxxxx.r2.dev"
  }
}
```

### 4.3 Verificar Segurança
- [ ] Confirmei que `appsettings.Development.json` está no `.gitignore`
- [ ] **NÃO** commitei credenciais no Git
- [ ] Planejei uso de variáveis de ambiente para produção

**✅ Projeto configurado corretamente!**

---

## ETAPA 5: Testes

### 5.1 Preparar para Teste
- [ ] Tenho uma imagem de teste (JPG, PNG, GIF ou WebP)
- [ ] Sei o `userId` de um usuário existente no banco
- [ ] O banco de dados está rodando

### 5.2 Iniciar Servidor
- [ ] Executei `dotnet run` no diretório do projeto
- [ ] Servidor iniciou sem erros
- [ ] Vi a mensagem de "Application started"

### 5.3 Testar via Swagger
- [ ] Acessei http://localhost:5000/swagger
- [ ] Localizei o endpoint `POST /api/User/{userId}/profile-picture`
- [ ] Cliquei em **Try it out**
- [ ] Preenchi o `userId`
- [ ] Selecionei uma imagem no campo `file`
- [ ] Cliquei em **Execute**
- [ ] Recebi status **200 OK**
- [ ] Recebi uma resposta JSON com `fileUrl`

**OU**

### 5.4 Testar via cURL
- [ ] Executei o comando:
```bash
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -F "file=@caminho/para/foto.jpg"
```
- [ ] Recebi status **200 OK**
- [ ] Recebi uma resposta JSON com `fileUrl`

**OU**

### 5.5 Testar via Script
- [ ] Executei: `./test-upload.sh {userId} foto.jpg`
- [ ] Upload concluído com sucesso
- [ ] Recebi a URL da imagem

**✅ Upload funcionando!**

---

## ETAPA 6: Validação Final

### 6.1 Verificar no Cloudflare
- [ ] Acessei o dashboard do R2
- [ ] Abri meu bucket
- [ ] Naveguei até a pasta `profile-pictures/`
- [ ] Vi o arquivo que acabei de fazer upload

### 6.2 Verificar no Banco de Dados
- [ ] Executei query SQL:
```sql
SELECT Id, Name, ProfilePicture FROM User WHERE Id = '{userId}';
```
- [ ] Campo `ProfilePicture` contém a URL do R2
- [ ] URL está completa e correta

### 6.3 Verificar Acesso à Imagem
- [ ] Copiei a `fileUrl` da resposta
- [ ] Colei no navegador
- [ ] Imagem carregou corretamente
- [ ] Não recebi erro 404 ou Access Denied

**✅ Tudo validado e funcionando!**

---

## ETAPA 7: Segurança e Produção

### 7.1 Segurança
- [ ] Removi credenciais de qualquer arquivo commitado
- [ ] Configurei variáveis de ambiente para produção
- [ ] Planejei rotação de credenciais (a cada 3-6 meses)
- [ ] Documentei onde as credenciais estão armazenadas

### 7.2 Monitoramento
- [ ] Configurei alertas no Cloudflare para uso próximo ao limite
- [ ] Implementei logging de uploads no aplicativo
- [ ] Planejei revisão mensal de custos (se aplicável)

### 7.3 Próximos Passos
- [ ] Li a documentação completa em `docs/CLOUDFLARE_R2_SETUP.md`
- [ ] Testei diferentes cenários (arquivo grande, tipo inválido, etc)
- [ ] Planejei integração com frontend
- [ ] Considerei adicionar autenticação JWT ao endpoint
- [ ] Considerei implementar rate limiting

**✅ Pronto para produção!**

---

## 📊 Resumo de Status

| Etapa | Status | Data |
|-------|--------|------|
| 1. Conta e Bucket Cloudflare | ⬜ | __/__/____ |
| 2. Credenciais de API | ⬜ | __/__/____ |
| 3. Acesso Público | ⬜ | __/__/____ |
| 4. Configuração do Projeto | ⬜ | __/__/____ |
| 5. Testes | ⬜ | __/__/____ |
| 6. Validação Final | ⬜ | __/__/____ |
| 7. Segurança e Produção | ⬜ | __/__/____ |

**Legend**: ⬜ Pendente | ✅ Concluído | ❌ Com Problemas

---

## 🆘 Problemas Comuns

### ❌ "Access Denied"
**Solução:**
1. Verifique se as credenciais estão corretas
2. Confirme que o token tem permissões "Object Read & Write"
3. Verifique se o bucket existe

### ❌ "The bucket does not exist"
**Solução:**
1. Confirme o nome do bucket (case-sensitive!)
2. Verifique se está no Account correto

### ❌ Imagem não aparece (404)
**Solução:**
1. Habilite acesso público no bucket
2. Verifique a PublicUrl no appsettings.json
3. Aguarde alguns segundos para propagação

### ❌ "Arquivo muito grande"
**Solução:**
1. Reduza o tamanho da imagem
2. Ou aumente o limite em `Program.cs` e `UploadUserProfilePictureCommand.cs`

---

## 📚 Documentação Adicional

- 📘 [Quick Start](docs/QUICK_START.md) - Configuração em 5 minutos
- 📗 [Guia Completo](docs/CLOUDFLARE_R2_SETUP.md) - Documentação detalhada
- 📙 [Exemplos de Teste](docs/API_TEST_EXAMPLES.md) - Testes em várias linguagens
- 📕 [FAQ](docs/FAQ.md) - Perguntas frequentes

---

## ✨ Pronto!

Se você marcou todos os checkboxes, seu sistema de upload está **100% configurado e funcionando!**

🎉 **Parabéns!** Agora você tem upload de fotos grátis, rápido e confiável!

---

**Data de Configuração**: ____/____/________
**Configurado por**: _______________________
**Versão**: 1.0
**Última atualização**: Janeiro 2025