using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perekatyvalki2D
{
    /// <summary></summary>
    public class SideCell : MonoBehaviour
    {
        // 
        private RespwanManager _respwanManager;
        // 
        private SpriteRenderer _spriteRenderer;
        /// <summary>Идентификатор.</summary>
        public int Index { get; private set; }
        /// <summary>Номер стороны.</summary>
        public int Side { get; private set; }
        /// <summary>Номер стороны.</summary>
        public int ColorIndex { get; private set; }

        // 
        public float TranslateVelocity = 1.0f;

        // 
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private float _factor;
        private bool _isMove = false;
        private bool _isMoving = false;



        // 
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        // 
        private void Update()
        {
            if (_isMove)
            {
                transform.position = Vector3.Lerp(_startPosition, _endPosition, _factor);
                _factor += TranslateVelocity * Time.deltaTime;
                if (_factor > 1.0f)
                {
                    transform.position = _endPosition;
                    _isMove = false;
                }
            }
        }



        // 
        public void Init(RespwanManager respwanManager, int index, int sideNum, bool isMoving)
        {
            _respwanManager = respwanManager;
            Index = index;
            Side = sideNum;
            _isMoving = isMoving;
        }

        /// <summary>Установить новый цвет.</summary>
        public void SetColor(Color color, int colorIndex)
        {
            _spriteRenderer.material.SetColor("_Color", color);
            ColorIndex = colorIndex;
        }

        /// <summary></summary>
        public void MoveToRespawn(Vector3 newPosition)
        {
            _startPosition = transform.position;
            _endPosition = newPosition;
            _factor = 0.0f;
            _isMove = true;
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_isMove || !_isMoving) return;

            // 
            SideCell otherCell = collision.GetComponent<SideCell>();
            if (otherCell != null)
            {
                if (otherCell.Side != Side)
                {
                    _respwanManager.CellsCollision(this, otherCell);
                }
            }
        }

    }
}
