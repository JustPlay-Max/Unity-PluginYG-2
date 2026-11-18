namespace YG
{
    public partial interface IPlatformsYG2
    {
        void InitPayments() { }
        void BuyPayments(string id) { }
        void ConsumePurchases(bool onPurchaseSuccess) { }
        void ConsumePurchaseByID(string id, bool onPurchaseSuccess) { }
        void GetPendingPurchases()
        {
            YG2.onGetPendingPurchasesFailed?.Invoke("Pending purchases API is not supported on this platform.");
        }
        void ConsumePurchaseByToken(string purchaseToken)
        {
            YG2.onPurchaseConsumeFailed?.Invoke(purchaseToken);
        }
    }
}
