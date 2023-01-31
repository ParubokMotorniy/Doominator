using Cinemachine;
using Game.Platforms;
using UnityEngine;

namespace Game.PickUps
{
    public class LowerWallPerk : MonoBehaviour,IPerk
    {
        [Range(-10,0)]
        [SerializeField] float amountToLower;
        public void ActivatePerk(GameObject target)
        {
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponentInChildren<WallOfFireController>().MoveWall(amountToLower);
        }
        public void PerkFadeEnd()
        {
            Destroy(this.gameObject);
        }
    }
}

