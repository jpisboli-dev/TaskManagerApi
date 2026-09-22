# TaskManager API

API REST em ASP.NET Core 10 para gerenciamento de tarefas, com autenticação JWT e banco SQLite via Entity Framework Core.

## Tecnologias
- ASP.NET Core 10 (Web API)
- Entity Framework Core + SQLite
- Autenticação JWT
- BCrypt para hash de senha
- Swagger / OpenAPI

## Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado

## Como rodar

```bash
cd TaskManagerApi
dotnet restore
dotnet run
```

O Swagger abre automaticamente em `https://localhost:5001/swagger` (ou porta similar mostrada no terminal). O banco SQLite (`taskmanager.db`) é criado automaticamente na primeira execução.

## Fluxo de uso

### 1. Registrar usuário
`POST /api/auth/register`
```json
{
  "name": "João Pedro",
  "email": "joao@exemplo.com",
  "password": "senha123"
}
```
Retorna um token JWT.

### 2. Login
`POST /api/auth/login`
```json
{
  "email": "joao@exemplo.com",
  "password": "senha123"
}
```

### 3. Usar o token
No Swagger, clique em **Authorize** e cole apenas o token (sem "Bearer "). Em outras ferramentas (Postman, curl), envie o header:
```
Authorization: Bearer {seu_token}
```

### 4. Endpoints de tarefas (todos autenticados)

| Método | Rota                     | Descrição                          |
|--------|---------------------------|-------------------------------------|
| GET    | /api/tasks                | Lista as tarefas do usuário logado |
| GET    | /api/tasks?status=Pending | Filtra por status                  |
| GET    | /api/tasks/{id}           | Busca uma tarefa específica        |
| POST   | /api/tasks                | Cria uma nova tarefa               |
| PUT    | /api/tasks/{id}           | Atualiza uma tarefa completa       |
| PATCH  | /api/tasks/{id}/status    | Atualiza só o status               |
| DELETE | /api/tasks/{id}           | Remove uma tarefa                  |

Exemplo de criação:
```json
{
  "title": "Estudar EF Core",
  "description": "Revisar relacionamentos e migrations",
  "priority": "High",
  "dueDate": "2026-10-01T00:00:00Z"
}
```

Status possíveis: `Pending`, `InProgress`, `Done`
Prioridades possíveis: `Low`, `Medium`, `High`

## Estrutura do projeto

```
TaskManagerApi/
├── Controllers/       # AuthController, TasksController
├── Models/             # User, TaskItem
├── DTOs/                # Objetos de entrada/saída da API
├── Data/                # AppDbContext (EF Core)
├── Services/           # TokenService (geração de JWT)
├── Program.cs           # Configuração da aplicação
└── appsettings.json      # Connection string e config do JWT
```

## Próximos passos sugeridos
- Trocar SQLite por PostgreSQL/SQL Server em produção
- Adicionar paginação em `GET /api/tasks`
- Adicionar testes automatizados (xUnit)
- Gerar migrations reais com `dotnet ef migrations add InitialCreate` em vez de `EnsureCreated()`
- Adicionar refresh token
- Dockerizar a aplicação

## Importante sobre segurança
A chave JWT em `appsettings.json` é um placeholder — troque por uma chave forte e mantenha em variável de ambiente ou `dotnet user-secrets` antes de usar em produção.
# TaskManagerApi
