using System.Collections.Concurrent;

namespace APICatalago.Logging;

// Provedor customizado de loggers que gerencia e fornece instâncias de CustomerLogger
public class CustomLoggerProvider : ILoggerProvider
{
    // Configuração com o nível de log que será respeitado pelos loggers
    private readonly CustomLoggerProviderConfiguration loggerConfig;

    // Dicionário concorrente para armazenar loggers já criados, reutilizando-os por categoria
    private readonly ConcurrentDictionary<string, CustomerLogger> loggers =
        new ConcurrentDictionary<string, CustomerLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        loggerConfig = config;
    }

    // Cria ou retorna um logger associado à categoria (nome do contexto que está logando)
    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
    }

    // Libera os loggers armazenados
    public void Dispose()
    {
        loggers.Clear();
    }
}