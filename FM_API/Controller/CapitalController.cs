using FM_API.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FM_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapitalController : ControllerBase
    {
        [HttpPost]
        public void Post(CapitalRequest _request) {
            if (Auth.ProtectedHandler(Response, Request, out string _username)) {

                int? _id = DBManager.GetUserIDFromUsername(_username);
                if (_id == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("UserController: Failed to fetch UserID");
                    return;
                }

                if (_request.CapitalDate > DateTime.Now) {
                    var _errors = new Dictionary<string, string>();
                    _errors.Add("capitaldate", "Datum ist in der Zukunft!");
                    Responses.ValidationError(Response, "Capital Validation Error", _errors);
                    return;
                }

                DBManager.UpdateCapital((int)_id, _request.Capital, _request.CapitalDate);

                Responses.JsonOk(Response, "Successfully updated capital");
            }
        }

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

                var _transactions = DBManager.GetTransactions((int)_id);
                float _totalTransaction = 0;

                foreach (var _t in _transactions) {
                    if (_t.StartDate == null) {
                        _totalTransaction += _t.TransactionValue;
                        continue;
                    }

                    DateTime _startDate = (DateTime)_t.StartDate;
                    if (_t.StartDate < _user.CapitalDate) {
                        _startDate = (DateTime)_user.CapitalDate;
                    }

                    DateTime _endDate = DateTime.Now;
                    if (_t.EndDate != null) {
                        _endDate = (DateTime)_t.EndDate;
                    }

                    float _months = Util.GetDateMonthDiffrence(_startDate, _endDate);

                    _totalTransaction += _months * _t.TransactionValue;
                }

                var _savings = DBManager.GetSavings((int)_id);
                float _totalSavings = 0;

                foreach (var _s in _savings) {
                    DateTime _startDate = (DateTime)_s.StartDate;
                    if (_s.StartDate < _user.CapitalDate) {
                        _startDate = (DateTime)_user.CapitalDate;
                    }

                    float _months = Util.GetDateMonthDiffrence(_startDate, DateTime.Now);

                    _totalSavings += _months * _s.MonthlyAmount;
                }

                Responses.JsonOk(Response, new CapitalResponse((float)_user.Capital + _totalTransaction - _totalSavings));
            }
        }

        public class CapitalResponse
        {
            [Required] public float Capital { get; set; }

            public CapitalResponse(float capital) {
                Capital = capital;
            }
        }


        public class CapitalRequest {
            [Required] public float Capital { get; set; }
            [Required] public DateTime CapitalDate { get; set; }
        }

    }
}
