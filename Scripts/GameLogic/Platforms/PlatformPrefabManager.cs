using System.Collections;
using System.Collections.Generic;
using Game.PickUps;
using UnityEngine;

namespace Game.Platforms
{
    public class PlatformPrefabManager : MonoBehaviour
    {
        [SerializeField] Transform playerInitialPosition;
        [SerializeField] List<PerkPosition> perksPositions;
        public Vector3 GetPlayerInitialPosition()
        {
            return playerInitialPosition.position;
        }
        public List<PerkPosition> GetPerksPositions()
        {
            return perksPositions;
        }
    }

}
