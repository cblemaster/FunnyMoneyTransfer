namespace FunnyMoneyTransfer.Data;
public partial class Transfer
{
    public TransferTypeEnum TransferTypeEnum => (TransferTypeEnum)TransferTypeId;

    public TransferStatusEnum TransferStatusEnum => (TransferStatusEnum)TransferStatusId;
}
