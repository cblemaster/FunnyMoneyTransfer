namespace FunnyMoneyTransfer.Data;

public partial class StartingBalance
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Account Account { get; set; } = null!;
}
