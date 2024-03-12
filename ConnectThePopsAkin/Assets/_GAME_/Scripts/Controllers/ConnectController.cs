using System.Collections.Generic;
using ConnectThePops.Views;
using UnityEngine;
using Zenject;

namespace ConnectThePops.Controllers
{
    public class ConnectController
    {
        private List<PopView> _connectedPops;
        private PopView _firstTappedPop;
        private PopView _lastTappedPop;

        #region Injection

        private SignalBus _signalBus;

        public ConnectController(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        #endregion

        public void Initialize()
        {
            _connectedPops = new List<PopView>();
            _signalBus.Subscribe<PopViewTappedSignal>(OnPopViewTappedSignal);
            _signalBus.Subscribe<TappingStoppedSignal>(OnTappingStoppedSignal);
        }

        private void OnPopViewTappedSignal(PopViewTappedSignal signal)
        {
            if (signal.PopView.IsTapped)
            {
                /*if (_connectedPops[^1] == signal.PopView)
                {
                    _lastTappedPop = _connectedPops[^1];
                    _connectedPops.Remove(_connectedPops[^1]);
                }*/
                return;
            }

            if (_firstTappedPop == null)
            {
                signal.PopView.Tapped();
                _firstTappedPop = signal.PopView;
                _lastTappedPop = signal.PopView;
                return;
            }

            if (_lastTappedPop.Value == signal.PopView.Value &&
                _lastTappedPop.CurrentSlot.IsAdjacent(signal.PopView.CurrentSlot))
            {
                _connectedPops.Add(_lastTappedPop);
                signal.PopView.Tapped();
                _lastTappedPop = signal.PopView;
            }
        }

        private void OnTappingStoppedSignal()
        {
            if (_connectedPops.Count < 1)
            {
                ClearAll(false);
                return;
            }

            foreach (PopView pop in _connectedPops)
            {
                pop.Merge(_lastTappedPop.CurrentSlot);
            }
            
            _signalBus.Fire<PopsConnectedSignal>();
            ClearAll(true);
        }

        private void ClearAll(bool isConnected)
        {
            if (isConnected)
            {
                _connectedPops.Clear();
            }
            _firstTappedPop = null;
            _lastTappedPop.Release();
            _lastTappedPop = null;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PopViewTappedSignal>(OnPopViewTappedSignal);
            _signalBus.Unsubscribe<TappingStoppedSignal>(OnTappingStoppedSignal);
        }
    }
}