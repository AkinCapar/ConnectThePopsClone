
using ConnectThePops.Views;

namespace ConnectThePops
{
    public readonly struct PopViewTappedSignal
    {
        public readonly PopView PopView;
        public PopViewTappedSignal(PopView _popView)
        {
            PopView = _popView;
        }
    }

    public readonly struct TappingStoppedSignal
    {
        
    }

    public readonly struct PopsConnectedSignal
    {
        
    }
}

