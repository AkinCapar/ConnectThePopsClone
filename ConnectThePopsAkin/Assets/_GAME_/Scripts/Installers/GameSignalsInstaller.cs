using Zenject;

namespace ConnectThePops.Installer
{
    public class GameSignalsInstaller :Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PopViewTappedSignal>().OptionalSubscriber();
            Container.DeclareSignal<TappingStoppedSignal>().OptionalSubscriber();
            Container.DeclareSignal<PopsConnectedSignal>().OptionalSubscriber();
        }
    }
}
