using FunnyMoneyTransfer.Data;
using FunnyMoneyTransfer.UI.WPF.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;

namespace FunnyMoneyTransfer.UI.WPF.Account
{
    public class BalanceViewModel : INotifyPropertyChanged
    {
        #region ctor
        public BalanceViewModel()
        {
            LoggedInUserViewModel loggedInUserContext = (((((App.Current.MainWindow as MainWindow)!).loggedInUserView).DataContext) as LoggedInUserViewModel)!;

            Data.User loggedInUser = loggedInUserContext.LoggedInUser;
            
            if (loggedInUserContext != null && loggedInUser is Data.User && loggedInUser.Account is Data.Account)
            {
                this.CalculatedBalance = loggedInUserContext.LoggedInUser.Account!.CalculatedBalance;                
            }
            this.AsOfDate = DateTime.Now.ToString();
        }
        #endregion

        #region fields
        private readonly FunnyMoneyTransferContext _db = new();
        private decimal _calculatedBalance;
        private string _asOfDate = null!;
        #endregion

        #region events
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        #endregion

        #region properties       
        public decimal CalculatedBalance
        {
            get => _calculatedBalance;
            set
            {
                if (value != _calculatedBalance)
                {
                    _calculatedBalance = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(CalculatedBalance)));
                    this.AsOfDate = DateTime.Now.ToString();
                }
            }
        }

        public string AsOfDate  //TODO: Need this to refresh when balance is recalculated, not sure if it is working this way...
        {
            get => _asOfDate;
            set
            {
                if (value != _asOfDate)
                {
                    _asOfDate = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(AsOfDate)));
                }
            }
        }
        #endregion
    }
}
