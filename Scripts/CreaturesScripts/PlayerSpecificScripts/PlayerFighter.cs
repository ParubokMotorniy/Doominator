using System.Collections;
using System.Collections.Generic;
using Game.HealthManagement;
using Game.UI;
using UnityEngine;

namespace Game.Player
{
    public class PlayerFighter : MonoBehaviour,IBarOwner
    {
        [SerializeField] private float attackRange, swingCoolDown;
        [SerializeField] private LayerMask enemies;
        [SerializeField] Animator animator;
        [SerializeField] string attackTrigger;

        private float swingTimer = Mathf.Infinity;
        private Health enemyHealth;


        void Update()
        {
            if (swingTimer < swingCoolDown)
            {
                swingTimer += Time.deltaTime;
            }
        }
        public void SwingSword(Touch targetTouch)
        {
            if (swingTimer >= swingCoolDown)
            {
                Vector3 convertedPosition = Camera.main.WorldToScreenPoint(transform.position);
                if (Vector3.Distance(targetTouch.position, convertedPosition) <= attackRange)
                {
                    RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(targetTouch.position), enemies);
                    if (hit.collider != null)
                    {
                        enemyHealth = hit.collider.gameObject.GetComponent<Health>();
                    }
                }
                if (animator != null)
                {
                    animator.SetTrigger(attackTrigger);
                }
                swingTimer = 0;
            }
        }
        public void AttackPointReached()
        {          
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
        }

        public float ReportValuePercentage()
        {
            return swingTimer / swingCoolDown;
        }
    }

}