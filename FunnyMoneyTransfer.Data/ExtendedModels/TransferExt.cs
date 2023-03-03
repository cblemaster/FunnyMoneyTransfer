using FunnyMoneyTransfer.Data.ExtendedModels;

namespace FunnyMoneyTransfer.Data
{
    public partial class Transfer
    {
        public TransferTypeEnum TransferTypeEnum => (TransferTypeEnum)this.TransferTypeId;

        public TransferStatusEnum TransferStatusEnum => (TransferStatusEnum)this.TransferStatusId;
    }
}
