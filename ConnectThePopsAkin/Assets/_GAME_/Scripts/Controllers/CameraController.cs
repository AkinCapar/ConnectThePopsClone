using System;
using ConnectThePops.Settings;
using ConnectThePops.Utilities;
using UnityEngine;
using Zenject;

namespace ConnectThePops.Controllers
{
    public class CameraController
    {
        private Camera _camera;
        private GameSettings _gameSettings;

        public CameraController([Inject(Id = Constants.ZenjectIDs.Camera)] Camera camera
            , GameSettings gameSettings)
        {
            _camera = camera;
            _gameSettings = gameSettings;
        }


        public void AdjustCamera(int gridSize)
        {
            float maxXY = (gridSize - 1) * (float)_gameSettings.DistanceBetweenPops;
            float cameraPos = maxXY / 2;

            _camera.orthographicSize = cameraPos / _camera.aspect + _gameSettings.CameraPadding;
            _camera.transform.position = new Vector3(cameraPos, cameraPos, _camera.transform.position.z);
        }
    }
}