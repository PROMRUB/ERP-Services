using ERP.Services.API.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        [HttpGet]
        [Route("HealthCheck")]
        [MapToApiVersion("1")]
        public IActionResult HealthCheck()
        {
            try
            {
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
