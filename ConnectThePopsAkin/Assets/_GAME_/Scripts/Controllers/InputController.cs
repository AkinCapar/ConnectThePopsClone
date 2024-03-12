using ConnectThePops.Utilities;
using ConnectThePops.Views;
using UnityEngine;
using Zenject;

namespace ConnectThePops.Controllers
{
    public class InputController : ITickable
    {
        #region Injection

        private Camera _camera;
        private SignalBus _signalBus;
        
        public InputController([Inject(Id = Constants.ZenjectIDs.Camera)] Camera camera
            , SignalBus signalBus)
        {
            _camera = camera;
            _signalBus = signalBus;
        }

        #endregion
        
        public void Tick()
        {
            
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                
                RaycastHit2D[] results = new RaycastHit2D[1];
                Physics2D.RaycastNonAlloc(mouseWorldPosition, Vector3.forward, results);
                if (results[0].collider != null)
                {
                    if (results[0].collider.TryGetComponent(out PopView popView))
                    {
                        _signalBus.Fire(new PopViewTappedSignal(popView));
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _signalBus.Fire<TappingStoppedSignal>();
            }
        }
    }
}
