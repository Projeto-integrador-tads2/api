#!/bin/bash

# Script para executar o projeto CRM com Docker

echo "=== CRM Docker Setup ==="

# Verificar se o Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Inicie o Docker e tente novamente."
    exit 1
fi

echo "✅ Docker está rodando"

# Construir e executar os containers
echo "🏗️  Construindo e iniciando os containers..."
docker-compose up --build -d

# Aguardar os serviços ficarem prontos
echo "⏳ Aguardando os serviços ficarem prontos..."
sleep 10

# Verificar status dos containers
echo "📊 Status dos containers:"
docker-compose ps

echo ""
echo "🎉 Aplicação iniciada com sucesso!"
echo ""
echo "📍 URLs disponíveis:"
echo "   - API: http://localhost:5000"
echo "   - Swagger: http://localhost:5000/swagger"
echo "   - MySQL: localhost:3306"
echo ""
echo "🔑 Credenciais do MySQL:"
echo "   - Database: crm"
echo "   - User: crmuser"
echo "   - Password: crmpass"
echo "   - Root Password: root"
echo ""
echo "📝 Comandos úteis:"
echo "   - Parar: docker-compose down"
echo "   - Ver logs: docker-compose logs -f"
echo "   - Ver logs da API: docker-compose logs -f api"
echo "   - Ver logs do MySQL: docker-compose logs -f mysql"
