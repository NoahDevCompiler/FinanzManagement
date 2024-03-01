using System.ComponentModel.DataAnnotations;

namespace FM_API.Backend
{
    public class User
    {
        public int User_id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public float? Capital { get; set; }
        public DateTime? CapitalDate { get; set; }

        public User(int user_id, string username, string email, float? capital, DateTime? capitalDate) {
            User_id = user_id;
            Username = username;
            Email = email;
            Capital = capital;
            CapitalDate = capitalDate;
        }
    }

    public class Transaction {

        [Required] public int Id { get; set; }
        [Required] public string TransactionName { get; set; }
        [Required] public float TransactionValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Transaction(int id, string transactionName, float transactionValue, DateTime? startDate, DateTime? endDate) {
            Id = id;
            TransactionName = transactionName;
            TransactionValue = transactionValue;
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    public class Saving {
        [Required] public int Id { get; set; }
        [Required] public string SavingName { get; set;}
        [Required] public float SavingGoal { get; set; }
        [Required] public float MonthlyAmount { get; set; }
        [Required] public DateTime StartDate { get; set; }

        public Saving(int id, string savingName, float savingGoal, float monthlyAmount, DateTime startDate) {
            Id = id;
            SavingName = savingName;
            SavingGoal = savingGoal;
            MonthlyAmount = monthlyAmount;
            StartDate = startDate;
        }
    }
}
