# ❓ FAQ - Perguntas Frequentes sobre Upload de Fotos

## 📋 Índice
- [Configuração Inicial](#configuração-inicial)
- [Upload de Arquivos](#upload-de-arquivos)
- [Cloudflare R2](#cloudflare-r2)
- [Segurança](#segurança)
- [Troubleshooting](#troubleshooting)
- [Performance](#performance)
- [Custos](#custos)

---

## 🔧 Configuração Inicial

### P: Preciso pagar para usar o Cloudflare R2?
**R:** Não! O plano gratuito oferece:
- 10 GB de armazenamento/mês
- 1 milhão de operações de leitura/mês
- 10 milhões de operações de escrita/mês
- **Saída de dados ilimitada e gratuita** (principal diferença do AWS S3)

### P: Onde encontro meu Account ID?
**R:** Existem 2 formas:
1. No endpoint da API: `https://ACCOUNT_ID.r2.cloudflarestorage.com`
   - O Account ID é a parte antes de `.r2.cloudflarestorage.com`
2. No dashboard do Cloudflare: Sidebar → R2 → na URL você verá `/accounts/ACCOUNT_ID/r2`

### P: Posso usar outro serviço de storage além do R2?
**R:** Sim! A interface `IFileStorageService` permite trocar a implementação. Você pode criar implementações para:
- AWS S3
- Google Cloud Storage
- Azure Blob Storage
- MinIO
- Sistema de arquivos local

Basta implementar a interface e registrar no DI container.

### P: Como proteger minhas credenciais?
**R:** Siga estas práticas:
1. **Desenvolvimento**: Use `appsettings.Development.json` (já está no `.gitignore`)
2. **Produção**: Use variáveis de ambiente ou Azure Key Vault / AWS Secrets Manager
3. **Nunca** comite credenciais no Git
4. Use `.gitignore` para arquivos de configuração sensíveis

---

## 📤 Upload de Arquivos

### P: Quais tipos de arquivo são aceitos?
**R:** Atualmente, apenas imagens:
- JPEG (`.jpg`, `.jpeg`)
- PNG (`.png`)
- GIF (`.gif`)
- WebP (`.webp`)

Para aceitar outros tipos, modifique o método `ValidateFile()` em `UploadUserProfilePictureCommand.cs`.

### P: Qual o tamanho máximo de arquivo?
**R:** 5 MB por arquivo. Para alterar:

1. Modifique a validação em `UploadUserProfilePictureCommand.cs`:
```csharp
if (file.Length > 10 * 1024 * 1024) // 10MB
    throw new ArgumentException("Arquivo muito grande (máximo 10MB)");
```

2. Aumente o limite no `Program.cs`:
```csharp
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
});
```

### P: O que acontece com a foto antiga quando faço upload de uma nova?
**R:** A foto antiga é **automaticamente deletada** do R2 para economizar espaço. Veja o código em `UploadUserProfilePictureCommandHandler`:
```csharp
if (!string.IsNullOrEmpty(user.ProfilePicture))
{
    var oldFileKey = ExtractFileKeyFromUrl(user.ProfilePicture);
    await _fileStorageService.DeleteFileAsync(oldFileKey);
}
```

### P: Posso fazer upload de múltiplas fotos ao mesmo tempo?
**R:** O endpoint atual aceita apenas um arquivo por vez. Para múltiplos arquivos:
1. Faça requisições separadas para cada arquivo
2. Ou crie um novo endpoint que aceite `List<IFormFile>`

### P: Como customizar o nome do arquivo?
**R:** Use o parâmetro `customFileName`:

```bash
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -F "file=@foto.jpg" \
  -F "customFileName=joao-silva.jpg"
```

Se não informado, o sistema gera automaticamente: `{GUID}-{nomeOriginal}`

### P: Os uploads são síncronos ou assíncronos?
**R:** O upload é **assíncrono** usando `async/await`, mas a requisição HTTP aguarda a conclusão. Para uploads muito grandes, considere implementar um sistema de fila (RabbitMQ, Azure Service Bus, etc).

---

## ☁️ Cloudflare R2

### P: Qual a diferença entre R2 e S3?
**R:** 
| Recurso | Cloudflare R2 | AWS S3 |
|---------|---------------|--------|
| API | Compatível S3 | S3 nativo |
| Saída de dados | **GRÁTIS ilimitado** | Paga (caro!) |
| Armazenamento gratuito | 10 GB/mês | 5 GB (12 meses) |
| CDN integrado | Sim (Cloudflare) | Não (CloudFront separado) |
| Preço storage (após free tier) | $0.015/GB | $0.023/GB |

### P: Como funciona a URL pública?
**R:** Quando você habilita acesso público, o Cloudflare gera uma URL como:
```
https://pub-{random}.r2.dev
```

Todas as imagens ficam acessíveis em:
```
https://pub-{random}.r2.dev/pasta/arquivo.jpg
```

### P: Posso usar meu próprio domínio?
**R:** Sim! No dashboard do R2:
1. Vá no seu bucket → Settings
2. Custom Domains → Connect Domain
3. Digite seu domínio (ex: `cdn.seusite.com`)
4. Configure o DNS conforme instruções
5. Atualize `PublicUrl` no `appsettings.json`

### P: As imagens são servidas via CDN?
**R:** **Sim!** O Cloudflare automaticamente distribui os arquivos pela rede global de CDN deles (200+ localizações). Isso garante baixa latência em qualquer lugar do mundo.

### P: Posso configurar cache das imagens?
**R:** Sim! No R2, você pode configurar:
- Cache-Control headers
- TTL personalizado
- Regras de cache no Cloudflare

### P: O R2 tem limite de requisições por segundo?
**R:** O Cloudflare não publica limites específicos para o plano gratuito, mas é generoso. Para aplicações de produção, monitore o uso no dashboard.

---

## 🔒 Segurança

### P: As imagens são públicas ou privadas?
**R:** Atualmente, **públicas** (qualquer pessoa com a URL pode acessar). Para tornar privadas:
1. Desabilite acesso público no bucket
2. Gere URLs assinadas temporárias (presigned URLs)
3. Implemente autenticação no endpoint

### P: Como gerar URLs temporárias (presigned URLs)?
**R:** Adicione este método em `CloudflareR2Service.cs`:

```csharp
public string GetPresignedUrl(string fileKey, int expirationMinutes = 60)
{
    var request = new GetPreSignedUrlRequest
    {
        BucketName = _bucketName,
        Key = fileKey,
        Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
    };
    
    return _s3Client.GetPreSignedURL(request);
}
```

### P: Como adicionar autenticação JWT no endpoint?
**R:** Adicione o atributo `[Authorize]` no controller:

```csharp
[Authorize]
[HttpPost("{userId}/profile-picture")]
public async Task<IActionResult> UploadProfilePicture(...)
{
    // Validar se o usuário pode fazer upload para este userId
    var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (currentUserId != userId.ToString())
        return Forbid();
    
    // ... resto do código
}
```

### P: Como prevenir uploads maliciosos?
**R:** O sistema já valida:
- ✅ Tipo MIME (apenas imagens)
- ✅ Tamanho máximo (5MB)
- ✅ Arquivo não vazio

Para segurança adicional:
1. Valide o conteúdo real (magic bytes)
2. Escaneie por malware (ClamAV, VirusTotal API)
3. Re-encode a imagem (remove metadata EXIF)
4. Rate limiting por usuário

### P: Como implementar rate limiting?
**R:** Use `AspNetCoreRateLimit`:

```bash
dotnet add package AspNetCoreRateLimit
```

```csharp
services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "POST:/api/User/*/profile-picture",
            Limit = 10,
            Period = "1h"
        }
    };
});
```

---

## 🐛 Troubleshooting

### P: Erro "Access Denied" ao fazer upload
**R:** Verifique:
1. ✅ Credenciais corretas no `appsettings.json`
2. ✅ Token tem permissões "Object Read & Write"
3. ✅ Bucket existe e nome está correto
4. ✅ Token não expirou

### P: Erro "The bucket does not exist"
**R:** 
- Confirme que o nome do bucket está **exatamente** igual (case-sensitive)
- Verifique se criou o bucket no Account correto

### P: Upload funciona mas imagem não aparece (404)
**R:** 
1. ✅ Habilite acesso público no bucket
2. ✅ Verifique se `PublicUrl` está correto no `appsettings.json`
3. ✅ Aguarde alguns segundos (propagação de DNS)
4. ✅ Teste a URL diretamente no navegador

### P: Erro "Request Entity Too Large"
**R:** O arquivo excede o limite configurado. Aumente em `Program.cs`:

```csharp
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 20 * 1024 * 1024; // 20MB
});
```

E no servidor web (Nginx/IIS):
```nginx
# Nginx
client_max_body_size 20M;
```

### P: Upload lento, como otimizar?
**R:** 
1. Use regiões próximas ao usuário (R2 tem auto-região)
2. Implemente upload direto do cliente para R2 (presigned POST)
3. Adicione progresso de upload no frontend
4. Considere compressão de imagem no cliente

---

## ⚡ Performance

### P: Quanto tempo leva um upload?
**R:** Depende de:
- Tamanho do arquivo
- Velocidade da internet
- Localização geográfica

Exemplo: 2MB geralmente leva 1-3 segundos.

### P: Como melhorar a performance?
**R:** 
1. **Client-side**: Comprima imagens antes do upload
2. **Server-side**: Use processamento assíncrono com filas
3. **CDN**: O R2 já usa Cloudflare CDN automaticamente
4. **Caching**: Configure headers de cache

### P: Posso fazer upload direto do browser para o R2?
**R:** Sim! Use **presigned POST URLs**:

```csharp
public async Task<PresignedPostUrl> GetUploadUrlAsync(string fileName)
{
    // Gera URL temporária para upload direto
    // Cliente faz POST diretamente para R2
    // Economiza banda do servidor
}
```

### P: Como processar imagens (resize, thumbnail)?
**R:** Opções:
1. **Cloudflare Images** (pago, mas integrado)
2. **Image Resizing no .NET** (ImageSharp, SkiaSharp)
3. **Lambda/Worker** para processamento assíncrono

Exemplo com ImageSharp:
```csharp
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

var image = Image.Load(fileStream);
image.Mutate(x => x.Resize(800, 600));
```

---

## 💰 Custos

### P: O plano gratuito é suficiente para produção?
**R:** Depende da escala:
- **Sim** para: MVP, startups pequenas, projetos pessoais
- **Talvez** para: aplicações médias (monitore o uso)
- **Não** para: grandes empresas com milhões de usuários

### P: O que acontece se ultrapassar o limite gratuito?
**R:** O Cloudflare começa a cobrar automaticamente:
- Storage: $0.015/GB/mês
- Operações Classe A (write): $4.50/milhão
- Operações Classe B (read): $0.36/milhão
- **Saída de dados**: SEMPRE GRÁTIS

### P: Como monitorar o uso?
**R:** 
1. Dashboard Cloudflare → R2 → Analytics
2. Veja: storage usado, operações, requests
3. Configure alertas de uso

### P: Vale mais a pena que AWS S3?
**R:** Para a maioria dos casos, **SIM**:
- R2: $0.015/GB + egress grátis
- S3: $0.023/GB + $0.09/GB de egress

Economia significativa em tráfego de saída!

---

## 📊 Boas Práticas

### P: Devo salvar a URL no banco de dados?
**R:** **Sim**, salve em `User.ProfilePicture`. Vantagens:
- ✅ Acesso rápido
- ✅ Facilita queries
- ✅ Histórico de alterações

### P: Como organizar os arquivos no bucket?
**R:** Use estrutura de pastas lógica:
```
bucket/
├── profile-pictures/
│   ├── user-123.jpg
│   └── user-456.png
├── documents/
│   └── invoice-001.pdf
└── company-logos/
    └── company-789.svg
```

### P: Devo versionar os arquivos?
**R:** Para fotos de perfil, geralmente não (sobrescreve). Mas para documentos importantes, sim:
```
documents/
├── invoice-001-v1.pdf
├── invoice-001-v2.pdf
└── invoice-001-v3.pdf
```

### P: Como implementar log de uploads?
**R:** O sistema já loga. Para log detalhado:

```csharp
_logger.LogInformation("Upload started - User: {UserId}, File: {FileName}, Size: {Size}", 
    userId, fileName, fileSize);

// ... upload ...

_logger.LogInformation("Upload completed - URL: {Url}, Duration: {Duration}ms", 
    fileUrl, stopwatch.ElapsedMilliseconds);
```

---

## 🚀 Próximos Passos

### P: Como integrar com o frontend?
**R:** Veja exemplos em [API_TEST_EXAMPLES.md](./API_TEST_EXAMPLES.md):
- React
- Vue.js
- Angular
- Vanilla JavaScript

### P: Como fazer deploy em produção?
**R:** 
1. Configure variáveis de ambiente
2. Use HTTPS
3. Adicione autenticação JWT
4. Configure rate limiting
5. Monitore logs e métricas

### P: Onde encontro mais documentação?
**R:** 
- [QUICK_START.md](./QUICK_START.md) - Início rápido
- [CLOUDFLARE_R2_SETUP.md](./CLOUDFLARE_R2_SETUP.md) - Guia completo
- [API_TEST_EXAMPLES.md](./API_TEST_EXAMPLES.md) - Exemplos de teste
- [Cloudflare R2 Docs](https://developers.cloudflare.com/r2/)
- [AWS SDK .NET Docs](https://docs.aws.amazon.com/sdk-for-net/)

---

**📧 Tem outra pergunta?** Abra uma issue ou consulte a documentação oficial!

*Atualizado: Janeiro 2025*