#!/bin/bash

# Script de configuração do MinIO para desenvolvimento local
# Este script inicia o MinIO e cria o bucket necessário

set -e

# Cores
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo ""
echo "╔════════════════════════════════════════╗"
echo "║   🚀 Setup MinIO - Storage Local      ║"
echo "╔════════════════════════════════════════╗"
echo ""

# Verificar se Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo -e "${RED}❌ Docker não está rodando!${NC}"
    echo ""
    echo "Inicie o Docker primeiro:"
    echo "  sudo systemctl start docker"
    echo "  OU abra o Docker Desktop"
    echo ""
    exit 1
fi

echo -e "${GREEN}✅ Docker está rodando${NC}"
echo ""

# Parar containers existentes
echo -e "${YELLOW}🛑 Parando containers existentes...${NC}"
docker-compose -f docker-compose.minio.yml down 2>/dev/null || true
echo ""

# Subir MinIO
echo -e "${YELLOW}🚀 Iniciando MinIO...${NC}"
docker-compose -f docker-compose.minio.yml up -d

# Aguardar MinIO iniciar
echo -e "${YELLOW}⏳ Aguardando MinIO iniciar...${NC}"
sleep 5

# Verificar se MinIO está rodando
if ! docker ps | grep -q minio; then
    echo -e "${RED}❌ Erro ao iniciar MinIO!${NC}"
    echo ""
    echo "Verifique os logs:"
    echo "  docker-compose -f docker-compose.minio.yml logs"
    exit 1
fi

echo -e "${GREEN}✅ MinIO iniciado com sucesso!${NC}"
echo ""

# Instalar MinIO Client (mc) se não existir
if ! command -v mc &> /dev/null; then
    echo -e "${YELLOW}📦 Instalando MinIO Client (mc)...${NC}"

    if [[ "$OSTYPE" == "linux-gnu"* ]]; then
        wget -q https://dl.min.io/client/mc/release/linux-amd64/mc -O /tmp/mc
        chmod +x /tmp/mc
        sudo mv /tmp/mc /usr/local/bin/
    elif [[ "$OSTYPE" == "darwin"* ]]; then
        brew install minio/stable/mc 2>/dev/null || {
            wget -q https://dl.min.io/client/mc/release/darwin-amd64/mc -O /tmp/mc
            chmod +x /tmp/mc
            sudo mv /tmp/mc /usr/local/bin/
        }
    fi

    echo -e "${GREEN}✅ MinIO Client instalado${NC}"
else
    echo -e "${GREEN}✅ MinIO Client já está instalado${NC}"
fi
echo ""

# Configurar alias do MinIO
echo -e "${YELLOW}🔧 Configurando MinIO Client...${NC}"
mc alias set local http://localhost:9002 minioadmin minioadmin123 > /dev/null 2>&1 || {
    echo -e "${RED}❌ Erro ao configurar alias do MinIO${NC}"
    echo ""
    echo "Tente manualmente:"
    echo "  mc alias set local http://localhost:9002 minioadmin minioadmin123"
    exit 1
}
echo -e "${GREEN}✅ MinIO Client configurado${NC}"
echo ""

# Criar bucket
echo -e "${YELLOW}🪣 Criando bucket 'crm-files'...${NC}"
if mc ls local/crm-files > /dev/null 2>&1; then
    echo -e "${YELLOW}⚠️  Bucket 'crm-files' já existe${NC}"
else
    mc mb local/crm-files
    echo -e "${GREEN}✅ Bucket criado com sucesso${NC}"
fi
echo ""

# Configurar política de acesso público
echo -e "${YELLOW}🔓 Configurando acesso público...${NC}"
mc anonymous set download local/crm-files
echo -e "${GREEN}✅ Acesso público configurado${NC}"
echo ""

# Resumo
echo "╔════════════════════════════════════════╗"
echo "║          ✨ SETUP CONCLUÍDO! ✨        ║"
echo "╔════════════════════════════════════════╗"
echo ""
echo -e "${GREEN}📊 Informações de Acesso:${NC}"
echo ""
echo "  🌐 Console Web:    http://localhost:9003"
echo "  🔌 API Endpoint:   http://localhost:9002"
echo "  👤 Usuário:        minioadmin"
echo "  🔑 Senha:          minioadmin123"
echo "  🪣 Bucket:         crm-files"
echo ""
echo -e "${YELLOW}📋 Próximos passos:${NC}"
echo ""
echo "  1. Acesse o console: http://localhost:9003"
echo "  2. Faça login com as credenciais acima"
echo "  3. Verifique o bucket 'crm-files'"
echo "  4. Inicie sua API: dotnet run"
echo "  5. Teste o upload via Swagger: http://localhost:5000/swagger"
echo ""
echo -e "${GREEN}✅ MinIO pronto para uso!${NC}"
echo ""

# Verificar status
echo -e "${YELLOW}📊 Status dos containers:${NC}"
docker ps --filter "name=minio" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
echo ""
