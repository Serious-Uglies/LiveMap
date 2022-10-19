using DasCleverle.DcsExport.State.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DasCleverle.DcsExport.LiveMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StateController : Controller
{
    private readonly ILiveStateStore _store;

    public StateController(ILiveStateStore store)
    {
        _store = store;
    }

    [HttpGet]
    public LiveState GetState() => _store.GetState();
}