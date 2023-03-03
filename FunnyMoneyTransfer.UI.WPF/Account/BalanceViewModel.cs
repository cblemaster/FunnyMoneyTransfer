using FunnyMoneyTransfer.Data;
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
            MainWindowViewModel mainContext = (App.Current.MainWindow.DataContext as MainWindowViewModel)!;

            if (mainContext != null && mainContext.LoggedInUser != null)
            {
                Data.Account a = this._db.Accounts
                                        .Include(n => n.StartingBalance)
                                        .Include(t => t.TransferAccountIdToNavigations)
                                        .Include(t => t.TransferAccountIdFromNavigations)
                                        .FirstOrDefault(a => a.UserId == mainContext.LoggedInUser.Id)!;
                if (a != null)
                {
                    this.CalculatedBalance = a.CalculatedBalance();
                }
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
