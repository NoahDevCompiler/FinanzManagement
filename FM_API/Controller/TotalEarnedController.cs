using FM_API.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FM_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalEarnedController : ControllerBase
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

                var _transactions = DBManager.GetTransactions((int)_id);
                float _totalEarned = 0;

                foreach (var t in _transactions) {
                    if (t.TransactionValue < 0) {
                        continue;
                    }

                    if (t.StartDate == null) {
                        _totalEarned += t.TransactionValue;
                        continue;
                    }

                    DateTime _endDate = DateTime.Now;
                    if (t.EndDate != null) {
                        _endDate = (DateTime)t.EndDate;
                    }

                    float _months = Util.GetDateMonthDiffrence((DateTime)t.StartDate, _endDate);

                    _totalEarned += _months * t.TransactionValue;
                }

                Responses.JsonOk(Response, new TotalEarnedResponse(_totalEarned));
            }
        }

        public class TotalEarnedResponse
        {
            [Required] public float TotalEarned { get; set; }

            public TotalEarnedResponse(float totalEarned) {
                TotalEarned = totalEarned;
            }
        }


    }
}
