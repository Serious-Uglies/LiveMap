using DasCleverle.DcsExport.LiveMap.State;
using Microsoft.AspNetCore.Mvc;

namespace DasCleverle.DcsExport.LiveMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : Controller
    {
        private readonly ILiveState _state;

        public StateController(ILiveState state)
        {
            _state = state;
        }

        [HttpGet]
        public ILiveState GetState() => _state;
    }
}