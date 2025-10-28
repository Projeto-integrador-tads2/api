#!/bin/bash

# Script de teste rápido para upload de foto de perfil
# Uso: ./test-upload.sh <userId> <caminho-da-imagem>

set -e

# Cores para output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configurações
API_URL="http://localhost:5000"
ENDPOINT="/api/User"

# Banner
echo ""
echo "╔════════════════════════════════════════╗"
echo "║   🧪 Teste de Upload - CRM API        ║"
echo "╔════════════════════════════════════════╗"
echo ""

# Validar argumentos
if [ $# -lt 2 ]; then
    echo -e "${RED}❌ Erro: Argumentos insuficientes${NC}"
    echo ""
    echo "Uso: $0 <userId> <caminho-da-imagem>"
    echo ""
    echo "Exemplos:"
    echo "  $0 3fa85f64-5717-4562-b3fc-2c963f66afa6 foto.jpg"
    echo "  $0 \$(uuidgen) ~/Pictures/perfil.png"
    echo ""
    exit 1
fi

USER_ID=$1
IMAGE_PATH=$2
CUSTOM_NAME=$3

# Verificar se o arquivo existe
if [ ! -f "$IMAGE_PATH" ]; then
    echo -e "${RED}❌ Erro: Arquivo não encontrado: $IMAGE_PATH${NC}"
    exit 1
fi

# Verificar se é uma imagem
FILE_TYPE=$(file -b --mime-type "$IMAGE_PATH")
if [[ ! "$FILE_TYPE" =~ ^image/ ]]; then
    echo -e "${YELLOW}⚠️  Aviso: O arquivo não parece ser uma imagem (tipo: $FILE_TYPE)${NC}"
    read -p "Deseja continuar? (s/N) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Ss]$ ]]; then
        exit 1
    fi
fi

# Informações do arquivo
FILE_SIZE=$(du -h "$IMAGE_PATH" | cut -f1)
FILE_NAME=$(basename "$IMAGE_PATH")

echo -e "${YELLOW}📋 Informações do Upload:${NC}"
echo "  User ID:       $USER_ID"
echo "  Arquivo:       $FILE_NAME"
echo "  Tamanho:       $FILE_SIZE"
echo "  Tipo MIME:     $FILE_TYPE"
echo "  API URL:       $API_URL$ENDPOINT/$USER_ID/profile-picture"
echo ""

# Construir comando curl
CURL_CMD="curl -X POST \"$API_URL$ENDPOINT/$USER_ID/profile-picture\" \
  -H \"Content-Type: multipart/form-data\" \
  -F \"file=@$IMAGE_PATH\""

if [ -n "$CUSTOM_NAME" ]; then
    CURL_CMD="$CURL_CMD -F \"customFileName=$CUSTOM_NAME\""
    echo "  Nome custom:   $CUSTOM_NAME"
    echo ""
fi

echo -e "${YELLOW}🚀 Enviando arquivo...${NC}"
echo ""

# Fazer o upload
RESPONSE=$(eval "$CURL_CMD -w \"\\n%{http_code}\"" -s)

# Separar body e status code
HTTP_BODY=$(echo "$RESPONSE" | head -n -1)
HTTP_STATUS=$(echo "$RESPONSE" | tail -n 1)

# Verificar resultado
if [ "$HTTP_STATUS" -eq 200 ]; then
    echo -e "${GREEN}✅ Upload realizado com sucesso!${NC}"
    echo ""
    echo -e "${YELLOW}📊 Resposta da API:${NC}"
    echo "$HTTP_BODY" | python3 -m json.tool 2>/dev/null || echo "$HTTP_BODY"
    echo ""

    # Extrair URL da imagem
    IMAGE_URL=$(echo "$HTTP_BODY" | grep -o '"fileUrl":"[^"]*"' | cut -d'"' -f4)

    if [ -n "$IMAGE_URL" ]; then
        echo -e "${GREEN}🌐 URL da imagem:${NC}"
        echo "  $IMAGE_URL"
        echo ""

        # Oferecer para abrir no navegador
        read -p "Deseja abrir a imagem no navegador? (s/N) " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Ss]$ ]]; then
            if command -v xdg-open &> /dev/null; then
                xdg-open "$IMAGE_URL"
            elif command -v open &> /dev/null; then
                open "$IMAGE_URL"
            else
                echo -e "${YELLOW}⚠️  Não foi possível abrir automaticamente. Copie a URL acima.${NC}"
            fi
        fi
    fi
else
    echo -e "${RED}❌ Erro no upload (HTTP $HTTP_STATUS)${NC}"
    echo ""
    echo -e "${YELLOW}📊 Resposta do servidor:${NC}"
    echo "$HTTP_BODY" | python3 -m json.tool 2>/dev/null || echo "$HTTP_BODY"
    echo ""
    exit 1
fi

echo ""
echo -e "${GREEN}✨ Teste concluído!${NC}"
echo ""
