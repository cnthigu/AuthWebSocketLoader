# AuthWebSocketLoader

Projeto completo de autenticação via WebSocket, com integração a banco de dados, painel de administração via bot do Discord e loader cliente Windows Forms para autenticação de usuários.

---

## Demonstração em vídeo


> Clique na imagem acima para assistir ao vídeo de demonstração (1:20).
Ou acesse diretamente: [https://streamable.com/7h9cbs](https://streamable.com/7h9cbs)


## Descrição

O **AuthWebSocketLoader** é uma solução que permite autenticação de usuários utilizando WebSocket, com suporte a verificação por HWID (Hardware ID), gerenciamento de usuários e comandos administrativos via bot do Discord. O projeto é composto por dois principais componentes:

- **ServerWebSocket:** Servidor WebSocket responsável pela autenticação dos usuários, gerenciamento do banco de dados e integração com bot do Discord.
- **LoaderClient:** Aplicação cliente em Windows Forms para autenticação do usuário.

---

## Funcionalidades

- Autenticação de usuários via WebSocket.
- Verificação de senha e expiração de conta.
- Validação e fixação de HWID ao usuário.
- Bloqueio de acesso caso HWID não corresponda.
- Geração e envio de JWT token após login.
- Comandos administrativos via bot do Discord:
  - Adicionar dias de validade para o usuário.
  - Limpar HWID de um usuário.
- Logs detalhados e respostas de erro para o cliente.
- Armazenamento de usuários em banco de dados SQL Server.

---

## Arquitetura e Estrutura

```
AuthWebSocketLoader/
│
├── ServerWebSocket/
│   ├── Program.cs                 # Inicialização do servidor WebSocket e bot Discord
│   ├── AuthBehavior.cs            # Lógica principal de autenticação via WebSocket
│   ├── AppDbContext.cs            # Contexto do banco de dados (Entity Framework)
│   ├── Database.cs                # (Alternativo) Persistência em arquivo JSON
│   ├── DiscordBot/
│   │   └── BotService.cs          # Implementação do bot Discord e comandos
│   ├── SlashCommands.cs           # Comandos administrativos para Discord
│   └── Migrations/                # Migrations do banco de dados
│
└── LoaderClient/
    ├── Program.cs                 # Inicialização da aplicação cliente
    └── Form1.cs                   # Interface e lógica do loader cliente
```

---

## Como Executar

### 1. Banco de Dados

Configure o SQL Server local com um banco chamado `MeuBanco`. Ajuste a string de conexão em `AppDbContext.cs` se necessário.

```csharp
optionsBuilder.UseSqlServer("Server=localhost;Database=MeuBanco;Trusted_Connection=True;TrustServerCertificate=True");
```

A tabela `Users` será criada automaticamente por migrations.

### 2. Servidor WebSocket

1. Instale as dependências do projeto ServerWebSocket.
2. Execute o projeto:
   ```
   dotnet run --project ServerWebSocket/ServerWebSocket.csproj
   ```
3. O servidor será iniciado em `ws://localhost:8080/auth`.

### 3. Bot do Discord

- Defina o token do bot Discord em `BotService.cs` (variável `token`).
- Adicione o bot ao seu servidor Discord.
- O bot irá registrar comandos globais automaticamente ao iniciar.

### 4. Loader Cliente

1. Abra e compile o projeto em LoaderClient.
2. Execute o aplicativo.
3. Preencha usuário, senha e clique em Login.

---

## Configuração

- **users.json:** (caso use persistência em arquivo) Lista de usuários com `Username`, `PasswordHash`, `DataExpiracao`, `HWID`.
- **Banco SQL:** Estrutura da tabela Users conforme migrations.
- **Bot Discord:** Necessário token válido e permissões para comandos slash.

---

## Comandos via Discord

- `/adddias <username> <dias>`  
  Adiciona dias de validade à conta do usuário.

- `/limparhwid <username>`  
  Limpa o HWID cadastrado do usuário.

---

## Dependências

- .NET 7 ou superior
- [WebSocketSharp](https://github.com/sta/websocket-sharp)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- [Discord.Net](https://github.com/discord-net/Discord.Net)
- [Microsoft.EntityFrameworkCore](https://docs.microsoft.com/ef/)
- SQL Server Local/Remoto

---

## Segurança

- O token JWT é gerado no login e enviado ao cliente.
- O HWID é utilizado para evitar compartilhamento de contas.
- Expiração automática de contas baseada em data.
- Recomenda-se alterar a chave secreta do JWT e proteger o token do bot Discord.

---

### Contato

Desenvolvido por [@cnthigu](https://github.com/cnthigu).
