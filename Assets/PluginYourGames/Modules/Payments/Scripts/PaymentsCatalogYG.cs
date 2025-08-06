using UnityEngine;
using YG.Insides;

namespace YG
{
    public class PaymentsCatalogYG : MonoBehaviour
    {
#if UNITY_EDITOR
        [Tooltip(Langs.t_rootSpawnPurchases)]
#endif
        public Transform rootSpawnPurchases;
#if UNITY_EDITOR
        [Tooltip(Langs.t_purchasePrefab)]
#endif
        public GameObject purchasePrefab;

        private void OnEnable() => YG2.onGetPayments += UpdatePurchasesList;
        private void OnDisable() => YG2.onGetPayments -= UpdatePurchasesList;
        private void Start() => UpdatePurchasesList();

        public void UpdatePurchasesList()
        {
            // Clear catalog
            int childCount = rootSpawnPurchases.childCount;
            for (int i = childCount - 1; i >= 0; i--)
                Destroy(rootSpawnPurchases.GetChild(i).gameObject);

            // Spawn catalog
            for (int i = 0; i < YG2.purchases.Length; i++)
            {
                GameObject purchaseObj = Instantiate(purchasePrefab, rootSpawnPurchases);
                purchaseObj.GetComponent<PurchaseYG>().id = YG2.purchases[i].id;
            }
        }

        public void BuyPurchase(string id) => YG2.BuyPayments(id);
    }
}