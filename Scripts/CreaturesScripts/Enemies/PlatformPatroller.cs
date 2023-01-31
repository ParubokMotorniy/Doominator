using Game.Tools;
using UnityEngine;

namespace Game.EnemiesLogic {
    public class PlatformPatroller : MonoBehaviour,IActionProvider
    {
        [SerializeField] Transform patrolBorder1, patrolBorder2;
        [SerializeField] float borderDwellTime,speed;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] Animator animator;
        private Transform currentTarget;
        private float dwellTime = 0;
        private bool activeTimer;
        public void Action()
        {
            if (sprite.isVisible)
            {
                if (!Mathf.Approximately(transform.position.x, currentTarget.position.x))
                {
                    Vector3 targetPos = Vector3.MoveTowards(transform.position, currentTarget.position, speed + Time.deltaTime);
                    transform.position = targetPos;
                    Math.FlipCharacter(new Vector3(currentTarget.position.x, transform.position.y, transform.position.z),transform);
                } else
                {
                    if(activeTimer != true)
                    {
                        activeTimer = true;
                        animator.SetBool("IsPatrolling", true);
                    }
                }
                if (dwellTime >= borderDwellTime)
                {
                    dwellTime = 0;
                    activeTimer = false;
                    animator.SetBool("IsPatrolling", false);
                    if (currentTarget == patrolBorder1)
                    {
                        currentTarget = patrolBorder2;
                    }
                    else
                    {
                        currentTarget = patrolBorder1;
                    }
                }
            }
        }
        public void DeathAnimationStarted()
        {
            this.enabled = false;
        }
        public void CancelAction()
        {
        }

        void Start()
        {
            currentTarget = patrolBorder1;
        }
        private void Update()
        {
            if(activeTimer)
            {
                dwellTime += Time.deltaTime;
            }
        }

    }
}

