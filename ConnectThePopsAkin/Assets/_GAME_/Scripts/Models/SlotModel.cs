using System.Collections;
using System.Collections.Generic;
using ConnectThePops.Views;
using UnityEngine;

namespace ConnectThePops.Models
{
    public class SlotModel
    {
        private Vector2Int _gridPos;
        private GridModel _grid;
        public Vector2 WorldPos { get; }

        private PopView _popView;


        
        public SlotModel(Vector2Int gridPos, Vector2 worldPos, GridModel grid)
        {
            _gridPos = gridPos;
            _grid = grid;
            WorldPos = worldPos;
        }


        public void SetPopView(PopView numberView)
        {
            _popView = numberView;

            if (_popView != null)
            {
                _popView.SetCurrentSlot(this);
            }
        }
    }
}
