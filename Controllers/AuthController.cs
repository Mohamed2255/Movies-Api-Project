using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Movies_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuth auth;
        public AuthController(IAuth _auth)
        {
            auth=_auth;
        }
        [HttpPost("Register")]
        public async Task <IActionResult> register([FromBody]Register register)
        {
            if (!ModelState.IsValid)
            {
                return  BadRequest(ModelState);
            }
            var result = await  auth.Register(register);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login([FromBody] Loginmodel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await auth.Login(login);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(new {result.Username,result.Roles,result.Token});
        }
        [HttpPost("AddToRole")]
        public async Task<IActionResult> AddRole([FromBody] Rolemodel rolemodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await auth.AddRole(rolemodel);
            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }
            return Ok(rolemodel);
        }
    }
}
