using JetBrains.Annotations;
using UnityEngine;

namespace Core
{
    public class CellData
    {
        public CellData Parent { get; set; }
        
        public (int x, int y) Position { get; set; } 
        public CellType CellType { get; set; }
        
        public float DistanceToFinish { get; set; }
        
        public float PassedDistance { get; set; }

        public float TotalDistance => PassedDistance + DistanceToFinish;

        public bool IsFirstElement => PassedDistance == 0;
    }
}