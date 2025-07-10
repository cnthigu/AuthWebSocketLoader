using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

public class BotService
{
    private readonly DiscordSocketClient _client;

    public BotService()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds
                             | GatewayIntents.GuildMessages
                             | GatewayIntents.MessageContent
        });
    }

    public async Task StartAsync()
    {
        _client.Log += LogAsync;
        _client.MessageReceived += HandleMessageAsync;

        var token = "SUA KEY AQUI";
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
    }

    private Task LogAsync(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(SocketMessage message)
    {
        if (message.Author.IsBot) return;

        Console.WriteLine($"Mensagem recebida: {message.Content} de {message.Author.Username}");

        var content = message.Content.Split(' ');
        var command = content[0].ToLower();

        using var db = new AppDbContext();

        if (command == "!adddias" && content.Length == 3)
        {
            if (!int.TryParse(content[2], out int dias))
            {
                await message.Channel.SendMessageAsync("Número de dias inválido.");
                return;
            }

            var username = content[1];
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.DataExpiracao = user.DataExpiracao > DateTime.Now
                    ? user.DataExpiracao.AddDays(dias)
                    : DateTime.Now.AddDays(dias);

                await db.SaveChangesAsync();
                await message.Channel.SendMessageAsync($"✅ {dias} dias adicionados para {username}.");
            }
            else
            {
                await message.Channel.SendMessageAsync($"❌ Usuário {username} não encontrado.");
            }
            return;
        }

        if (command == "!limparhwid" && content.Length == 2)
        {
            var username = content[1];
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.HWID = null;
                await db.SaveChangesAsync();
                await message.Channel.SendMessageAsync($"✅ HWID limpo para {username}.");
            }
            else
            {
                await message.Channel.SendMessageAsync($"❌ Usuário {username} não encontrado.");
            }
            return;
        }
    }
}
