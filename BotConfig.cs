using System.Text.Json;

public class BotConfig
{
    public string Host { get; set; } = "";
    public int Port { get; set; }
    public string Username { get; set; } = "ServerBot";
    public int ReconnectSeconds { get; set; } = 10;

    public static BotConfig Load()
    {
        string file = "BotConfig.json";

        if (!File.Exists(file))
        {
            throw new FileNotFoundException(
                $"No se encontró el archivo {file}"
            );
        }

        string json = File.ReadAllText(file);

        BotConfig? config =
            JsonSerializer.Deserialize<BotConfig>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

        if (config == null)
        {
            throw new Exception("BotConfig.json no contiene una configuración válida.");
        }

        if (string.IsNullOrWhiteSpace(config.Host) ||
            config.Host == "TU_IP_DEL_SERVIDOR")
        {
            throw new Exception(
                "Debes colocar la IP de tu servidor en BotConfig.json."
            );
        }

        if (config.Port <= 0)
        {
            throw new Exception(
                "El puerto del servidor no es válido."
            );
        }

        return config;
    }
}
