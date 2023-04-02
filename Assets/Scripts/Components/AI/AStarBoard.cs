using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Components.AI
{
    /// <summary>
    /// Реализация AStar алгоритма
    /// </summary>
    [RequireComponent(typeof(BoardController))]
    public class AStarBoard : NavigateComponent
    {
        private BoardController _boardController;
        private CellData[][] _cellGrid;
        private (int x, int y) _boardSize;
        private (int x, int y) _startPosition;
        private (int x, int y) _finishPosition;

        private void Awake()
        {
            _boardController = GetComponent<BoardController>();
            _boardSize = (_boardController.fieldSize.x, _boardController.fieldSize.y);
            _cellGrid = new CellData[_boardSize.x][];

            for (var i = 0; i < _boardSize.x; i++)
            {
                _cellGrid[i] = new CellData[_boardSize.y];
                for (var j = 0; j < _boardSize.y; j++)
                {
                    _cellGrid[i][j] = new CellData
                    {
                        Position = (i, j),
                        CellType = CellType.Empty
                    };
                }
                    
            }

            _startPosition = (Random.Range(0, _boardSize.x), Random.Range(0, _boardSize.y));
            _finishPosition = (Random.Range(0, _boardSize.x), Random.Range(0, _boardSize.y));
        }

        private void Start()
        {   
            GenerateBoardPoints();
            MarkSurroundingPointsAsObservable(_startPosition);
            InvokeRepeating(nameof(CalculateNextMove), 0.5f, 0.2f);
        }

        private void CalculateNextMove()
        {
            var observedPoints = GetObservedPoints();
            var optimal = FindOptimalPoint(observedPoints);

            if (optimal == null) 
                return;
            
            MarkClosePoint(optimal.Value);
            MarkSurroundingPointsAsObservable(optimal.Value);

            if (_cellGrid[_finishPosition.x][_finishPosition.y].CellType == CellType.Observed)
            {
                CancelInvoke(nameof(CalculateNextMove));
                FormPathToFinish();
            }
                
        }

        #region Вспомогательные функции для вычесления

        /// <summary>
        /// Формирование итогового пути от начала до конца
        /// </summary>
        public void FormPathToFinish()
        {
            var finish = _cellGrid[_finishPosition.x][_finishPosition.y];
            while (finish.Parent != null)
            {
                HighlitePoint(finish.Position);
                finish = finish.Parent;
            }
        }
        
        /// <summary>
        /// омечает все рядомстоящие пустые клетки как обозреваемые
        /// </summary>
        /// <param name="pos"></param>
        private void MarkSurroundingPointsAsObservable((int x, int y) pos)
        {
            var endX = pos.x + 2 <= _boardSize.x ? pos.x + 2 : _boardSize.x;
            var endY = pos.y + 2 <= _boardSize.y ? pos.y + 2 : _boardSize.y;
            var startX = pos.x - 1 < 0 ? 0 : pos.x - 1;
            var startY = pos.y - 1 < 0 ? 0 : pos.y - 1;
            
            for (var i = startX; i < endX; i++)
            for (var j = startY; j < endY; j++)
                if (i == pos.x || j == pos.y)
                    MarkObservePoint((i, j), pos);
        }

        /// <summary>
        /// Находит наиболее оптимальный вариант для движения в к целевой точке
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private (int x, int y)? FindOptimalPoint(List<(int x, int y)> points)
        {
            if (points == null || !points.Any())
                return null;
            
            var firstPos = points.First();
            var minimal = _cellGrid[firstPos.x][firstPos.y];

            foreach (var pos in points)
            {
                var cellData = _cellGrid[pos.x][pos.y];
                if (cellData.TotalDistance < minimal.TotalDistance || 
                    cellData.TotalDistance == minimal.TotalDistance && 
                    cellData.DistanceToFinish < minimal.DistanceToFinish)
                    minimal = _cellGrid[pos.x][pos.y];
            }

            return minimal.Position;
        }

        /// <summary>
        /// Получение позиций обозреваемых в данный момент точек
        /// </summary>
        /// <returns></returns>
        private List<(int x, int y)> GetObservedPoints()
        {
            var positions = new List<(int x, int y)>();

            foreach (var cellRow in _cellGrid)
            foreach (var cell in cellRow)
                if (cell.CellType is CellType.Observed)
                    positions.Add(cell.Position);

            return positions;
        }

        #endregion
        
        #region Ячейки

        /// <summary>
        /// Помечает клетку как просмотренную, больше к ней не будет обращенй
        /// </summary>
        /// <param name="pos"></param>
        private void MarkClosePoint((int x, int y) pos)
        {
            _cellGrid[pos.x][pos.y].CellType = CellType.Closed;
            _boardController.UpdateCell(new Vector2Int(pos.x, pos.y), CellType.Closed);
        }

        /// <summary>
        /// Пометить клетку как обозреваемую
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="parentPos"></param>
        private void MarkObservePoint((int x, int y) pos, (int x, int y) parentPos)
        {
            if (_cellGrid[pos.x][pos.y].CellType is CellType.Empty or CellType.Finish)
            {
                _cellGrid[pos.x][pos.y].CellType = CellType.Observed;
                _cellGrid[pos.x][pos.y].Parent = _cellGrid[parentPos.x][parentPos.y];
                _cellGrid[pos.x][pos.y].PassedDistance = _cellGrid[parentPos.x][parentPos.y].PassedDistance + 1;
                _cellGrid[pos.x][pos.y].DistanceToFinish = (new Vector2(_finishPosition.x, _finishPosition.y)  - new Vector2(pos.x, pos.y)).magnitude;

                var additionalData =
                    $"P:{_cellGrid[pos.x][pos.y].PassedDistance}\nD:{Mathf.Round(_cellGrid[pos.x][pos.y].DistanceToFinish * 100) / 100}\nT:{Mathf.Round(_cellGrid[pos.x][pos.y].TotalDistance * 100) / 100 }";
                
                _boardController.UpdateCell(new Vector2Int(pos.x, pos.y), CellType.Observed, additionalData);
            }
        }
        
        /// <summary>
        /// Помечает клетку как просмотренную, больше к ней не будет обращенй
        /// </summary>
        /// <param name="pos"></param>
        private void MarkStartPoint((int x, int y) pos)
        {
            _cellGrid[pos.x][pos.y].PassedDistance = 0;
            _cellGrid[pos.x][pos.y].DistanceToFinish = (new Vector2(_finishPosition.x, _finishPosition.y)  - new Vector2(pos.x, pos.y)).magnitude;
            _cellGrid[pos.x][pos.y].CellType = CellType.Start;
            _boardController.UpdateCell(new Vector2Int(pos.x, pos.y), CellType.Start);
        }
        
        /// <summary>
        /// Помечает клетку как просмотренную, больше к ней не будет обращенй
        /// </summary>
        /// <param name="pos"></param>
        private void MarkEndPoint((int x, int y) pos)
        {
            _cellGrid[pos.x][pos.y].CellType = CellType.Finish;
            _boardController.UpdateCell(new Vector2Int(pos.x, pos.y), CellType.Finish);
        }
        
        /// <summary>
        /// Отмечает точку как непрходимое препятствие
        /// </summary>
        /// <param name="pos"></param>
        private void MarkBrick((int x, int y) pos)
        {
            if (_cellGrid[pos.x][pos.y].CellType == CellType.Empty)
            {
                _cellGrid[pos.x][pos.y].CellType = CellType.Brick;
                _boardController.UpdateCell(new Vector2Int(pos.x, pos.y), CellType.Brick);    
            }
        }
        
        /// <summary>
        /// Отмечает точку как непрходимое препятствие
        /// </summary>
        /// <param name="pos"></param>
        private void ClearPoint((int x, int y) pos)
        {
            _cellGrid[pos.x][pos.y].CellType = CellType.Empty;
            _cellGrid[pos.x][pos.y].Parent = null;
            _boardController.UpdateCell(new Vector2Int(pos.x, pos.y), CellType.Empty);
        }
        
        private void HighlitePoint((int x, int y) pos)
        {
            _cellGrid[pos.x][pos.y].CellType = CellType.Marked;
            _boardController.UpdateCell(new Vector2Int(pos.x, pos.y), CellType.Marked);
        }

        #endregion
        
        #region Генерация

        /// <summary>
        /// Генерация старта, конца, и всех кирпичиков)
        /// </summary>
        private void GenerateBoardPoints()
        {
            MarkStartPoint(_startPosition);
            MarkEndPoint(_finishPosition);

            for (var i = 0; i < _cellGrid.Length; i++)
            for (var j = 0; j < _cellGrid[i].Length; j++)
                if (Random.Range(0, 100) > 75)
                    MarkBrick((i, j));
        }

        #endregion

    }
}