using FM_API.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FM_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalSavingsController : ControllerBase
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

                var _savings = DBManager.GetSavings((int)_id);
                float _totalSavings = 0;

                foreach (var t in _savings) {
                    
                    float _months = Util.GetDateMonthDiffrence((DateTime)t.StartDate, DateTime.Now);

                    _totalSavings += _months * t.MonthlyAmount;
                }

                Responses.JsonOk(Response, new TotalSavedResponse(_totalSavings));
            }
        }

        public class TotalSavedResponse
        {
            [Required] public float TotalSaved { get; set; }

            public TotalSavedResponse(float totalSaved) {
                TotalSaved = totalSaved;
            }
        }

    }
}
