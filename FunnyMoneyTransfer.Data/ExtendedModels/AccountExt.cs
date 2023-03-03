namespace FunnyMoneyTransfer.Data
{
    public partial class Account
    {
        public decimal CalculatedBalance
        {
            get
            {
                if (this.StartingBalance != null)
                    return this.StartingBalance.Amount
                        + TransferAccountIdToNavigations.Sum(t => t.Amount)
                        - TransferAccountIdFromNavigations.Sum(t => t.Amount);
                return 0m;
            }
        }
    }
}
