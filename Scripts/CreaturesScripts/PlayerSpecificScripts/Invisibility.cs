using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class Invisibility : MonoBehaviour
    {
        [SerializeField] GameObject invisibilityMask;
        SpriteMask mask;
        public IEnumerator GoInvisible(float invisibilityDuration)
        {
            SpriteMask mask = invisibilityMask.GetComponent<SpriteMask>();
            if(mask != null)
            {
                mask.enabled = true;
                float distanceToCover = invisibilityMask.transform.localScale.y;
                float speed = distanceToCover / invisibilityDuration;
                while (invisibilityMask.transform.localScale.y > 0)
                {
                    invisibilityMask.transform.localScale += new Vector3(0, -Time.deltaTime * speed, 0);
                    yield return new WaitForEndOfFrame();
                }
                mask.enabled = false;
                invisibilityMask.transform.localScale = new Vector3(invisibilityMask.transform.localScale.x,distanceToCover, invisibilityMask.transform.localScale.z);
            }
        }
    }
}

