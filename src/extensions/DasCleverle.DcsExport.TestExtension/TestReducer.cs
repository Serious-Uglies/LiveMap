using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.TestExtension;

public class TestReducer : Reducer
{
    private ILogger<TestReducer> _logger;

    public TestReducer(ILogger<TestReducer> logger)
    {
        _logger = logger;
    }

    public override IEnumerable<string> EventTypes { get; } = Reducer.CatchAll;

    protected override LiveState Reduce(LiveState state, IExportEvent exportEvent)
    {
        _logger.LogInformation("Hello from Test Reducer!");
        return state;
    }

}