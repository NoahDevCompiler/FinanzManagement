using FM_API.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FM_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase 
    {
        [HttpPost]
        public void Post(LoginRequest _request) {
            var _errors = new Dictionary<string, string>();

            int? _id = DBManager.GetUserIDFromUsername(_request.Username);
            if (_id == null) {
                _errors.Add("username", "Nutzer wurde nicht gefunden!");
                Responses.ValidationError(Response, "Login Validation Error", _errors);
                return;
            }

            // TODO: Password Encryption
            bool? _check = DBManager.CheckUserPassword((int)_id, _request.Password);
            if (_check == null) {
                Responses.InternalServerError(Response);
                Console.WriteLine("LoginController: Password Chech Failed");
                return;
            }

            if (!(bool)_check) {
                _errors.Add("password", "Passwort ist falsch!");
            }

            if (_errors.Count != 0) {
                Responses.ValidationError(Response, "Login Validation Error", _errors);
                return;
            }

            string? _token = Auth.GenerateJSONWebToken(_request.Username);
            if (_token == null) {
                Responses.InternalServerError(Response);
                Console.WriteLine("LoginController: Token Generation Failed");
                return;
            }

            Responses.JsonOk(Response, new LoginResponse(_token));
        }

        public class LoginRequest {
            [Required] public string Username { get; set; }
            [Required] public string Password { get; set; }

            public LoginRequest(string username, string password) {
                Username = username;
                Password = password;
            }
        }

        public class LoginResponse { 
            [Required] public string Token { get; set; }
            public LoginResponse(string token) {
                Token = token;
            }
        }


    }


}
