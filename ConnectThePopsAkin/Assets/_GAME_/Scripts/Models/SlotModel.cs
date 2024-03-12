using System.Collections;
using System.Collections.Generic;
using ConnectThePops.Views;
using UnityEngine;
using Zenject;

namespace ConnectThePops.Models
{
    public class SlotModel
    {
        private Vector2Int GridPos { get; }
        private GridModel _grid;
        public Vector2 WorldPos { get; }

        public PopView PopView { get; private set; }
        
        public bool IsEmpty { get; private set; }

        
        public SlotModel(Vector2Int gridPos, Vector2 worldPos, GridModel grid)
        {
            GridPos = gridPos;
            _grid = grid;
            WorldPos = worldPos;
        }


        public void SetPopView(PopView popView)
        {
            PopView = popView;
            IsEmpty = false;
        }

        public void SetEmpty()
        {
            PopView = null;
            IsEmpty = true;
        }

        public float GetDistanceToOtherSlot(SlotModel slot)
        {
            return Vector2Int.Distance(GridPos, slot.GridPos);
        }

        public bool IsAdjacent(SlotModel slot)
        {
            if (Vector2Int.Distance(GridPos, slot.GridPos) > Mathf.Sqrt(2))
            {
                return false;
            }

            return true;
        }
    }
}
