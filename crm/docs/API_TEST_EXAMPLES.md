# 🧪 Exemplos de Teste - API de Upload de Fotos

## 📋 Índice
- [Teste via cURL](#teste-via-curl)
- [Teste via Postman](#teste-via-postman)
- [Teste via C# HttpClient](#teste-via-c-httpclient)
- [Teste via JavaScript/Fetch](#teste-via-javascriptfetch)
- [Teste via Python](#teste-via-python)

---

## 🔧 Pré-requisitos

Antes de testar, certifique-se de:
1. ✅ O servidor está rodando (`dotnet run`)
2. ✅ Você tem um `userId` válido no banco de dados
3. ✅ O Cloudflare R2 está configurado corretamente

---

## 🌐 Teste via cURL

### Upload Simples (sem nome customizado)

```bash
curl -X POST "http://localhost:5000/api/User/3fa85f64-5717-4562-b3fc-2c963f66afa6/profile-picture" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/caminho/para/foto.jpg"
```

### Upload com Nome Customizado

```bash
curl -X POST "http://localhost:5000/api/User/3fa85f64-5717-4562-b3fc-2c963f66afa6/profile-picture" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/caminho/para/foto.jpg" \
  -F "customFileName=meu-perfil.jpg"
```

### Upload com Autenticação JWT

```bash
curl -X POST "http://localhost:5000/api/User/3fa85f64-5717-4562-b3fc-2c963f66afa6/profile-picture" \
  -H "Authorization: Bearer SEU_TOKEN_JWT_AQUI" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/caminho/para/foto.jpg"
```

### Visualizar Resposta Formatada

```bash
curl -X POST "http://localhost:5000/api/User/3fa85f64-5717-4562-b3fc-2c963f66afa6/profile-picture" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/caminho/para/foto.jpg" \
  | json_pp
```

---

## 📮 Teste via Postman

### Passo 1: Criar Nova Request

1. Abra o Postman
2. Clique em **New** > **HTTP Request**
3. Configure:
   - **Method**: `POST`
   - **URL**: `http://localhost:5000/api/User/{userId}/profile-picture`

### Passo 2: Configurar Body

1. Vá na aba **Body**
2. Selecione **form-data**
3. Adicione os campos:

| Key | Type | Value |
|-----|------|-------|
| `file` | File | Selecione sua imagem |
| `customFileName` | Text | `meu-perfil.jpg` (opcional) |

### Passo 3: Configurar Headers (se necessário)

Se sua API requer autenticação:

1. Vá na aba **Headers**
2. Adicione:

| Key | Value |
|-----|-------|
| `Authorization` | `Bearer SEU_TOKEN_JWT_AQUI` |

### Passo 4: Enviar Request

1. Clique em **Send**
2. Verifique a resposta no painel inferior

### Resposta Esperada (200 OK)

```json
{
  "fileUrl": "https://pub-xxxxxxxxxxxxxxxxxx.r2.dev/profile-pictures/abc123-foto.jpg",
  "fileName": "foto.jpg",
  "fileSize": 245678,
  "contentType": "image/jpeg",
  "uploadedAt": "2025-01-15T10:30:00.000Z"
}
```

---

## 💻 Teste via C# HttpClient

### Exemplo Completo

```csharp
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var filePath = "/caminho/para/foto.jpg";
        
        await UploadProfilePictureAsync(userId, filePath);
    }
    
    static async Task UploadProfilePictureAsync(Guid userId, string filePath)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:5000");
        
        // Se precisar de autenticação:
        // httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer SEU_TOKEN_JWT");
        
        using var form = new MultipartFormDataContent();
        
        // Adicionar arquivo
        var fileStream = File.OpenRead(filePath);
        var fileName = Path.GetFileName(filePath);
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        form.Add(streamContent, "file", fileName);
        
        // Adicionar nome customizado (opcional)
        // form.Add(new StringContent("meu-perfil.jpg"), "customFileName");
        
        var response = await httpClient.PostAsync($"/api/User/{userId}/profile-picture", form);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Upload bem-sucedido!");
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine($"Erro: {response.StatusCode}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
```

---

## 🌐 Teste via JavaScript/Fetch

### Exemplo com HTML + JavaScript

```html
<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <title>Upload de Foto de Perfil</title>
</head>
<body>
    <h1>Upload de Foto de Perfil</h1>
    
    <form id="uploadForm">
        <input type="file" id="fileInput" accept="image/*" required>
        <input type="text" id="customFileName" placeholder="Nome customizado (opcional)">
        <button type="submit">Enviar</button>
    </form>
    
    <div id="result"></div>
    
    <script>
        document.getElementById('uploadForm').addEventListener('submit', async (e) => {
            e.preventDefault();
            
            const userId = '3fa85f64-5717-4562-b3fc-2c963f66afa6'; // Substitua pelo ID real
            const fileInput = document.getElementById('fileInput');
            const customFileName = document.getElementById('customFileName').value;
            const resultDiv = document.getElementById('result');
            
            const formData = new FormData();
            formData.append('file', fileInput.files[0]);
            
            if (customFileName) {
                formData.append('customFileName', customFileName);
            }
            
            try {
                resultDiv.innerHTML = 'Enviando...';
                
                const response = await fetch(`http://localhost:5000/api/User/${userId}/profile-picture`, {
                    method: 'POST',
                    body: formData,
                    // Se precisar de autenticação:
                    // headers: {
                    //     'Authorization': 'Bearer SEU_TOKEN_JWT_AQUI'
                    // }
                });
                
                const data = await response.json();
                
                if (response.ok) {
                    resultDiv.innerHTML = `
                        <h3>Upload bem-sucedido!</h3>
                        <p><strong>URL:</strong> <a href="${data.fileUrl}" target="_blank">${data.fileUrl}</a></p>
                        <img src="${data.fileUrl}" alt="Foto de perfil" style="max-width: 200px;">
                    `;
                } else {
                    resultDiv.innerHTML = `<p style="color: red;">Erro: ${JSON.stringify(data)}</p>`;
                }
            } catch (error) {
                resultDiv.innerHTML = `<p style="color: red;">Erro: ${error.message}</p>`;
            }
        });
    </script>
</body>
</html>
```

### Exemplo com Node.js (usando FormData)

```javascript
const FormData = require('form-data');
const fs = require('fs');
const axios = require('axios');

async function uploadProfilePicture(userId, filePath) {
    const form = new FormData();
    form.append('file', fs.createReadStream(filePath));
    // form.append('customFileName', 'meu-perfil.jpg'); // Opcional
    
    try {
        const response = await axios.post(
            `http://localhost:5000/api/User/${userId}/profile-picture`,
            form,
            {
                headers: {
                    ...form.getHeaders(),
                    // 'Authorization': 'Bearer SEU_TOKEN_JWT_AQUI' // Se necessário
                }
            }
        );
        
        console.log('Upload bem-sucedido!');
        console.log(response.data);
        return response.data;
    } catch (error) {
        console.error('Erro no upload:', error.response?.data || error.message);
        throw error;
    }
}

// Uso
const userId = '3fa85f64-5717-4562-b3fc-2c963f66afa6';
const filePath = './foto.jpg';
uploadProfilePicture(userId, filePath);
```

---

## 🐍 Teste via Python

### Usando Requests

```python
import requests

def upload_profile_picture(user_id, file_path, custom_file_name=None):
    url = f"http://localhost:5000/api/User/{user_id}/profile-picture"
    
    files = {
        'file': open(file_path, 'rb')
    }
    
    data = {}
    if custom_file_name:
        data['customFileName'] = custom_file_name
    
    headers = {
        # 'Authorization': 'Bearer SEU_TOKEN_JWT_AQUI'  # Se necessário
    }
    
    response = requests.post(url, files=files, data=data, headers=headers)
    
    if response.status_code == 200:
        print("Upload bem-sucedido!")
        print(response.json())
        return response.json()
    else:
        print(f"Erro: {response.status_code}")
        print(response.text)
        return None

# Uso
user_id = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
file_path = "/caminho/para/foto.jpg"
result = upload_profile_picture(user_id, file_path)

# Com nome customizado
# result = upload_profile_picture(user_id, file_path, "meu-perfil.jpg")
```

---

## 🧪 Casos de Teste

### ✅ Teste 1: Upload de Imagem Válida (JPEG)

```bash
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -F "file=@foto.jpg"
```

**Resultado Esperado**: Status 200, URL da imagem retornada

---

### ✅ Teste 2: Upload de Imagem PNG

```bash
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -F "file=@foto.png"
```

**Resultado Esperado**: Status 200, URL da imagem retornada

---

### ❌ Teste 3: Upload de Arquivo Não Suportado (PDF)

```bash
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -F "file=@documento.pdf"
```

**Resultado Esperado**: Status 400, mensagem "Apenas imagens são permitidas"

---

### ❌ Teste 4: Upload de Arquivo Muito Grande (>5MB)

```bash
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -F "file=@imagem-grande.jpg"
```

**Resultado Esperado**: Status 400, mensagem "Arquivo muito grande (máximo 5MB)"

---

### ❌ Teste 5: Upload Sem Arquivo

```bash
curl -X POST "http://localhost:5000/api/User/{userId}/profile-picture" \
  -H "Content-Type: multipart/form-data"
```

**Resultado Esperado**: Status 400, mensagem "Arquivo não fornecido"

---

### ❌ Teste 6: Upload com UserId Inválido

```bash
curl -X POST "http://localhost:5000/api/User/00000000-0000-0000-0000-000000000000/profile-picture" \
  -F "file=@foto.jpg"
```

**Resultado Esperado**: Status 400, mensagem "Usuário não encontrado"

---

## 📊 Validações Implementadas

| Validação | Descrição | Mensagem de Erro |
|-----------|-----------|------------------|
| Arquivo obrigatório | O arquivo deve ser enviado | "Arquivo não fornecido" |
| Tipo de arquivo | Apenas JPEG, PNG, GIF, WebP | "Apenas imagens são permitidas" |
| Tamanho máximo | Máximo de 5MB | "Arquivo muito grande (máximo 5MB)" |
| Usuário existente | O userId deve existir no banco | "Usuário não encontrado" |

---

## 🔍 Verificando se o Upload Funcionou

### 1. Verificar no Cloudflare R2

1. Acesse o [Cloudflare Dashboard](https://dash.cloudflare.com/)
2. Vá em **R2** > Selecione seu bucket
3. Navegue até a pasta `profile-pictures/`
4. Você deve ver o arquivo enviado

### 2. Verificar no Banco de Dados

```sql
SELECT Id, Name, Email, ProfilePicture 
FROM User 
WHERE Id = '3fa85f64-5717-4562-b3fc-2c963f66afa6';
```

O campo `ProfilePicture` deve conter a URL do R2.

### 3. Acessar a URL Diretamente

Copie a URL retornada na resposta e cole no navegador:

```
https://pub-xxxxxxxxxxxxxxxxxx.r2.dev/profile-pictures/abc123-foto.jpg
```

A imagem deve ser exibida.

---

## 🐛 Troubleshooting

### Erro: "CORS policy"

Se testar do navegador, adicione CORS no `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Antes de app.Run():
app.UseCors("AllowAll");
```

### Erro: "Connection refused"

- Verifique se o servidor está rodando
- Confirme a porta (padrão: 5000 ou 5001)

### Erro: "Access Denied" no R2

- Verifique as credenciais no `appsettings.json`
- Confirme que o token tem permissões corretas

---

## 📝 Notas Importantes

1. ✅ Substitua `{userId}` por um GUID válido do seu banco de dados
2. ✅ Ajuste o caminho do arquivo para o correto no seu sistema
3. ✅ Se usar HTTPS, troque `http://` por `https://`
4. ✅ Em produção, sempre use HTTPS e tokens JWT
5. ✅ Teste com diferentes tipos de imagens (JPG, PNG, GIF, WebP)

---

🎉 **Pronto! Use estes exemplos para testar sua API de upload!**