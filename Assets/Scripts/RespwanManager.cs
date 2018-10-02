using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perekatyvalki2D
{
    /// <summary></summary>
    public class RespwanManager : MonoBehaviour
    {
        public event Action ScoredSquare;

        // 
        [SerializeField] private RectTransform _leftTop;
        [SerializeField] private RectTransform _leftBottom;
        [SerializeField] private RectTransform _rightTop;
        [SerializeField] private RectTransform _rightBottom;

        // 
        private Vector3[] _leftPositions = new Vector3[3];
        private Vector3[] _rightPositions = new Vector3[3];

        // 
        private Vector3 _scaleCell = Vector3.one;

        // 
        [SerializeField] private GameObject _squarePrefab;
        [SerializeField] private GameObject _circlePrefab;

        // 
        [SerializeField] private Color[] _availableColors;
        private int[] _rndColors;

        // 
        private SideCell[] _leftSideCells = new SideCell[3];
        private SideCell[] _rightSideCells = new SideCell[3];

        // 
        private SideCell _dragCell;

        // 
        private Resolution _currentResolution;

        // 
        private bool _isFirstTap = true;
        private SideCell _dualTapCell;
        private float _firstDualTimer = 1.0f;
        private float _secondDualTimer = 1.0f;





        // 
        private void Start()
        {
            _currentResolution = Screen.currentResolution;

            // 
            CalculatePositions();

            // 
            InstantiateSide(_squarePrefab, _leftSideCells, 0, true);
            InstantiateSide(_circlePrefab, _rightSideCells, 1, false);

            // 
            ResetRespawn();

            // 
            if (GameSettings.Data.Colors.Length >= 3)
            {
                _availableColors = new Color[GameSettings.Data.Colors.Length];
                for (int i = 0; i < GameSettings.Data.Colors.Length; i++)
                {
                    ColorVector color3 = GameSettings.Data.Colors[i];
                    _availableColors[i] = new Color(color3.r, color3.g, color3.b);
                }
            }
            else
            {
                if (_availableColors.Length < 3)
                    _availableColors = new Color[] { Color.red, Color.green, Color.blue };
            }

            // 
            _rndColors = new int[_availableColors.Length - 2];
            for (int i = 0; i < 3; i++)
            {
                SetRandomColor(_leftSideCells[i]);
                SetRandomColor(_rightSideCells[i]);
            }
        }

        /// <summary></summary>
        private void CalculatePositions()
        {
            Vector3 leftTop = Camera.main.ScreenToWorldPoint(_leftTop.position);
            Vector3 leftBottom = Camera.main.ScreenToWorldPoint(_leftBottom.position);
            Vector3 rightTop = Camera.main.ScreenToWorldPoint(_rightTop.position);
            //Vector3 rightBottom = Camera.main.ScreenToWorldPoint(_rightBottom.position);

            // 
            float dx = Mathf.Abs(leftTop.x - rightTop.x) / 6;
            float dy = Mathf.Abs(leftTop.y - leftBottom.y) / 6;

            // 
            float x = leftTop.x + dx;
            _leftPositions[0] = new Vector3(x, leftTop.y - dy, 0.0f);
            _leftPositions[1] = new Vector3(x, leftTop.y - 3 * dy, 0.0f);
            _leftPositions[2] = new Vector3(x, leftTop.y - 5 * dy, 0.0f);

            // 
            x = leftTop.x + 5 * dx;
            _rightPositions[0] = new Vector3(x, leftTop.y - dy, 0.0f);
            _rightPositions[1] = new Vector3(x, leftTop.y - 3 * dy, 0.0f);
            _rightPositions[2] = new Vector3(x, leftTop.y - 5 * dy, 0.0f);

            // 
            x = (500.0f / Screen.currentResolution.width) + (500.0f / Screen.currentResolution.height);
            _scaleCell = new Vector3(x, x, 1.0f);
        }

        /// <summary>Создать игровые объекты.</summary>
        private void InstantiateSide(GameObject respawnPrefab, SideCell[] arraySideCells, int sideNum, bool isMoving)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject respawnGO = GameObject.Instantiate<GameObject>(respawnPrefab);
                SideCell sideCell = respawnGO.GetComponent<SideCell>();
                arraySideCells[i] = sideCell;
                sideCell.Init(this, i, sideNum, isMoving);
                sideCell.SetColor(_availableColors[i], i);
            }
        }

        /// <summary></summary>
        public void RespawnAll()
        {
            RespawnSide(_leftPositions, _leftSideCells);
            RespawnSide(_rightPositions, _rightSideCells);
        }
        /// <summary>Создание экземпляров с одной из сторон.</summary>
        public void RespawnSide(Vector3[] side, SideCell[] arraySideCells)
        {
            for (int i = 0; i < 3; i++)
            {
                SideCell sideCell = arraySideCells[i];
                RespawnPrefab(side[i], sideCell);
            }
        }
        /// <summary>Создать экземпляр в заданной точке.</summary>
        public void RespawnPrefab(Vector3 respawnPoint, SideCell sideCell)
        {
            sideCell.transform.position = respawnPoint;
            sideCell.transform.localScale = _scaleCell;
        }

        /// <summary></summary>
        public void ResetRespawn()
        {
            Vector3 resetPos =new Vector3(1000.0f, 0.0f, 0.0f);
            for (int i = 0; i < 3; i++)
            {
                _leftSideCells[i].transform.position = resetPos;
                _rightSideCells[i].transform.position = -resetPos;
            }
        }

        /// <summary>Заменить цвета всех объектов на правой стороне.</summary>
        public void RandomizeRightColor()
        {
            for (int i = 0; i < 3; i++)
            {
                SetRandomColor(_rightSideCells, _rightSideCells[i]);
            }
        }
        /// <summary>Установить рандомный цвет.</summary>
        public void SetRandomColor(SideCell sideCell)
        {
            if (sideCell.Side == 0)
                SetRandomColor(_leftSideCells, sideCell);
            else if (sideCell.Side == 1)
                SetRandomColor(_rightSideCells, sideCell);
        }
        // 
        private void SetRandomColor(SideCell[] arraySideCells, SideCell sideCell)
        {
            int[] colorsIndex = new int[2];
            for (int i = 0, j = 0; i < 3; i++)
            {
                SideCell cell = arraySideCells[i];
                if (cell != sideCell)
                {
                    colorsIndex[j] = cell.ColorIndex;
                    j++;
                }
            }

            // 
            for (int i = 0, j = 0; i < _availableColors.Length; i++)
            {
                if (colorsIndex[0] != i && colorsIndex[1] != i)
                {
                    _rndColors[j] = i;
                    j++;
                }
            }

            // 
            int index = _rndColors[UnityEngine.Random.Range(0, _rndColors.Length)];
            sideCell.SetColor(_availableColors[index], index);
        }

        //
        private void CheckCellColor(SideCell sideCell)
        {
            for (int i = 0; i <_rightSideCells.Length; i++)
            {
                if (sideCell.ColorIndex == _rightSideCells[i].ColorIndex) return;
            }

            // 
            SetRandomColor(sideCell);
        }

        /// <summary>Столкновение двух объектов.</summary>
        public void CellsCollision(SideCell firstCell, SideCell secondCell)
        {
            SideCell moveCell = (firstCell.Side == 0) ? firstCell : secondCell;
            if (firstCell.ColorIndex == secondCell.ColorIndex)
            {
                // телепортировать объект на его место с заменой цвета
                RespawnPrefab(_leftPositions[moveCell.Index], moveCell);
                SetRandomColor(moveCell);

                // 
                if (ScoredSquare != null)
                    ScoredSquare();
            }
            else
            {
                // переместить объект обратно на его место
                moveCell.MoveToRespawn(_leftPositions[moveCell.Index]);
            }
        }



        // перетаскивание тачем
        private void Update()
        {
            if (_currentResolution.width != Screen.currentResolution.width ||
                _currentResolution.height != Screen.currentResolution.height)
            {
                CalculatePositions();
                RespawnAll();
                _currentResolution = Screen.currentResolution;
            }

            // 
            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                Vector2 ray = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                RaycastHit2D raycastHit = Physics2D.Raycast(ray, Vector2.zero);
                switch (touches[i].phase)
                {
                    case TouchPhase.Began:
                        if (raycastHit)
                        {
                            _dragCell = raycastHit.transform.gameObject.GetComponent<SideCell>();
                            if (_dragCell.Side != 0)
                            {
                                _dragCell = null;
                            }
                            else
                            {
                                if (_isFirstTap)
                                    _firstDualTimer = 1.0f;
                                else
                                    _secondDualTimer = 1.0f;
                            }
                        }
                        break;

                    case TouchPhase.Moved:
                        if (_dragCell != null)
                        {
                            Vector3 pos = Camera.main.ScreenToWorldPoint(touches[i].position);
                            pos.z = 0.0f;
                            _dragCell.transform.position = pos;

                            // 
                            if (_isFirstTap)
                                _firstDualTimer -= Time.deltaTime;
                            else
                                _secondDualTimer -= Time.deltaTime;
                        }
                        break;

                    case TouchPhase.Ended:
                        if (_isFirstTap)
                        {
                            if (_firstDualTimer > 0)
                            {
                                _dualTapCell = _dragCell;
                                _isFirstTap = false;
                            }
                        }
                        else
                        {
                            _isFirstTap = true;
                            if (_secondDualTimer > 0)
                            {
                                CheckCellColor(_dualTapCell);
                            }
                        }

                        // 
                        _dragCell = null;
                        break;
                }
            }
        }




    }
}