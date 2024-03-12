using System;
using ConnectThePops.Settings;
using ConnectThePops.Models;
using ConnectThePops.Views;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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
        private SignalBus _signalBus;

        public GridController(GameSettings gameSettings
            , PopView.Factory popViewFactory
            , PopsSettings popsSettings
            , CameraController cameraController
            , SignalBus signalBus)
        {
            _gameSettings = gameSettings;
            _popViewFactory = popViewFactory;
            _popsSettings = popsSettings;
            _cameraController = cameraController;
            _signalBus = signalBus;
        }

        #endregion

        public void Initialize()
        {
            _signalBus.Subscribe<PopsConnectedSignal>(OnPopsConnectedSignal);
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
                    PopView popView = _popViewFactory.Create(_popsSettings.Pops[Random.Range(0, 6)]);
                    SlotModel slot = _gridModel.GetSlot(i, j);
                    slot.SetPopView(popView);
                    slot.PopView.SetCurrentSlot(slot);
                    popView.SetPositionImmediate(slot.WorldPos);
                }
            }
        }

        private void OnPopsConnectedSignal()
        {
            for (int i = 0; i < _gridSize.x; i++)
            {
                int emptySlotCountOnY = 0;
                for (int j = 0; j < _gridSize.y; j++)
                {
                    SlotModel slot = _gridModel.GetSlot(i, j);
                    if (slot.IsEmpty)
                    {
                        emptySlotCountOnY++;
                    }

                    else
                    {
                        SlotModel emptySlot = _gridModel.GetSlot(i, j - emptySlotCountOnY);
                        emptySlot.SetPopView(slot.PopView);
                        emptySlot.PopView.MoveToNewSlot(emptySlot, _gameSettings.PopsMoveTime).Forget();
                    }

                    if (_gridSize.y - 1 == j)
                    {
                        SpawnNewPops(i, emptySlotCountOnY, _gameSettings.PopsMoveTime).Forget();
                    }
                }
            }
        }

        private async UniTask SpawnNewPops(int gridPosX, int spawnAmount, float delayTime)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                PopView popView = _popViewFactory.Create(_popsSettings.Pops[Random.Range(0, 6)]);
                popView.transform.localScale = Vector3.zero;
                SlotModel slot = _gridModel.GetSlot(gridPosX, _gridSize.y - i - 1);
                slot.SetPopView(popView);
                slot.PopView.SetCurrentSlot(slot);
                popView.SetPositionImmediate(slot.WorldPos);
                await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
                popView.transform.DOScale(Vector3.one / 2, _gameSettings.PopsMoveTime);
            }
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PopsConnectedSignal>(OnPopsConnectedSignal);
        }
    }
}