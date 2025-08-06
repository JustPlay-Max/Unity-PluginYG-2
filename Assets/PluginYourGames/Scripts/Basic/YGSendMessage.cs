using UnityEngine;

namespace YG.Insides
{
    public partial class YGSendMessage : MonoBehaviour
    {
        private void Start()
        {
            YG2.StartInit();
        }

        public void GetDataInvoke()
        {
            YG2.GetDataInvoke();
        }
    }
}