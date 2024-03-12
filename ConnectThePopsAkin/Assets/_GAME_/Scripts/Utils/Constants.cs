using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectThePops.Utilities
{
    public class Constants
    {
        public struct MenuNames
        {
            public const string ParentMenuName = "ConnectThePops/";
            public const string INSTALLERS = ParentMenuName + "Installers/";
            public const string SETTINGS = ParentMenuName + "Settings/";
        }

        public struct ZenjectIDs
        {
            public const string Camera = "Camera";
            public const string LineRenderer = "LineRenderer";
            public const string CalculationView = "CalculationView";
        }
        
        public struct Tags
        {
            public const string Pop = "Pop";
        }
    }
}
