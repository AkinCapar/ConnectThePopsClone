using Zenject;

namespace ConnectThePops.Installer
{
    public class GameSignalsInstaller :Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}
