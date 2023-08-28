using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace AuthServer.API.Controllers.Common
{
    // API Response için ortak yapı
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult ActionResult<T>(Response<T> response)where T: class
        {
            // Response içerisinde hangi HttpStatusCode gelirse onun metodu return edilir
            // 200 gelirse -> Ok, 404 gelirse -> NotFound...
            return new ObjectResult(response)
            {
                StatusCode = response.HttpStatusCode
            };
        }
    }
}
