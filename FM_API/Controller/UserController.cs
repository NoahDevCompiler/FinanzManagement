using FM_API.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace FM_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public void Get() {
            if (Auth.ProtectedHandler(Response, Request, out string _username)) {

                int? _id = DBManager.GetUserIDFromUsername(_username);
                if (_id == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("UserController: Failed to fetch UserID");
                    return;
                }

                User? _user = DBManager.GetUser((int)_id);
                if (_user == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("UserController: Failed to fetch User");
                    return;
                }

                Responses.JsonOk(Response, _user);
            }
        }


    }
}
