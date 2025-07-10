using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

public class BotService
{
    private readonly DiscordSocketClient _client;
    private InteractionService _interactionService;
    private IServiceProvider _services;

    public BotService()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
        });
        _interactionService = new InteractionService(_client.Rest);
    }

    public async Task StartAsync()
    {
        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;
        _client.InteractionCreated += HandleInteraction;

        var services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_interactionService)
            .AddSingleton<AppDbContext>()
            .AddSingleton<SlashCommands>()
            .BuildServiceProvider();

        _services = services;

        var token = "";
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task ReadyAsync()
    {
        await _interactionService.AddModulesAsync(typeof(SlashCommands).Assembly, _services);

        await _interactionService.RegisterCommandsGloballyAsync();

        Console.WriteLine("Bot pronto!");
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        var ctx = new SocketInteractionContext(_client, interaction);

        try
        {
            await _interactionService.ExecuteCommandAsync(ctx, _services);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            if (interaction.Type == InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync().ContinueWith(async msg => await msg.Result.DeleteAsync());
        }
    }

    private Task LogAsync(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
