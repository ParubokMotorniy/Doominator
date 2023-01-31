using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System {
    public class GameManagement : MonoBehaviour
    {
        public static bool gameIsOver;
        void Awake()
        {
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}
