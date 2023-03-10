namespace FunnyMoneyTransfer.Data;

public partial class Transfer
{
    public int Id { get; set; }

    public int TransferTypeId { get; set; }

    public int TransferStatusId { get; set; }

    public int AccountIdFrom { get; set; }

    public int AccountIdTo { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Account AccountIdFromNavigation { get; set; } = null!;

    public virtual Account AccountIdToNavigation { get; set; } = null!;

    public virtual TransferStatus TransferStatus { get; set; } = null!;

    public virtual TransferType TransferType { get; set; } = null!;
}
