namespace FunnyMoneyTransfer.Data;
public partial class Account
{
    public decimal CalculatedBalance
    {
        get
        {
            if (StartingBalance == null)
                return 0m;
            return StartingBalance.Amount
                + TransferAccountIdToNavigations.Sum(t => t.Amount)
                - TransferAccountIdFromNavigations.Sum(t => t.Amount);
        }
    }
}
