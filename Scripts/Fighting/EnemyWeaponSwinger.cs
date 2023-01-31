using Game.HealthManagement;
using Game.Tools;
using UnityEngine;
namespace Game.Combat
{
    public class EnemyWeaponSwinger : MonoBehaviour, IAttacker
    {
        [SerializeField] private float swingCoolDown;
        [SerializeField] private string attackTrigger;
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private LayerMask attackTargets,hitObstacles;
        [SerializeField] private Transform attackCapsuleOrigin;
        [SerializeField] private Vector2 attackCapsuleSize;
        [SerializeField] private CapsuleDirection2D attackCapsuleDirection;
        [SerializeField] private Component IAttackerIfCantSwing;
        [SerializeField] private int damage = 1;

        private float swingTimer = Mathf.Infinity;
        private GameObject target;
        public bool Attack(GameObject target)
        {
            if (swingTimer >= swingCoolDown && target.GetComponent<Health>().IsAlive())
            {
                Vector3 scanDirection = target.transform.position - transform.position;
                bool enemyBlocked = Physics2D.Raycast(transform.position, scanDirection, scanDirection.magnitude, hitObstacles);

                if (!enemyBlocked)
                {
                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("AttackIsOver", false);
                        enemyAnimator.SetTrigger(attackTrigger);
                    }
                    swingTimer = 0;
                } else
                {
                    if (IAttackerIfCantSwing != null)
                    {
                        IAttacker attacker = IAttackerIfCantSwing as IAttacker;
                        attacker.Attack(target);
                    } else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        void Update()
        {
            if (swingTimer < swingCoolDown)
            {
                swingTimer += Time.deltaTime;
            }
        }
        public void AttackPointReached()
        {

            Collider2D impactCollider = Physics2D.OverlapCapsule(attackCapsuleOrigin.position, attackCapsuleSize, attackCapsuleDirection, 0, attackTargets);
            if (impactCollider != null)
            {
                Health enemyHealth = impactCollider.gameObject.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
    }
}
