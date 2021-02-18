using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jwt_WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomClaimController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public string Get()
        {
            var customClaim = User.FindFirst("custom-claim");
            
            return $"The custom claim value is '{customClaim.Value}'";
        }
    }
}
