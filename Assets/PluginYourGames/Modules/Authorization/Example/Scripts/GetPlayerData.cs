using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class GetPlayerData : MonoBehaviour
    {
        public ImageLoadYG imageLoad;
        public Text playerDataText;

        private void OnEnable()
        {
            YG2.onGetSDKData += DebugData;

            if (YG2.isSDKEnabled)
                DebugData();
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= DebugData;
        }

        private void DebugData()
        {
            string playerId = YG2.player.id;
            if (playerId.Length > 10)
                playerId = playerId.Remove(10) + "...";

            playerDataText.text = ShowParam("auth", YG2.player.auth.ToString()) +
                ShowParam("name", YG2.player.name) +
                ShowParam("id", playerId);

            if (imageLoad != null && YG2.player.auth)
                imageLoad.Load(YG2.player.photo);
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