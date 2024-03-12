using ConnectThePops.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace ConnectThePops.Settings
{
    [CreateAssetMenu(fileName = nameof(GameSettings), menuName = Constants.MenuNames.SETTINGS + nameof(GameSettings))]
    public class GameSettings : ScriptableObject
    {
        [Range(2,10)]
        public int GridSize;
        public int DistanceBetweenPops;
        
        [Range(1.25f, 3)]
        public float CameraPadding;
    }
}
