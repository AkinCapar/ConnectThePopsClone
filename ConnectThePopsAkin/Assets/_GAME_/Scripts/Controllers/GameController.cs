using System;
using ConnectThePops.Utilities;
using TMPro.EditorUtilities;
using Zenject;

namespace ConnectThePops.Controllers
{
    public class GameController : IInitializable, ITickable, IDisposable
    {
        private GameStates _gameState = GameStates.WaitingToStart;
        private GridController _gridController;
        private ConnectController _connectController;

        public GameController(GridController gridController
            , ConnectController connectController)
        {
            _gridController = gridController;
            _connectController = connectController;
        }

        public void Initialize()
        {
            _connectController.Initialize();
        }

        public void Tick()
        {
            switch (_gameState)
            {
                case GameStates.WaitingToStart:
                {
                    UpdateStarting();
                    break;
                }
                case GameStates.Playing:
                {
                    UpdatePlaying();
                    break;
                }
            }
        }

        private void UpdatePlaying()
        {
            
        }

        private void UpdateStarting()
        {
            if (_gameState != GameStates.WaitingToStart)
            {
                return;
            }

            StartGame();
        }

        private void StartGame()
        {
            if(_gameState != GameStates.WaitingToStart) { return;}
            
            _gridController.Initialize();
            _gameState = GameStates.Playing;
        }

        public GameStates GameState
        {
            get { return _gameState; }
        }

        public void Dispose()
        {
            _gridController.Dispose();
            _connectController.Dispose();
        }
    }
}
