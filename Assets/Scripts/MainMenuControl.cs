using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Perekatyvalki2D
{
    /// <summary></summary>
    public class MainMenuControl : MonoBehaviour
    {
        [SerializeField] private GameObject _stripsPanel;
        [SerializeField] private Text _scoresText;
        [SerializeField] private RespwanManager _respwanManager;
        [SerializeField] private ScoresControl _scoresControl;
        [SerializeField] private GameTimer _gameTimer;



        // 
        private void Awake()
        {
            _gameTimer.TimeEnd += OnTimeEnd;
        }
        // 
        private void OnTimeEnd()
        {
            ToMainMenu();
            _respwanManager.ResetRespawn();
        }

        // 
        private void Start()
        {
            ToMainMenu();
        }


        /// <summary></summary>
        public void OnStargGame()
        {
            ToGame();

            // 
            _respwanManager.RespawnAll();
            _scoresControl.ResetScores();
            _gameTimer.ResetTimer();
        }

        /// <summary></summary>
        public void OnEndGame()
        {
            Application.Quit();
        }


        /// <summary>Переключить UI в режим главного меню.</summary>
        public void ToMainMenu()
        {
            _scoresControl.gameObject.SetActive(false);
            _stripsPanel.SetActive(false);
            _gameTimer.gameObject.SetActive(false);

            // 
            gameObject.SetActive(true);
            _scoresText.text = _scoresControl.CurrentScore.ToString();

        }
        /// <summary>Переключить UI в режим игры.</summary>
        public void ToGame()
        {
            _scoresControl.gameObject.SetActive(true);
            _stripsPanel.SetActive(true);
            _gameTimer.gameObject.SetActive(true);

            // 
            gameObject.SetActive(false);
        }

    }
}