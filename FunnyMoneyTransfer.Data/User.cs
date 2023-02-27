namespace FunnyMoneyTransfer.Data;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual Account? Account { get; set; }
}
