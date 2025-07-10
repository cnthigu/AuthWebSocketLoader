using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;

public class SlashCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly AppDbContext _db;

    public SlashCommands(AppDbContext db)
    {
        _db = db;
    }

    [SlashCommand("adddias", "Adiciona dias de validade para um usuário")]
    public async Task AddDiasAsync(string username, int dias)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            await RespondAsync($"❌ Usuário {username} não encontrado.");
            return;
        }

        user.DataExpiracao = user.DataExpiracao > DateTime.Now
            ? user.DataExpiracao.AddDays(dias)
            : DateTime.Now.AddDays(dias);

        await _db.SaveChangesAsync();
        await RespondAsync($"✅ {dias} dias adicionados para {username}.");
    }

    [SlashCommand("limparhwid", "Limpa o HWID do usuário")]
    public async Task LimparHwidAsync(string username)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            await RespondAsync($"❌ Usuário {username} não encontrado.");
            return;
        }

        user.HWID = null;
        await _db.SaveChangesAsync();
        await RespondAsync($"✅ HWID limpo para {username}.");
    }
}
