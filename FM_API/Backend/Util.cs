namespace FM_API.Backend
{
    public class Util
    {


        public static float GetDateMonthDiffrence(DateTime _startDate, DateTime _endDate) {
            return Math.Abs(((_startDate.Year - _endDate.Year) * 12) + _startDate.Month - _endDate.Month);
        }
    }
}
