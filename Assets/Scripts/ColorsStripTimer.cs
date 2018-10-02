using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Perekatyvalki2D
{
    /// <summary></summary>
    public class ColorsStripTimer : MonoBehaviour
    {
        [SerializeField] private RespwanManager _respwanManager;

        // 
        [SerializeField] private TimeStrip[] _timeStrips;

        // 
        [SerializeField] private RectTransform _topAnchor;
        [SerializeField] private RectTransform _bottomAnchor;
        [SerializeField] private RectTransform _heightAnchor;

        // 
        [SerializeField] private float _stripVelocity;

        /// <summary>Высота области таймера.</summary>
        private float Height { get { return _heightAnchor.rect.height; } }
        /// <summary>Верхняя крайняя точка.</summary>
        private float TopY { get { return _topAnchor.position.y; } }
        /// <summary>Нижняя крайняя точка.</summary>
        private float BottomY { get { return _bottomAnchor.position.y; } }

        /// <summary>Коэфициент перехода от координаты Y к высоте полосы.</summary>
        private float _factorY;
        /// <summary>Максимальная рандомная высота полосы.</summary>
        private Vector2 _randomHeight;
        /// <summary>Максимальная высота следующей полосы</summary>
        private Vector2 _nextHeight;

        /// <summary>Последняя созданная полоса.</summary>
        private TimeStrip _lastStrip;
        /// <summary>Координата при достижении которой будет создана новая полоса.</summary>
        private float _stripY;



        // 
        private void Start()
        {
            float lengthY = TopY + Mathf.Abs(BottomY);
            _factorY = Height / lengthY;

            // 
            _randomHeight = new Vector2(Height * 0.1f, Height * 0.8f);
            _nextHeight = _randomHeight;

            // 
            StartStrip();
        }

        // 
        private void Update()
        {
            for (int i = 0; i < _timeStrips.Length; i++)
            {
                if (!_timeStrips[i].IsFree)
                {
                    Vector3 pos = _timeStrips[i].StripTransform.position;
                    pos.y += _stripVelocity * Time.deltaTime;

                    // 
                    if (pos.y <= _bottomAnchor.position.y)
                    {
                        _timeStrips[i].StripTransform.position = _bottomAnchor.position + _bottomAnchor.position;
                        _timeStrips[i].IsFree = true;
                        _respwanManager.RandomizeRightColor();
                    }
                    else
                    {
                        _timeStrips[i].StripTransform.position = pos;
                    }
                }
            }

            // 
            if (_lastStrip == null)
            {
                _nextHeight = _randomHeight;
                StartStrip();
            }
            else if (_lastStrip.StripTransform.position.y <= _stripY)
            {
                StartStrip();
            }
        }
        /// <summary></summary>
        private void StartStrip()
        {
            for (int i = 0; i < 3; i++)
            {
                TimeStrip strip = _timeStrips[i];
                if (strip.IsFree)
                {
                    strip.StripTransform.position = _topAnchor.position;
                    strip.IsFree = false;

                    // 
                    Vector2 sizeDelta = strip.StripTransform.sizeDelta;
                    sizeDelta.y = Random.Range(_nextHeight.x, _nextHeight.y);
                    strip.StripTransform.sizeDelta = sizeDelta;

                    // 
                    _nextHeight = new Vector2(_randomHeight.x, 
                        Random.Range(_randomHeight.x, _randomHeight.y));

                    // 
                    _lastStrip = strip;
                    _stripY = _topAnchor.position.y - (_nextHeight.y / _factorY);
                    break;
                }
            }
        }



    }
}