using FM_API.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FM_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {  

        // TODO: Create new token when old one is outdated.
        // This is really really bad and stupid

        [HttpGet]
        public void Get() {
            if (Auth.ProtectedHandler(Response, Request, out string _username)) {
                string _token = (string)Request.Headers["Authorization"];
                _token = _token.Substring("Bearer ".Length);

                Responses.JsonOk(Response, new LoginController.LoginResponse(_token));
            }
        }


    }
}
