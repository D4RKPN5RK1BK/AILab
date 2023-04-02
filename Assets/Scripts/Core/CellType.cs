namespace Core
{
    public enum CellType
    {
        /// <summary>
        /// Пустая клента
        /// </summary>
        Empty = 0,
        
        /// <summary>
        /// Клетка с непроходимой стеной
        /// </summary>
        Brick = 1,
        
        /// <summary>
        /// Клетка начала маршрута
        /// </summary>
        Start = 2,
        
        /// <summary>
        /// Клетка с концом маршрута
        /// </summary>
        Finish = 3,
        
        /// <summary>
        /// Клетка под рассмотрением 
        /// </summary>
        Observed = 4,

        /// <summary>
        /// Пройденная или нерпигодная клетка
        /// </summary>
        Closed = 5,

        /// <summary>
        /// Отмеченная клетка
        /// </summary>
        Marked = 6,
    }
}