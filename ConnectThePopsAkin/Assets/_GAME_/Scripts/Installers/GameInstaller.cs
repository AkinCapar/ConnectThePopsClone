using ConnectThePops.Controllers;
using ConnectThePops.Data;
using ConnectThePops.Settings;
using ConnectThePops.Views;
using Zenject;

namespace ConnectThePops.Installer
{
    public class GameInstaller : MonoInstaller
    {
        #region Injection

        private PrefabSettings _prefabSettings;

        [Inject]
        private void Construct(PrefabSettings prefabSettings)
        {
            _prefabSettings = prefabSettings;
        }

        #endregion
        public override void InstallBindings()
        {
            GameSignalsInstaller.Install(Container);
            
            //CONTROLLERS
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.Bind<CameraController>().AsSingle();
            Container.Bind<GridController>().AsSingle();
            //Container.BindInterfacesTo<InputController>().AsSingle();

            InstallPops();
        }
        
        private void InstallPops()
        {
            Container.BindFactory<PopsData, PopView, PopView.Factory>()
                .FromPoolableMemoryPool<PopsData, PopView, PopView.Pool>(poolBinder => poolBinder
                    .WithInitialSize(100)
                    .FromComponentInNewPrefab(_prefabSettings.popView));
        }
    }
}
