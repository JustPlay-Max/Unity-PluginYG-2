namespace YG
{
    public partial interface IPlatformsYG2
    {
        void InitPayments() { }
        void BuyPayments(string id) { }
        void ConsumePurchases(bool onPurchaseSuccess) { }
        void ConsumePurchaseByID(string id, bool onPurchaseSuccess) { }
    }
}
