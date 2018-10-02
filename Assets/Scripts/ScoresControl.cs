using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Perekatyvalki2D
{
    /// <summary></summary>
    public class ScoresControl : MonoBehaviour
    {
        private static string _bestScorePrefName = "BestScores";

        // 
        [SerializeField] private RespwanManager _respwanManager;

        // 
        [SerializeField] private Text _currentScoreText;
        [SerializeField] private Text _bestScoresText;

        // 
        public int CurrentScore { get; private set; }
        // 
        public int BestScores { get; private set; }



        // 
        private void Awake()
        {
            CurrentScore = 0;
            _respwanManager.ScoredSquare += OnPlusPoint;

            // 
            if (PlayerPrefs.HasKey(_bestScorePrefName))
                BestScores = PlayerPrefs.GetInt(_bestScorePrefName);
            else
                BestScores = 0;

            // 
            _bestScoresText.text = BestScores.ToString();
        }
        // 
        private void OnPlusPoint()
        {
            PlusPoint();
        }



        /// <summary>Сброс счета.</summary>
        public void ResetScores()
        {
            CurrentScore = 0;
            SetCurrentScores();
        }
        /// <summary>Добавить очко.</summary>
        public void PlusPoint()
        {
            CurrentScore++;
            SetCurrentScores();

            // 
            if (CurrentScore > BestScores)
            {
                PlayerPrefs.SetInt(_bestScorePrefName, CurrentScore);
                BestScores = CurrentScore;
                _bestScoresText.text = BestScores.ToString();
            }
        }

        // 
        private void SetCurrentScores()
        {
            _currentScoreText.text = CurrentScore.ToString();
        }

    }
}