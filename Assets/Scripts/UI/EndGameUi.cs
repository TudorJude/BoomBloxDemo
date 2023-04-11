using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BoomBloxDemo.UI
{
    public class EndGameUi : MonoBehaviour
    {
        public Action OnRestartGameRequest;
        [SerializeField]
        private Button _replayButton;
        [SerializeField]
        private TextMeshProUGUI _endGameText;
        public void Init(int bulletsUsed, int cannonBallsUsed)
        {
            _replayButton.onClick.AddListener(RestartGame);
            _endGameText.text = "Congratulations, dragonslayer! Cannonball used: " + cannonBallsUsed + ", bullets used: " + bulletsUsed + ".";
        }

        private void RestartGame()
        {
            OnRestartGameRequest?.Invoke();
        }
    }
}
