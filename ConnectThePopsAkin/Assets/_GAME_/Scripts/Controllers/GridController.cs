using ConnectThePops.Settings;
using ConnectThePops.Models;
using ConnectThePops.Views;
using UnityEngine;

namespace ConnectThePops.Controllers
{
    public class GridController
    {
        private GridModel _gridModel;
        private Vector2Int _gridSize;

        #region Injection

        private GameSettings _gameSettings;
        private PopView.Factory _popViewFactory;
        private PopsSettings _popsSettings;
        private CameraController _cameraController;

        public GridController(GameSettings gameSettings
            , PopView.Factory popViewFactory
            , PopsSettings popsSettings
            , CameraController cameraController)
        {
            _gameSettings = gameSettings;
            _popViewFactory = popViewFactory;
            _popsSettings = popsSettings;
            _cameraController = cameraController;
        }
        
        #endregion
        public void Initialize()
        {
            _cameraController.AdjustCamera(_gameSettings.GridSize);
            _gridSize = new Vector2Int(_gameSettings.GridSize, _gameSettings.GridSize);
            _gridModel = new GridModel(_gridSize, _gameSettings.DistanceBetweenPops);

            SpawnPopsAtStart();
        }

        private void SpawnPopsAtStart()
        {
            for (int i = 0; i < _gridSize.x; i++)
            {
                for (int j = 0; j < _gridSize.y; j++)
                {
                    PopView popView =
                        _popViewFactory.Create(
                            _popsSettings.Pops[Random.Range(0, _popsSettings.Pops.Count)]);
                    _gridModel.GetSlot(i,j).SetPopView(popView);
                    popView.SetPositionImmediate(_gridModel.GetSlot(i,j).WorldPos);
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}
