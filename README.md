# 📖 Meu Livro de Receitas





## 🎯 Sobre o Projeto

O **Meu Livro de Receitas** é uma API projetada para quem gosta cozinhar e quer organizar suas receitas de forma digital. Desenvolvida seguindo os princípios de **Domain-Driven Design (DDD)** e **SOLID**, ela oferece uma arquitetura modular, testável e escalável.

### 🚀 Funcionalidades Principais

- ✅ **Gerenciamento de Usuários**
  - Cadastro de usuários com validação de email e senha
  - Autenticação JWT com Refresh Token
  - Login com Google OAuth
  - Atualização de perfil e alteração de senha
  - Exclusão de conta (com mensageria assíncrona)

- 🍳 **Gerenciamento de Receitas**
  - Criar, editar, deletar e listar receitas
  - Filtrar receitas por título, categoria, dificuldade e tempo de preparo
  - Buscar receita por ID
  - Upload de imagem de capa
  - Geração automática de receitas usando ChatGPT/OpenAI

- 📊 **Dashboard**
  - Visualizar todas as receitas do usuário logado

- 🔐 **Segurança**
  - Autenticação JWT
  - Refresh Token para renovação automática
  - Senhas criptografadas com BCrypt
  - IDs codificados (Sqids) para segurança adicional

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM para acesso a dados
- **FluentMigrator** - Migrações de banco de dados
- **FluentValidation** - Validação de dados
- **JWT** - Autenticação e autorização
- **BCrypt** - Criptografia de senhas
- **AutoMapper** - Mapeamento de objetos
- **Sqids** - Codificação de IDs

### Banco de Dados
- **MySQL** - Suporte completo
- **SQL Server** - Suporte completo

### Integrações
- **OpenAI/ChatGPT** - Geração de receitas por IA
- **Azure Blob Storage** - Armazenamento de imagens
- **Azure Service Bus** - Mensageria para processos assíncronos
- **Google OAuth** - Login com Google

### Qualidade e DevOps
- **xUnit** - Testes unitários e de integração
- **SonarCloud** - Análise de qualidade de código
- **Azure DevOps** - CI/CD Pipeline
- **Docker** - Containerização

### Documentação
- **Swagger/OpenAPI** - Documentação interativa da API

![badge-dot-net]
![badge-windows]
![badge-visual-studio]
![badge-mysql]
![badge-sqlserver]
![badge-swagger]
![badge-docker]
![badge-azure-devops]
![badge-azure]
![badge-azure-pipelines]
![badge-google]
![badge-openai]
![badge-sonarcloud]

## 📋 Pré-requisitos

Antes de começar, você precisa ter instalado:

- **.NET SDK 8.0** ou superior ([Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
- **Visual Studio 2022+** ou **Visual Studio Code** ou **Rider**
- **MySQL Server** ou **SQL Server** (ou use Docker)
- **Git** para clonar o repositório

### Opcionais (para funcionalidades completas)
- Conta no **Azure** (para Blob Storage e Service Bus)
- Chave da API **OpenAI** (para geração de receitas)
- Credenciais do **Google OAuth** (para login com Google)

## 🚀 Como Usar

### 1. Clonar o Repositório

```bash
git clone https://github.com/SEU_USUARIO/MyRecipeBook.git
cd MyRecipeBook
```

### 2. Preencha as informações no arquivo appsettings.Development.json

### 3. Execute a API


![hero-image]

<!-- Links -->
[dot-net-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[curso-udemy]: https://www.udemy.com/course/net-core-curso-orientado-para-mercado-de-trabalho/?referralCode=C0850BF224055DE39722

<!-- Images -->
[hero-image]: images/heroheader.png

<!-- Sonarcloud -->
[sonarcloud-dashboard]: https://sonarcloud.io/summary/overall?id=welissonArley_MyRecipeBook
[sonarcloud-qualityGate]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=alert_status
[sonarcloud-bugs]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=bugs
[sonarcloud-vulnerabilities]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=vulnerabilities
[sonarcloud-code-smells]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=code_smells
[sonarcloud-coverage]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=coverage
[sonarcloud-duplicated-lines]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=duplicated_lines_density

<!-- Badges -->
[badge-sqlserver]: https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=fff&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-windows]: https://img.shields.io/badge/Windows-0078D4?logo=windows&logoColor=fff&style=for-the-badge
[badge-visual-studio]: https://img.shields.io/badge/Visual%20Studio-5C2D91?logo=visualstudio&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
[badge-docker]: https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=fff&style=for-the-badge
[badge-azure-devops]: https://img.shields.io/badge/Azure%20DevOps-0078D7?logo=azuredevops&logoColor=fff&style=for-the-badge
[badge-azure]: https://img.shields.io/badge/Microsoft%20Azure-0078D4?logo=microsoftazure&logoColor=fff&style=for-the-badge
[badge-azure-pipelines]: https://img.shields.io/badge/Azure%20Pipelines-2560E0?logo=azurepipelines&logoColor=fff&style=for-the-badge
[badge-google]: https://img.shields.io/badge/Google-4285F4?logo=google&logoColor=fff&style=for-the-badge
[badge-openai]: https://img.shields.io/badge/OpenAI-412991?logo=openai&logoColor=fff&style=for-the-badge
[badge-sonarcloud]: https://img.shields.io/badge/SonarCloud-F3702A?logo=sonarcloud&logoColor=fff&style=for-the-badge
