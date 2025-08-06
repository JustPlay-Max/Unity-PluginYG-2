using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class GetEnvirData : MonoBehaviour
    {
        public Text dataText;

        private void OnEnable()
        {
            YG2.onGetSDKData += OnGetData;
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= OnGetData;
        }

        private void Start()
        {
            OnGetData();
        }

        private void OnGetData()
        {
            dataText.text = ShowParam("domain", YG2.envir.domain) +
                ShowParam("device type", YG2.envir.deviceType) +
                ShowParam("is mobile", YG2.envir.isMobile.ToString()) +
                ShowParam("is desktop", YG2.envir.isDesktop.ToString()) +
                ShowParam("is tablet", YG2.envir.isTablet.ToString()) +
                ShowParam("app id", YG2.envir.appID) +
                ShowParam("language", YG2.envir.language) +
                ShowParam("browser lang", YG2.envir.browserLang) +
                ShowParam("browser", YG2.envir.browser) +
                ShowParam("platform", YG2.envir.platform) +
                ShowParam("payload", YG2.envir.payload);
        }

        private string ShowParam(string name, string param)
        {
            if (param == null || param == string.Empty)
                param = "---";

            string color1 = "#613c25";
            string color2 = "#cd5200";
            string final = @$"<color={color1}>{name}</color> <color={color2}>= ""</color>";
            final += param;
            final += @$"<color={color2}>""</color>";
            final += "\n";
            return final;
        }
    }
}
