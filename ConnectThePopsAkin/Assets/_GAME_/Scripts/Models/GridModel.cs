using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectThePops.Models
{
    public class GridModel
    {
        private SlotModel[,] _slots;
        private int _gridSizeX;
        private int _gridSizeY;

        public GridModel(Vector2Int gridSize, int distance)
        {
            _gridSizeX = gridSize.x;
            _gridSizeY = gridSize.y;
            _slots = new SlotModel[_gridSizeX, _gridSizeY];
            
            for (int i = 0; i < _gridSizeX; i++)
            {
                for (int j = 0; j < _gridSizeY; j++)
                {
                    var worldPos = Vector2.zero + new Vector2(distance * i , distance * j);
                    _slots[i, j] = new SlotModel(new Vector2Int(i, j), worldPos, this);
                }
            }
        }

        public SlotModel GetSlot(int x, int y)
        {
            return _slots[x, y];
        }
    }
}
