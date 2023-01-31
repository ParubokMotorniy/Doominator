using Game.Player;
using UnityEngine;

namespace Game.PickUps
{
    public class AddBulletsPerk : MonoBehaviour, IPerk
    {
        [SerializeField] int bulletsToGive;
        public void ActivatePerk(GameObject target)
        {
            PlayerController controller = target.GetComponent<PlayerController>();
            if(controller != null)
            {
                controller.GiveBullets(bulletsToGive);
            }
        }
        public void PerkFadeEnd()
        {
            Destroy(this.gameObject);
        }
    }
}

