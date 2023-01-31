using System.Collections;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine;

namespace Game.HealthManagement
{
    public class Health : MonoBehaviour,IStateVoter
    {
        [SerializeField] private string hurtTrigger, deathTrigger, resurrectionTrigger;
        [SerializeField] private int maxLives;
        [SerializeField] Animator animator;

        private int defaultObjectLayer,livesAmount;
        private bool isStunned = false, isAlive = true;
        private void Awake()
        {
            livesAmount = 1;
        }
        public void TakeDamage(int damage)
        {
            livesAmount -= damage;
            if(livesAmount <= 0)
            {
                Death();
            } else
            {
                if (animator != null)
                {
                    animator.SetTrigger(hurtTrigger);
                }
            }
        }
        private void Death()
        {
            if(animator != null)
            {
                animator.SetTrigger(deathTrigger);
            }
        } 
        public void GiveLives(int livesToGive)
        {
            int livesToAdd = maxLives - livesAmount;
            livesAmount += Mathf.Clamp(livesToGive,0,livesToAdd);
        }
        public int GetLives()
        {
            return livesAmount;
        }
        public void OnHurtStateEnter()
        {
            defaultObjectLayer = gameObject.layer;
            gameObject.layer = 11;
            isStunned = true;
        }
        public void OnHurtStateExit()
        {
            gameObject.layer = defaultObjectLayer;
            isStunned = false;
        }
        public void DeathAnimationStarted()
        {
            gameObject.layer = 13;
            isAlive = false;
        }
        public void CharacterDied()
        {
            animator.enabled = false;
            isAlive = false;
        }
        [ContextMenu("Resurrect")]
        public void Resurrect()
        {
            animator.enabled = true;
            animator.SetTrigger(resurrectionTrigger);
        }
        public void OnResurrection()
        {
            livesAmount = 1;
            isAlive = true;
            gameObject.layer = defaultObjectLayer;
        }
        public bool IsAlive()
        {
            return isAlive;
        }
        public bool IsStunned()
        {
            return isStunned;
        }

        public bool Vote()
        {
            return isAlive;
        }
    }
}

