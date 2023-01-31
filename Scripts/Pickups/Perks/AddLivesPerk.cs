using System.Collections;
using System.Collections.Generic;
using Game.HealthManagement;
using UnityEngine;

namespace Game.PickUps
{
    public class AddLivesPerk : MonoBehaviour,IPerk
    {
        [SerializeField] int livesToGive;
        public void ActivatePerk(GameObject target)
        {
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.GiveLives(livesToGive);
            }
        }
        public void PerkFadeEnd()
        {
            Destroy(this.gameObject);
        }
    }
}

