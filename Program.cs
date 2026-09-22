using McProtoNet;
using HandshakeSb = McProtoNet.Protocol.Packets.Handshaking.Serverbound;
using LoginSb = McProtoNet.Protocol.Packets.Login.Serverbound;

const int ProtocolVersion = 775;

Console.WriteLine("=================================");
Console.WriteLine(" MinecraftServerBot");
Console.WriteLine("=================================");

BotConfig config = BotConfig.Load();

Console.WriteLine($"Servidor: {config.Host}:{config.Port}");
Console.WriteLine($"Nombre: {config.Username}");
Console.WriteLine($"Protocolo: {ProtocolVersion}");
Console.WriteLine();

while (true)
{
    try
    {
        Console.WriteLine("Conectando al servidor...");

        await using var client = new MinecraftClient(
            new MinecraftClientOptions
            {
                Host = config.Host,
                Port = config.Port
            });

        await client.ConnectAsync();

        Console.WriteLine("TCP conectado.");

        await client.SendAsync(
            new HandshakeSb.SetProtocolPacket(
                ProtocolVersion,
                config.Host,
                config.Port,
                2),
            ProtocolVersion);

        Console.WriteLine("Handshake enviado.");

        await client.SendAsync(
            new LoginSb.LoginStartPacket(
                config.Username,
                V764_Last: new(Guid.NewGuid())),
            ProtocolVersion);

        Console.WriteLine("Login enviado.");

        await foreach (var packet in client.ReadPacketsAsync())
        {
            Console.WriteLine(
                $"Paquete recibido: {packet.GetType().Name}");
        }

        Console.WriteLine("La conexión terminó.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("ERROR:");
        Console.WriteLine(ex.Message);
        Console.WriteLine();
    }

    Console.WriteLine(
        $"Reintentando en {config.ReconnectSeconds} segundos...");

    await Task.Delay(
        TimeSpan.FromSeconds(config.ReconnectSeconds));
}
