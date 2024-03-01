using FM_API.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FM_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        public void Post(Transaction _request) {
            if (Auth.ProtectedHandler(Response, Request, out string _username)) {

                int? _id = DBManager.GetUserIDFromUsername(_username);
                if (_id == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("UserController: Failed to fetch UserID");
                    return;
                }

                // ---- Checks ----
                var _errors = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(_request.TransactionName)) {
                    _errors.Add("transactionName", "Der Transaction Name muss definiert werden!");
                }

                if (_request.TransactionName.Length > 64) {
                    _errors.Add("transactionName", "Der Transaction Name ist zu lang!");
                }

                if (((int)_request.TransactionValue).ToString().Length > 10) { 
                    _errors.Add("transaction", "Der Transaction Wert ist zu hoch!");
                }

                if (_request.StartDate != null) {
                    if (_request.StartDate > DateTime.Now) {
                        _errors.Add("startDate", "Das Datum darf nich in der Zukunft ligen!");
                    }
                }

                if (_request.EndDate != null) {
                    if (_request.EndDate > DateTime.Now) {
                        _errors.Add("endDate", "Das Datum darf nich in der Zukunft ligen!");
                    }
                }

                if (_errors.Count != 0) {
                    Responses.ValidationError(Response, "Transaction Validation Error", _errors);
                    return;
                }

                // ---- DB ----

                DBManager.CreateTransaction((int)_id, _request);
                Responses.JsonOk(Response, "Successfully created transaction");
            }
        }

        [HttpDelete]
        public void Delete(DeleteTransactionRequest _request) {
            if (Auth.ProtectedHandler(Response, Request, out string _username)) {

                int? _id = DBManager.GetUserIDFromUsername(_username);
                if (_id == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("UserController: Failed to fetch UserID");
                    return;
                }

                bool? _check = DBManager.CheckTransactionOwner((int)_id, _request.TransactionId);
                if (_check == null) {
                    Responses.InternalServerError(Response);
                    Console.WriteLine("LoginController: Deletion Failed");
                    return;
                }

                if (!(bool)_check) {
                    Responses.ServerError(Response, "User dose not own transaction");
                }

                DBManager.DeleteTransaction(_request.TransactionId);
                Responses.JsonOk(Response, "Successfully deleted transaction");
            }
        }


        public class DeleteTransactionRequest {
            [Required] public int TransactionId { get; set; }

            public DeleteTransactionRequest(int transactionId) {
                TransactionId = transactionId;
            }
        }

    }
}
