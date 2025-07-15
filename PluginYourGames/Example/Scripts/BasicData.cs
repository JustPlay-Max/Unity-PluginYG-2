using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class BasicData : MonoBehaviour
    {
        public Text dataText;

        private void OnEnable()
        {
            YG2.onGetSDKData += DrawText;
            YG2.onFocusWindowGame += OnDrawTextHostBool;
            YG2.onPauseGame += OnDrawTextHostBool;
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= DrawText;
            YG2.onFocusWindowGame -= OnDrawTextHostBool;
            YG2.onPauseGame -= OnDrawTextHostBool;
        }

        private void Start()
        {
            DrawText();
        }

        public void DrawText()
        {
            string platformName = $"platform = <color=#cd5200>{YG2.platform}</color>";
            string isFirstGameSession = $"is First Game Session = {YG2.isFirstGameSession.ToString()}";
            string gameplaying = $"is Gameplaying = {YG2.isGameplaying.ToString()}";
            string pauseGame = $"is Pause Game = {YG2.isPauseGame.ToString()}";

            dataText.text = $"{platformName}\n{isFirstGameSession}\n{gameplaying}\n{pauseGame}";
        }

        private void OnDrawTextHostBool(bool b)
        {
            DrawText();
        }
    }
}