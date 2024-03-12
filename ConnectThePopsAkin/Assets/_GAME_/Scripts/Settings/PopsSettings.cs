using System.Collections;
using System.Collections.Generic;
using ConnectThePops.Data;
using ConnectThePops.Utilities;
using UnityEngine;

namespace ConnectThePops.Settings
{
    [CreateAssetMenu(fileName = nameof(PopsSettings), menuName = Constants.MenuNames.SETTINGS + nameof(PopsSettings))]
    public class PopsSettings : ScriptableObject
    {
        public List<PopsData> Pops;
    }
}
