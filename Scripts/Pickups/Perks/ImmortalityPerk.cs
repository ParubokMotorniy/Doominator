using System.Collections;
using UnityEngine;

namespace Game.PickUps
{
    public class ImmortalityPerk : MonoBehaviour,IPerk
    {
        [SerializeField] int immortalityLayer;
        [SerializeField] float immortalityduration;
        public void ActivatePerk(GameObject target)
        {
            StartCoroutine(SetImmortal(target));
        }
        private IEnumerator SetImmortal(GameObject target)
        {
            int originalLayer = target.gameObject.layer;
            target.gameObject.layer = immortalityLayer;
            yield return new WaitForSeconds(immortalityduration);
            target.gameObject.layer = originalLayer;
            Destroy(this);
        }
    }
}

