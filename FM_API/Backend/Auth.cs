using FM_API.Controller;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FM_API.Backend
{
    public class Auth
    {
        public static string? GenerateJSONWebToken(string _username) {
            try {
                var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.current.SecretKey));
                var _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

                var _claims = new[] {
                    new Claim("username", _username),
                };

                var token = new JwtSecurityToken(
                    Config.current.PageURL,
                    Config.current.PageURL,
                    _claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: _credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception _e) {
                Console.WriteLine("GenerateJSONWebToken: " + _e.Message);
                return null;
            }
        }

        public static bool VerifyToken(string _srtToken, out string _username) {
            _username = "";

            var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.current.SecretKey));

            try {
                var _claims = new JwtSecurityTokenHandler().ValidateToken(
                    _srtToken,
                    new TokenValidationParameters
                    {
                        IssuerSigningKey = _securityKey,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Config.current.PageURL,
                        ValidAudience = Config.current.PageURL,
                    },
                    out var _token); ;

                string? _usernameClaim = _claims.FindFirstValue("username");
                if (_usernameClaim == null) throw new Exception("Claim is missing");
    
                _username = _usernameClaim;
                return true;
            }
            catch (Exception _e) {
                Console.WriteLine("VerifyToken: " + _e);
                return false;
            }
        }

        public static bool ProtectedHandler(HttpResponse w, HttpRequest r, out string _username) {
            _username = "";

            if (!r.Headers.ContainsKey("Authorization")) {
                Responses.Unauthorized(w, "Missing authorization header");
                return false;
            }

            string _token = (string)r.Headers["Authorization"];
            _token = _token.Substring("Bearer ".Length);

            string _usernameClaim;
            if (!VerifyToken(_token, out _usernameClaim)) {
                Responses.Unauthorized(w, "Invalid token");
                return false;
            };

            _username = _usernameClaim;
            return true;
        }


    }
}
