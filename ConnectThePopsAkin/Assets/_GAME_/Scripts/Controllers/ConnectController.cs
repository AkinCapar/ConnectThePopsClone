using System;
using System.Collections.Generic;
using ConnectThePops.Data;
using ConnectThePops.Settings;
using ConnectThePops.Utilities;
using ConnectThePops.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace ConnectThePops.Controllers
{
    public class ConnectController
    {
        private List<PopView> _connectedPops;
        private List<Transform> _lineTransforms;
        private PopView _firstTappedPop;
        private PopView _previousTappedPop;
        private PopView _currentTappedPop;
        private bool _tapEnabled;

        #region Injection

        private SignalBus _signalBus;
        private GameSettings _gameSettings;
        private LineRenderer _lineRenderer;
        private PopsSettings _popsSettings;
        private CalculationView _calculationView;

        public ConnectController(SignalBus signalBus
            , GameSettings gameSettings
            , [Inject(Id = Constants.ZenjectIDs.LineRenderer)] LineRenderer lineRenderer
            , [Inject(Id = Constants.ZenjectIDs.CalculationView)] CalculationView calculationView
            , PopsSettings popsSettings)
        {
            _signalBus = signalBus;
            _gameSettings = gameSettings;
            _lineRenderer = lineRenderer;
            _popsSettings = popsSettings;
            _calculationView = calculationView;
        }

        #endregion

        public void Initialize()
        {
            _connectedPops = new List<PopView>();
            _lineTransforms = new List<Transform>();
            
            float maxXY = (_gameSettings.GridSize - 1) * (float)_gameSettings.DistanceBetweenPops;
            float calcViewPos = maxXY / 2;

            _calculationView.gameObject.transform.position = new Vector2(calcViewPos, maxXY + _gameSettings.DistanceBetweenPops);
            
            _signalBus.Subscribe<PopViewTappedSignal>(OnPopViewTappedSignal);
            _signalBus.Subscribe<TappingStoppedSignal>(OnTappingStoppedSignal);

            _tapEnabled = true;
        }

        private void OnPopViewTappedSignal(PopViewTappedSignal signal)
        {
            if(!_tapEnabled) {return;}
                
            if (signal.PopView.IsTapped)
            {
                if (signal.PopView == _previousTappedPop)
                {
                    _currentTappedPop.Release();
                    if (_connectedPops.Count > 0)
                    {
                        _connectedPops.Remove(_connectedPops[^1]);
                    }
                    _currentTappedPop = _previousTappedPop;
                    _previousTappedPop = _connectedPops.Count > 0 ? _connectedPops[^1] : null;
                    ManageVisualFeedbacks();
                }
                return;
            }

            if (_firstTappedPop == null)
            {
                signal.PopView.Tapped();
                _firstTappedPop = signal.PopView;
                _currentTappedPop = signal.PopView;
                return;
            }

            if (_currentTappedPop.Value == signal.PopView.Value &&
                _currentTappedPop.CurrentSlot.IsAdjacent(signal.PopView.CurrentSlot))
            {
                _connectedPops.Add(_currentTappedPop);
                signal.PopView.Tapped();
                _previousTappedPop = _currentTappedPop;
                _currentTappedPop = signal.PopView;
                ManageVisualFeedbacks();
            }
        }

        private PopsData CalculateAndGetNewPopData()
        {
            int value = 0;
            
            if (_connectedPops.Count >= 3)
            { 
                value = _connectedPops[0].Value * 4;
            }
            
            else
            {
                value = _connectedPops[0].Value * 2;
            }

            float power = Mathf.Log(value, 2);

            if (((int)power - 1) < _popsSettings.Pops.Count)
            { 
                return _popsSettings.Pops[(int)power - 1];
            }

            else
            {
                return _popsSettings.Pops[^1];
            }
        }

        private void OnTappingStoppedSignal()
        {
            if(!_tapEnabled) {return;}
            _lineRenderer.enabled = false;
            _calculationView.gameObject.SetActive(false);
            
            if (_connectedPops.Count < 1)
            {
                ClearAll(false);
                return;
            }

            foreach (PopView pop in _connectedPops)
            {
                pop.Merge(_currentTappedPop.CurrentSlot);
            }

            DisableTap(_gameSettings.PopsMoveTime * 3).Forget();
            SendPopsConnectedSignal(_gameSettings.PopsMoveTime).Forget();
        }

        private async UniTask SendPopsConnectedSignal(float delayTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            _currentTappedPop.SetNewData(CalculateAndGetNewPopData());
            ClearAll(true);
            _signalBus.Fire<PopsConnectedSignal>();
        }

        private void ClearAll(bool isConnected)
        {
            if (isConnected)
            {
                _connectedPops.Clear();
            }
            _firstTappedPop = null;
            if(_currentTappedPop != null) {_currentTappedPop.Release();}
            _currentTappedPop = null;
            _previousTappedPop = null;
        }

        private void ManageVisualFeedbacks() 
        {
            if (_connectedPops.Count < 1)
            {
                _lineRenderer.enabled = false;
                _calculationView.gameObject.SetActive(false);
                return;
            }
            
            _calculationView.SetSprite(CalculateAndGetNewPopData().popSprite);
            _lineTransforms.Clear();
            Color color = _connectedPops[0].PopColor;
            color.a = 1;
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
            
            for (int i = 0; i < _connectedPops.Count; i++)
            {
                _lineTransforms.Add(_connectedPops[i].transform);
            }
            
            _lineTransforms.Add(_currentTappedPop.transform);

            _lineRenderer.positionCount = _lineTransforms.Count;
            for (int i = 0; i < _lineTransforms.Count; i++)
            {
                _lineRenderer.SetPosition(i, _lineTransforms[i].position);
            }

            _lineRenderer.enabled = true;
        }

        private async UniTask DisableTap(float delayTime)
        {
            _tapEnabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            _tapEnabled = true;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PopViewTappedSignal>(OnPopViewTappedSignal);
            _signalBus.Unsubscribe<TappingStoppedSignal>(OnTappingStoppedSignal);
        }
    }
}