using System;
using System.Collections.Generic;
using Game.HealthManagement;
using Game.Player;
using Game.Tools;
using UnityEngine;

namespace Game.EnemiesLogic
{
    public class EnemyAttackController : MonoBehaviour,IActionProvider
    {
        [SerializeField] private List<AttackAction> attacks = new List<AttackAction>();
        [SerializeField] private ActionStack enemyFSM;
        [SerializeField] private Animator animator;
        [SerializeField] private string isInFightAnimState,attackAnimState;

        private GameObject player;
        private bool hasPushedAttackAction;

        public void Action()
        {
            float targetDistance = Vector3.Distance(player.transform.position, transform.position);
            for (int i = (attacks.Count - 1); i >= -1; i--)
            {
                if(i == -1 || player.GetComponent<PlayerController>().PlayerCanBeIgnored())
                {
                    hasPushedAttackAction = false;
                    animator.SetBool(isInFightAnimState, false);
                    enemyFSM.Pop();
                    break;
                }
                if (targetDistance > attacks[i].minAttackTriggerRange && Vector3.Distance(player.transform.position, transform.position) < attacks[i].maxAttackTriggerRange)
                {
                    Tools.Math.FlipCharacter(new Vector3(player.transform.position.x, transform.position.y, transform.position.z), transform);
                    IAttacker attacker = attacks[i].IAttacker as IAttacker;
                    if (attacker.Attack(player))
                    {
                        break;
                    }
                }
            }
        }
        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        void Update()
        {
            if(attacks.Count > 0 && !hasPushedAttackAction)
            {
                if (!player.GetComponent<PlayerController>().PlayerCanBeIgnored() && Vector3.Distance(player.transform.position, transform.position) <= attacks[attacks.Count-1].maxAttackTriggerRange)
                {
                    hasPushedAttackAction = true;
                    animator.SetBool(isInFightAnimState, true);
                    enemyFSM.Push(this);
                }
            }
        }
        [Serializable]
        private struct AttackAction {
            public Component IAttacker;
            public float minAttackTriggerRange;
            public float maxAttackTriggerRange;
        }
        public void CharacterDied()
        {
            this.enabled = false;
        }
        public void AttackOver()
        {
            animator.SetBool(attackAnimState,true);
        }
        public void AttackStarted()
        {
            //animator.SetBool(attackAnimState,false);
        }

        public void CancelAction()
        {
            animator.SetBool(isInFightAnimState, false);
        }
    }
}
