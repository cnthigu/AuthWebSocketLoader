using WebSocketSharp.Server;

class Program
{
    static async Task Main(string[] args)
    {
        var wssv = new WebSocketServer("ws://0.0.0.0:8080");
        wssv.AddWebSocketService<AuthBehavior>("/auth");
        wssv.Start();
        Console.WriteLine("Servidor WebSocket iniciado em ws://localhost:8080/auth");

        var bot = new BotService();
        await bot.StartAsync(); 

        Console.ReadKey();
        wssv.Stop();
    }
}
