using FunnyMoneyTransfer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyMoneyTransfer.UI.WPF.Account
{
    public class BalanceViewModel : INotifyPropertyChanged
    {
        #region ctor
        public BalanceViewModel()
        {
            Data.Account a = this._db.Accounts
                                        .Include(n => n.StartingBalance)
                                        .Include(t => t.TransferAccountIdToNavigations)
                                        .Include(t => t.TransferAccountIdFromNavigations)
                                        .FirstOrDefault(a => a.UserId == 2)!;
            if (a != null)
            {
                this.CalculatedBalance = a.CalculatedBalance();
                this.AsOfDate = DateTime.Now.ToString();
            }
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
            get =>_calculatedBalance;
            set
            {
                if (value != _calculatedBalance)
                {
                    _calculatedBalance = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(CalculatedBalance)));
                }                
            }
        }

        public string AsOfDate
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
