namespace APICatalago.Logging
{
    // Vai definir a configuração do provedor de log personalizado
    public class CustomLoggerProviderConfiguration
    {
        //  Define o nível mínimo de log com padrao LogLevel.Warning
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        // Define o ID do evento de log, com padrao sendo zero
        public int EventId { get; set; } = 0;
    }
}