using FunnyMoneyTransfer.Data;

namespace FunnyMoneyTransfer.UI.WPF.User
{
    public class UserValidationResult
    {
        public bool IsValid { get; set; }
        public bool ShowValidationErrorInUI { get; set; }
        public string ErrorMessage { get; set; } = null!;        
    }
}
