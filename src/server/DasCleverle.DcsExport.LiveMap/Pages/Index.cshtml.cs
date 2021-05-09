using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DasCleverle.DcsExport.LiveMap.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public string MapboxToken { get; private set; }

        public string MapboxStyle { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            MapboxToken = _configuration["MapboxToken"];
            MapboxStyle = _configuration["MapboxStyle"];
        }
    }
}
