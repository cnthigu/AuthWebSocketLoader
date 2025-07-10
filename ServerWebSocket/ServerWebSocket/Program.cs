using WebSocketSharp.Server;

class Program
{
    static void Main()
    {
        var wssv = new WebSocketServer("ws://0.0.0.0:8080");

        wssv.AddWebSocketService<AuthBehavior>("/auth");

        wssv.Start();
        Console.WriteLine("Servidor WebSocket iniciado em ws://localhost:8080/auth");
        Console.WriteLine("Pressione qualquer tecla para encerrar...");
        Console.ReadKey();
        wssv.Stop();
    }
}
