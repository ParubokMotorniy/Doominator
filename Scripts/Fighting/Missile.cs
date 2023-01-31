using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.HealthManagement;
namespace Game.Combat
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] float missileStartSpeed;
        [SerializeField] int missileDamage = 1;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Health collisionHealth = collision.gameObject.GetComponent<Health>();
            if(collisionHealth != null)
            {
                collisionHealth.TakeDamage(missileDamage);
            }
            Destroy(gameObject);
        }
        void Start()
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                rigidbody.velocity = transform.right * missileStartSpeed;
            }
            Destroy(gameObject,15);
        }
    }

}
