#!/bin/bash

echo "🔄 Parando containers..."
sudo docker-compose down

echo "🗑️  Removendo imagem antiga..."
sudo docker rmi crm-api 2>/dev/null || true

echo "🧹 Limpando cache do Docker..."
sudo docker builder prune -f

echo "🔨 Reconstruindo sem cache..."
sudo docker-compose build --no-cache

echo "🚀 Subindo containers..."
sudo docker-compose up -d

echo "📋 Logs da API:"
sleep 3
sudo docker-compose logs api --tail=20

echo ""
echo "✅ Pronto! Acesse http://localhost:5000/swagger"
