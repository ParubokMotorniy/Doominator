using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tools;
using Game.HealthManagement;
using Game.UI;

namespace Game.Combat
{
    public class Shooter : MonoBehaviour, IAttacker, IBarOwner
    {
        [SerializeField] float shootCoolDown;
        [SerializeField] GameObject missilePrefab;
        [SerializeField] Transform missileSpawnPoint;
        [SerializeField] string attackTrigger;
        [SerializeField] Animator animator;
        private float shootTimer = Mathf.Infinity;
        private GameObject target;
        public bool Attack(GameObject target)
        {
            if (shootTimer >= shootCoolDown)
            {
                this.target = target;
                if (animator != null)
                {
                    animator.SetBool("AttackIsOver", false);
                    animator.SetTrigger(attackTrigger);
                }
                shootTimer = 0;
                return true;
            }
            return false;
        }

        public float ReportValuePercentage()
        {
            return shootTimer / shootCoolDown;
        }

        public void ShootPointReached()
        {
            Vector3 missileDirection = target.transform.position - missileSpawnPoint.position;
            float angle = Mathf.Atan2(missileDirection.y, missileDirection.x);
            GameObject spawnedMissile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
            spawnedMissile.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);
        }
        void Update()
        {
            if (shootTimer < shootCoolDown)
            {
                shootTimer += Time.deltaTime;
            }
        }
    }
}

