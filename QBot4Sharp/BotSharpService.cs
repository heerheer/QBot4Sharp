using Microsoft.Extensions.Hosting;

namespace QBot4Sharp;

public class BotSharpService : IHostedService
{
    public BotSharpService()
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}