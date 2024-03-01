using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;
using System.Data;

namespace FM_API.Backend
{
    public class DBManager 
    {
        public static MySqlConnection DB;
        private static bool isStreamOpen = true;

        public static void StartDataBaseConnection() {
            string _constr = 
                $"server={Config.current.DB_host};" +
                $"userid={Config.current.DB_username};" +
                $"password={Config.current.DB_password};" +
                $"database={Config.current.DB_database};";

            DB = new MySqlConnection(_constr);
            DB.Open();
            Console.WriteLine($"MySql version: {DB.ServerVersion}");

        }

        #region User

        public static User? GetUser(int _id) {
            OpenStream();

            string _sql = "SELECT tbl_users.user_id, tbl_users.username, tbl_users.email, tbl_users.capital, tbl_users.capitalDate FROM tbl_users WHERE tbl_users.user_id = @user_id;";

            User? _user = null;

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@user_id", _id);

                using (MySqlDataReader _reader = _command.ExecuteReader()) {
                    if (_reader.Read()) {

                        if (_reader.IsDBNull("capital") || _reader.IsDBNull("capitalDate")) {
                            _user = new User(
                                _reader.GetInt32("user_id"),
                                _reader.GetString("username"),
                                _reader.GetString("email"),
                                null, null
                            );
                        }
                        else {
                            _user = new User(
                                _reader.GetInt32("user_id"),
                                _reader.GetString("username"),
                                _reader.GetString("email"),
                                _reader.GetFloat("capital"),
                                _reader.GetDateTime("capitalDate")
                            );
                        }
                    }
                }
            }

            CloseStream();

            return _user;
        }

        public static void CreateUser(string _username, string _password, string _email) {
            OpenStream();

            string _sql = "INSERT INTO tbl_users(tbl_users.username, tbl_users.password, tbl_users.email) VALUES (@username, @password, @email)";
            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@username", _username);
                _command.Parameters.AddWithValue("@password", _password);
                _command.Parameters.AddWithValue("@email", _email);

                _command.ExecuteNonQuery();
            }

            CloseStream();
        }

        public static int? GetUserIDFromUsername(string _username) {
            OpenStream();

            string _sql = "SELECT tbl_users.user_id FROM tbl_users WHERE username = @username";

            int? _userID = null;

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@username", _username);

                using (MySqlDataReader _reader = _command.ExecuteReader()) {
                    if (_reader.Read()) {
                        _userID = _reader.GetInt32("user_id");
                    }
                }
            }

            CloseStream();

            return _userID;
        }

        public static bool? CheckUserPassword(int _id, string _password) {
            OpenStream();

            string _sql = "SELECT (tbl_users.password = @password ) As 'check' FROM tbl_users WHERE tbl_users.user_id = @user_id;";

            bool? _check = null;

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@user_id", _id);
                _command.Parameters.AddWithValue("@password", _password);

                using (MySqlDataReader _reader = _command.ExecuteReader()) {
                    if (_reader.Read()) {
                        _check = _reader.GetBoolean("check");
                    }
                }
            }

            CloseStream();

            return _check;
        }

        public static void UpdateCapital(int _id, float _capital, DateTime _capitalDate) {
            OpenStream();

            string _sql = "UPDATE `fm_db`.`tbl_users` SET `capital` = @capital, `capitalDate` = @capitalDate WHERE `user_id` = @user_id;";
            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@user_id", _id);
                _command.Parameters.AddWithValue("@capital", _capital);
                _command.Parameters.AddWithValue("@capitalDate", _capitalDate);

                _command.ExecuteNonQuery();
            }

            CloseStream();
        }

        #endregion

        #region Transaction
        public static void CreateTransaction(int _id, Transaction _transaction) {
            OpenStream();

            string _sql = "INSERT INTO `fm_db`.`tbl_transactions`(`name`,`transaction`,`startDate`,`endDate`,`id_user`) " +
                "VALUES (@name, @transaction, @startDate, @endDate, @userid);";

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@name", _transaction.TransactionName);
                _command.Parameters.AddWithValue("@transaction", _transaction.TransactionValue);
                _command.Parameters.AddWithValue("@startDate", _transaction.StartDate);
                _command.Parameters.AddWithValue("@endDate", _transaction.EndDate);
                _command.Parameters.AddWithValue("@userid", _id);

                _command.ExecuteNonQuery();
            }

            CloseStream();
        }

        public static void DeleteTransaction(int _transactionId) {
            OpenStream();

            string _sql = "DELETE FROM `fm_db`.`tbl_transactions` WHERE `tbl_transactions`.`transaction_id` = @id;";

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@id", _transactionId);

                _command.ExecuteNonQuery();
            }

            CloseStream();
        }

        public static bool? CheckTransactionOwner(int _id, int _transactionId) {
            OpenStream();

            string _sql = "SELECT (tbl_transactions.id_user = @user_id) As 'check' FROM tbl_transactions WHERE tbl_transactions.transaction_id = @transaction_id;";

            bool? _check = null;

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@user_id", _id);
                _command.Parameters.AddWithValue("@transaction_id", _transactionId);

                using (MySqlDataReader _reader = _command.ExecuteReader()) {
                    if (_reader.Read()) {
                        _check = _reader.GetBoolean("check");
                    }
                }
            }

            CloseStream();

            return _check;
        }

        public static List<Transaction> GetTransactions(int _id) {
            OpenStream();

            string _sql = "SELECT `tbl_transactions`.`transaction_id`, `tbl_transactions`.`name`, `tbl_transactions`.`transaction`, `tbl_transactions`.`startDate`, `tbl_transactions`.`endDate` FROM `fm_db`.`tbl_transactions` " +
                "WHERE`tbl_transactions`.`id_user` = @user_id;";

            List<Transaction> _transactions = new List<Transaction>();  

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@user_id", _id);

                using (MySqlDataReader _reader = _command.ExecuteReader()) {
                    while (_reader.Read()) {

                        DateTime? _startDate = null;
                        DateTime? _endDate = null;

                        if (!_reader.IsDBNull("startDate")) _startDate = _reader.GetDateTime("startDate");
                        if (!_reader.IsDBNull("endDate")) _startDate = _reader.GetDateTime("endDate");

                        _transactions.Add(new Transaction(
                            _reader.GetInt32("transaction_id"),
                            _reader.GetString("name"),
                            _reader.GetFloat("transaction"),
                            _startDate,
                            _endDate
                        )) ;
                    }
                }
            }

            CloseStream();

            return _transactions;
        }

        #endregion

        #region Saving

        public static void CreateSaving(int _id, Saving _saving) {
            OpenStream();

            string _sql = "INSERT INTO `fm_db`.`tbl_saving`(`savingName`,`savingGoal`,`monthlyAmount`,`startDate`,`id_user`) " +
                "VALUES (@savingName, @savingGoal, @monthlyAmount, @startDate, @id_user);";

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@savingName", _saving.SavingName);
                _command.Parameters.AddWithValue("@savingGoal", _saving.SavingGoal);
                _command.Parameters.AddWithValue("@monthlyAmount", _saving.MonthlyAmount);
                _command.Parameters.AddWithValue("@startDate", _saving.StartDate);
                _command.Parameters.AddWithValue("@id_user", _id);

                _command.ExecuteNonQuery();
            }

            CloseStream();
        }

        public static void DeleteSaving(int _savingId) {
            OpenStream();

            string _sql = "DELETE FROM `fm_db`.`tbl_saving` WHERE `tbl_saving`.`saving_id` = @id;";

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@id", _savingId);

                _command.ExecuteNonQuery();
            }

            CloseStream();
        }

        public static bool? CheckSavingOwner(int _id, int _savingId) {
            OpenStream();

            string _sql = "SELECT (tbl_saving.id_user = @user_id) As 'check' FROM tbl_saving WHERE tbl_saving.saving_id = @saving_id;";

            bool? _check = null;

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@user_id", _id);
                _command.Parameters.AddWithValue("@saving_id", _savingId);

                using (MySqlDataReader _reader = _command.ExecuteReader()) {
                    if (_reader.Read()) {
                        _check = _reader.GetBoolean("check");
                    }
                }
            }

            CloseStream();

            return _check;
        }

        public static List<Saving> GetSavings(int _id) {
            OpenStream();

            string _sql = "SELECT * FROM `fm_db`.`tbl_saving` " +
                "WHERE `tbl_saving`.`id_user` = @user_id;";

            List<Saving> _savings = new List<Saving>();

            using (MySqlCommand _command = new MySqlCommand(_sql, DB)) {
                _command.Parameters.AddWithValue("@user_id", _id);

                using (MySqlDataReader _reader = _command.ExecuteReader()) {
                    while (_reader.Read()) {

                        _savings.Add(new Saving(
                            _reader.GetInt32("saving_id"),
                            _reader.GetString("savingName"),
                            _reader.GetFloat("savingGoal"),
                            _reader.GetFloat("monthlyAmount"),
                            _reader.GetDateTime("startDate")
                        ));
                    }
                }
            }

            CloseStream();

            return _savings;
        }

        #endregion

        private static void CloseStream() {
            isStreamOpen = true;
        }

        private static void OpenStream() {
            while (!isStreamOpen) {
                Thread.Sleep(30);
            }
            isStreamOpen = false;
        }

    }
}
