namespace FunnyMoneyTransfer.Data.Models
{
    public partial class TransferType
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public virtual ICollection<Transfer> Transfers { get; } = new List<Transfer>();
    }

}

