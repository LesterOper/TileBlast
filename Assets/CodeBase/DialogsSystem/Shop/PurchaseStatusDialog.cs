namespace CodeBase.DialogsSystem.Shop
{
    public class PurchaseStatusDialog : DialogBase
    {
        protected override void Confirm()
        {
            CloseDialog();
            base.Confirm();
        }
    }
}