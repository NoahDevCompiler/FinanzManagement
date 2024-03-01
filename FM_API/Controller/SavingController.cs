using FM_API.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FM_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingController : ControllerBase
    {

        [HttpPost]
        public void Post(Saving _request) {
            if (Auth.ProtectedHandler(Response, Request, out string _username)) {

                int? _id = DBManager.GetUserIDFromUsername(_username);
                if (_id == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("UserController: Failed to fetch UserID");
                    return;
                }

                // ---- Checks ----
                var _errors = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(_request.SavingName)) {
                    _errors.Add("savingName", "Der Transaction Name muss definiert werden!");
                }

                if (_request.SavingGoal <= 0) { 
                    _errors.Add("savingGoal", "Sparziel darf nich null oder negativ sein!");
                }

                if (_request.MonthlyAmount <= 0) {
                    _errors.Add("monthlyAmount", "Einzahlungen dürfen nich null oder negativ sein!");
                }

                if (_request.SavingName.Length > 64) {
                    _errors.Add("savingName", "Der Transaction Name ist zu lang!");
                }

                if (_request.StartDate > DateTime.Now) {
                    _errors.Add("startDate", "Das Datum darf nich in der Zukunft ligen!");
                }

                if (_errors.Count != 0) {
                    Responses.ValidationError(Response, "Transaction Validation Error", _errors);
                    return;
                }

                // ---- DB ----

                DBManager.CreateSaving((int)_id, _request);
                Responses.JsonOk(Response, "Successfully created saving");
            }
        }

        [HttpDelete]
        public void Delete(DeleteSavingRequest _request) {
            if (Auth.ProtectedHandler(Response, Request, out string _username)) {

                int? _id = DBManager.GetUserIDFromUsername(_username);
                if (_id == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("UserController: Failed to fetch UserID");
                    return;
                }

                bool? _check = DBManager.CheckSavingOwner((int)_id, _request.SavingId);
                if (_check == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("LoginController: Deletion Failed");
                    return;
                }

                if (!(bool)_check) {
                    Responses.ServerError(Response, "User dose not own saving");
                }

                DBManager.DeleteSaving(_request.SavingId);
                Responses.JsonOk(Response, "Successfully deleted saving");
            }
        }


        public class DeleteSavingRequest
        {
            [Required] public int SavingId { get; set; }

            public DeleteSavingRequest(int savingId) {
                SavingId = savingId;
            }
        }

    }
}
