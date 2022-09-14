using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.TestExtension;

public class TestReducer : IReducer
{
    private ILogger<TestReducer> _logger;

    public TestReducer(ILogger<TestReducer> logger) 
    {
        _logger = logger;
    }

    public ValueTask<LiveState> ReduceAsync(LiveState state, IEventPayload payload)
    {
        _logger.LogInformation("Hello from TestReducer!");
        return new ValueTask<LiveState>(state);
    }
}