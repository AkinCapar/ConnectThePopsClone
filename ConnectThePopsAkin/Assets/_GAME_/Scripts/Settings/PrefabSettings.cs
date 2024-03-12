using System.Collections;
using System.Collections.Generic;
using ConnectThePops.Utilities;
using ConnectThePops.Views;
using UnityEngine;

namespace ConnectThePops.Settings
{
    [CreateAssetMenu(fileName = nameof(PrefabSettings), menuName = Constants.MenuNames.SETTINGS + nameof(PrefabSettings))]

    public class PrefabSettings : ScriptableObject
    {
        public PopView popView;
    }
}
