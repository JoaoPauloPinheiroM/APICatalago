namespace APICatalago.Logging;

// / Define os metodos  para registrar os logs
public class CustomerLogger : ILogger
{
    private readonly string Name;
    private readonly CustomLoggerProviderConfiguration loggerConfig;

    // Construtor que recebe o nome do logger e a configuração
    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        Name = name;
        loggerConfig = config;
    }

    // Verifica o nivel de log está habilitado, se não estiver, o log  não é registrado
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    // Implementação explícita para corresponder à interface ILogger
    IDisposable ILogger.BeginScope<TState>(TState state) => null!;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string messagem = $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";

        EscreverTextoNoArquivo(messagem);
    }

    private void EscreverTextoNoArquivo(string messagem)
    {
        string caminho = @"D:\Macoratti\APICatalago\logs\log.txt";
        using (StreamWriter sw = new StreamWriter(caminho, true))
        {
            try
            {
                sw.WriteLine(messagem);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}