using UnityEngine;

namespace YG.Example
{
    public class HideIfnotTV : MonoBehaviour
    {
        private void OnEnable() => YG2.onGetSDKData += GetData;
        private void OnDisable() => YG2.onGetSDKData -= GetData;

        private void Awake()
        {
            if (YG2.isSDKEnabled)
                GetData();
        }

        private void GetData()
        {
            if (!YG2.envir.isTV)
                gameObject.SetActive(false);
        }
    }
}