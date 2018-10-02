using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Perekatyvalki2D
{
    /// <summary></summary>
    public class GameTimer : MonoBehaviour
    {
        public event Action TimeEnd;

        // 
        [SerializeField] private Text _gameTimerText;

        // 
        public int _currentSeconds;
        // 
        private float _timerSec = 1.0f;

        // 
        private void Start()
        {
            _currentSeconds = GetCountSecondsTimer();
            ShowTimer();
        }
        // 
        private void Update()
        {
            if (_timerSec <= 0.0f)
            {
                _timerSec += 1.0f;
                _currentSeconds--;
                ShowTimer();

                // 
                if (_currentSeconds <= 0 && TimeEnd != null)
                    TimeEnd();
            }
            else
            {
                _timerSec -= Time.deltaTime;
            }
        }

        // 
        private void ShowTimer()
        {
            int min = _currentSeconds / 60;
            int sec = _currentSeconds % 60;
            _gameTimerText.text = min + ":";
            if (sec < 10)
                _gameTimerText.text += "0" + sec;
            else
                _gameTimerText.text += sec;
        }

        /// <summary>Сброс таймера.</summary>
        public void ResetTimer()
        {
            _currentSeconds = GetCountSecondsTimer();
            _timerSec = 1.0f;
        }

        // 
        private int GetCountSecondsTimer()
        {
            return GameSettings.Data.CountSeconds;
        }

    }
}