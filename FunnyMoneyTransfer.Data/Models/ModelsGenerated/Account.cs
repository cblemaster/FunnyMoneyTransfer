namespace FunnyMoneyTransfer.Data.Models
{
    public partial class Account
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual StartingBalance? StartingBalance { get; set; }

        public virtual ICollection<Transfer> TransferAccountIdFromNavigations { get; } = new List<Transfer>();

        public virtual ICollection<Transfer> TransferAccountIdToNavigations { get; } = new List<Transfer>();

        public virtual User User { get; set; } = null!;
    }

}

