using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;

namespace Game.PickUps {
    public class InvisibilityPerk : MonoBehaviour, IPerk
    {
        [SerializeField] float invisibilityDuration;
        private bool coroutineIsRunning = false;
        public void ActivatePerk(GameObject target)
        {
            PlayerController controller = target.GetComponent<PlayerController>();
            if(controller != null && !controller.IsInvisible())
            {
                coroutineIsRunning = true;
                StartCoroutine(SetInvisible(controller));
            }
        }
        public void PerkFadeEnd()
        {
            if (!coroutineIsRunning)
            {
                Destroy(this.gameObject);
            }
        }
        private IEnumerator SetInvisible(PlayerController target)
        {
            target.SetInvisible(true,invisibilityDuration);
            yield return new WaitForSeconds(invisibilityDuration);
            target.SetInvisible(false,0);
            Destroy(this);
        }
    }
}


