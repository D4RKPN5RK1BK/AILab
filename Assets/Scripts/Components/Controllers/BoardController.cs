using System.Collections.Generic;
using Core;
using UnityEngine;

/// <summary>
/// Этот контроллер должен реализовывать обычные действия доски,
/// тоесть все изменения клеток на ней должы производиться именно через этот контроллер и никак иначе
/// </summary>
public class BoardController : NavigateComponent
{
    
    public GameObject point;

    public Vector2Int fieldSize;

    private List<List<PointController>> _cells; 

    private void Awake()
    {
        _cells = new List<List<PointController>>();
        InitializeField();
    }

    /// <summary>
    /// Определяет ячейки для дальнешей ра боты с полем
    /// </summary>
    private void InitializeField()
    {
        for (var i = 0; i < fieldSize.x; i++)
        {
            _cells.Add(new List<PointController>());
            for (var j = 0; j < fieldSize.y; j++)
            {
                var temp = Instantiate(point, new Vector3(i - fieldSize.x / 2, 0, j - fieldSize.y / 2) + transform.position, new Quaternion());
                var component = temp.GetComponent<PointController>();
                component.UpdateCell(CellType.Empty);
                _cells[^1].Add(component);
            }
        }
        
        for (var i = -1; i < fieldSize.x + 1; i++)
        {
            var temp1 = Instantiate(point, new Vector3(i - fieldSize.x / 2, 0, fieldSize.y / 2) + transform.position, new Quaternion());
            var temp2 = Instantiate(point, new Vector3(i - fieldSize.x / 2, 0, -fieldSize.y / 2 - 1) + transform.position, new Quaternion());

            var component1 = temp1.GetComponent<PointController>();
            var component2 = temp2.GetComponent<PointController>();
            
            component1.UpdateCell(CellType.Brick);
            component2.UpdateCell(CellType.Brick);
        }

        for (int i = 0; i < fieldSize.y; i++)
        {
            var temp1 = Instantiate(point, new Vector3(fieldSize.x / 2, 0, i - fieldSize.y / 2) + transform.position, new Quaternion());
            var temp2 = Instantiate(point, new Vector3(-fieldSize.x / 2 - 1, 0, i -fieldSize.y / 2) + transform.position, new Quaternion());

            var component1 = temp1.GetComponent<PointController>();
            var component2 = temp2.GetComponent<PointController>();
            
            component1.UpdateCell(CellType.Brick);
            component2.UpdateCell(CellType.Brick);
        }
    }

    /// <summary>
    /// Обновляет информациб о клетке на поле
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    /// <param name="additionalData"></param>
    public void UpdateCell(Vector2Int pos, CellType type, string additionalData = null)
    {
        _cells[pos.x][pos.y].UpdateCell(type, additionalData);
    } 
}
