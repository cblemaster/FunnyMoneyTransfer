using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace FunnyMoneyTransfer.UI.WPF.Transfer
{
    public class SendTransferViewModel : INotifyPropertyChanged
    {
        #region ctor
        public SendTransferViewModel()
        {

        }
        #endregion

        #region fields
        private decimal _amount;
        #endregion

        #region events
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        #endregion

        #region properties
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }
        #endregion        
    }
}
