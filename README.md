#  CRM para a Empresa Tantto Tecnologia

> Documento de Visão do projeto de CRM desenvolvido para centralizar e otimizar a gestão comercial da **Tantto Tecnologia**.

---

## Autores
- **Felipe Marques** – [felipemq100@gmail.com](mailto:felipemq100@gmail.com)  
- **Gabriel Ribeiro** – [gabriel.dev09@gmail.com](mailto:gabriel.dev09@gmail.com) 
- **Giovana da Silva** – [giovanarafaela798@gmail.com](mailto:giovanarafaela798@gmail.com)  
- **Tobias Alquezar** – [tobiasperassi@gmail.com](mailto:tobiasperassi@gmail.com)  

---

## Índice
- [1. Introdução](#1-introdução)
- [2. Posicionamento](#2-posicionamento)
- [3. Stakeholders e Usuários](#3-stakeholders-e-usuários)
- [4. Descrição do Produto](#4-descrição-do-produto)
- [5. Requisitos de Alto Nível](#5-requisitos-de-alto-nível)
- [6. Características de Qualidade](#6-características-de-qualidade)
- [7. Restrições](#7-restrições)
- [8. Riscos](#8-riscos)
- [9. Cronograma de Marcos](#9-cronograma-de-marcos)

---

## 1. Introdução

### 1.1 Objetivo do Projeto
Desenvolver um sistema de CRM para a **Tantto Tecnologia**, centralizando o processo comercial, facilitando a gestão de leads e negociações, além de gerar relatórios para apoio à tomada de decisão.

### 1.2 Escopo do Produto
- Cadastro de leads, organizações e pessoas  
- Acompanhamento visual por Kanban  
- Geração de relatórios  
- Acesso multiplataforma  
- Uso de tecnologias **open source**  

### 1.3 Definições
- **CRM:** Customer Relationship Management  
- **Kanban:** Modelo visual de gestão de tarefas  
- **Lead:** Potencial cliente  

### 1.4 Referências
Documentos da Tantto Tecnologia, orientações da Escola SENAI e literatura sobre CRM e gestão de vendas.

---

## 2. Posicionamento

### 2.1 Oportunidade de Negócio
A empresa carece de uma solução unificada para gerenciar leads e vendas, comprometendo sua eficiência comercial.

### 🛠 2.2 Problema a Ser Resolvido
A ausência de um sistema CRM compromete o acompanhamento de oportunidades, organização de contatos e análise de dados comerciais.

### 📋 2.3 Descrição do Produto
Sistema CRM com:
- Gestão de contatos
- Funil de vendas visual
- Relatórios
- Interface simples

### 2.4 Declaração de Posição do Produto
> O **CRM Tantto** é uma ferramenta que centraliza dados, otimiza processos e fornece visibilidade clara das oportunidades para a equipe comercial.

---

## 3. Stakeholders e Usuários

### 3.1 Stakeholders
- Thiago Martins – Representante da empresa  
- Equipe comercial da Tantto  
- Equipe técnica de desenvolvimento  

### 3.2 Perfis de Usuários
- Vendedores  
- Gestores  
- Administradores  

### 📈 3.3 Necessidades
- Organizar contatos  
- Acompanhar negociações  
- Gerar relatórios  
- Apoiar decisões estratégicas  

### 3.4 Ambiente Operacional
- Navegadores modernos  
- Layout responsivo (desktop e mobile)  
- Hospedagem em servidor de nuvem de baixo custo  

---

## 4. Descrição do Produto

### 4.1 Perspectiva
Sistema desenvolvido sob medida, com possibilidade de integrações futuras.

### ⚙ 4.2 Funcionalidades
- Cadastro de leads  
- Visualização Kanban  
- Relatórios e dashboards  
- Login com controle de perfil  

### 🔗 4.3 Suposições e Dependências
**Front-end:**
- Next.js + TypeScript  
- React, Yup, NextAuth  

**Back-end:**
- .NET + C#  
- ORM para banco de dados  
- Swagger para documentação  
- Arquitetura em camadas  

**Banco de dados:**
- MySQL  

### 4.4 Limitações
- Sem integração inicial com sistemas externos  
- Sem funcionalidades pagas  

---

## 5. Requisitos de Alto Nível

### 5.1 Funcionais
- Cadastro de leads, organizações e pessoas  
- Funil visual Kanban  
- Relatórios gerenciais  
- Controle de acesso  

### 5.2 Não Funcionais
- Interface responsiva  
- Suporte a múltiplos usuários  
- Tempo de resposta rápido  
- Uso de software livre  

---

## 6. Características de Qualidade
| Característica  | Descrição |
|----------------|-----------|
| **Usabilidade** | Interface simples e intuitiva |
| **Confiabilidade** | Sistema estável, com backups |
| **Desempenho** | Baixo tempo de resposta |
| **Segurança** | Acesso controlado |
| **Portabilidade** | Multiplataforma |

---

## 7. Restrições
- Não utilizar tecnologias pagas ou complexas  
- Não depender de hardware específico ou sistemas proprietários  

---

## 8. Riscos
- Adoção limitada pela equipe comercial  
- Mudanças de escopo  
- Infraestrutura limitada  

---

## 9. Cronograma de Marcos

| Marco                           | Data       |
|---------------------------------|------------|
| Início da análise de requisitos | 11/08/2025 |
| Protótipo de interface          | 22/08/2025 |
| Versão MVP funcional            | 03/10/2025 |
| Testes com usuários             | 07/11/2025 |
| Entrega final                   | 12/12/2025 |

---

## Tecnologias Previstas
![Next.js](https://img.shields.io/badge/Next.js-000000?style=for-the-badge&logo=next.js&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-005C84?style=for-the-badge&logo=mysql&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
