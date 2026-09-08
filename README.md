# Sistema de Gestão de Consultas UVV

Aplicação ASP.NET Core MVC + Entity Framework Core (Code First) para cadastro de
usuários, login e gerenciamento (CRUD) de consultas.

> **Obs:** Link do vídeo demonstrativo:  
> https://youtube.com/shorts/PsUsW_eofLc?si=mH0DCj0IVxo1k6W1

## Pré-requisitos

- .NET SDK 10.0 (https://dotnet.microsoft.com/download)
- SQL Server LocalDB (já vem com o Visual Studio) ou uma instância SQL Server
- Visual Studio 2022 ou VS Code

## Como rodar

1. Abra a pasta do projeto no Visual Studio (arquivo `.csproj`) ou no terminal.
2. Restaure os pacotes (o Visual Studio faz isso sozinho ao abrir; via terminal):

   ```
   dotnet restore
   ```

3. Confira a connection string em `appsettings.json`. Por padrão usa o LocalDB:

   ```
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GestaoConsultasUVV;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   ```

4. Crie a migration inicial e o banco de dados (Code First):

   ```
   dotnet tool install --global dotnet-ef   # se ainda não tiver o dotnet-ef instalado
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

   No Visual Studio, isso também pode ser feito pelo **Package Manager Console**:

   ```
   Add-Migration InitialCreate
   Update-Database
   ```

5. Rode a aplicação:

   ```
   dotnet run
   ```

6. Acesse a URL exibida no terminal (algo como `https://localhost:xxxx`), crie
   uma conta em **Cadastrar** e depois faça **Login**.

## Estrutura do projeto

```
GestaoConsultasUVV/
├── Models/              -> Usuario.cs, Consulta.cs (entidades com Data Annotations)
├── ViewModels/           -> LoginViewModel, RegisterViewModel
├── Data/                 -> ApplicationDbContext.cs (EF Core)
├── Controllers/          -> HomeController, AccountController, ConsultasController
├── Views/                -> Razor Views (Account/Login, Register; Consultas/Index, Create, Edit, Delete)
├── Program.cs            -> DI do DbContext + pipeline de middlewares
└── appsettings.json      -> Connection string
```

## Como os requisitos do Trabalho Prático foram atendidos

### A. Modelagem e Persistência (EF Core)
- **Code First**: entidades em `Models/Usuario.cs` e `Models/Consulta.cs`, banco gerado
  via `dotnet ef migrations` (veja seção "Como rodar").
- **Relacionamento**: `Consulta.UsuarioId` é FK para `Usuario`, mapeado em
  `ApplicationDbContext.OnModelCreating` (1 Usuário : N Consultas).
- **Validação**: `[Required]`, `[EmailAddress]`, `[StringLength]` em ambos os models
  e nos ViewModels de Login/Registro.

### B. Funcionalidades (CRUD e Fluxo)
- **Cadastro de Usuário**: `AccountController.Register` (GET exibe formulário,
  POST grava no banco com senha em hash via `PasswordHasher<Usuario>`).
- **Login**: `AccountController.Login` valida e-mail/senha e autentica via Cookie
  (`SignInAsync`).
- **Consultas**: `ConsultasController` com `Index` (lista só as consultas do
  usuário logado), `Create`, `Edit` e `Delete` completos.

### C. Arquitetura e Configuração
- **Injeção de Dependência**: `builder.Services.AddDbContext<ApplicationDbContext>(...)`
  em `Program.cs`.
- **Pipeline de Middleware**: em `Program.cs`, `app.UseAuthentication()` vem
  **antes** de `app.UseAuthorization()`, que por sua vez vem depois de
  `app.UseRouting()`.
- **Segurança**: `ConsultasController` é decorado com `[Authorize]` (todas as
  ações exigem login); além disso, cada consulta só é visível/editável pelo
  próprio dono (`UsuarioId == UsuarioLogadoId`), evitando que um usuário
  autenticado acesse consultas de outro apenas trocando o `id` na URL.

## Observações importantes

- As senhas **nunca** são armazenadas em texto puro — usamos
  `Microsoft.AspNetCore.Identity.PasswordHasher<Usuario>`, que aplica um hash
  seguro (PBKDF2) com salt.
- Este projeto usa autenticação **por Cookie feita manualmente** (não o
  ASP.NET Core Identity completo), conforme pedido no enunciado (entidade
  `Usuario` própria com Nome/E-mail/Senha/DataCadastro).
- Se preferir usar **Razor Pages** em vez de MVC, ou o **ASP.NET Core Identity**
  completo (com Roles, confirmação de e-mail etc.), a estrutura de Models e
  DbContext pode ser reaproveitada quase integralmente.
