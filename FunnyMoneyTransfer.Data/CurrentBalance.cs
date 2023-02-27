namespace FunnyMoneyTransfer.Data
{
    public class CurrentBalance
    {
        public static decimal CalculateBalance(decimal startingBalance, List<Transfer> transfersTo, List<Transfer> transfersFrom)
        {
            decimal currentBalance = 0m;
            currentBalance += startingBalance;
            currentBalance += transfersTo.Sum(t => t.Amount);
            currentBalance -= transfersFrom.Sum(t => t.Amount);
            return currentBalance;
        }
    }
}
