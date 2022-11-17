using HumanGun.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanGun.GunRelated
{
    [RequireComponent(typeof(Rigidbody))]
    public class ExplosiveProjectile : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private int _hitAmount = 1;
        private WaitForSeconds _despawnDelay = new WaitForSeconds(2);

        private List<IObstacleInteraction> aoeTargets = new List<IObstacleInteraction>();
        private void Awake()
        {
            _rigidbody= GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            Invoke(nameof(Explode),3f);
        }
        public void ShootBullet(Vector3 startVelocity, int hitAmount)
        {
            _rigidbody.velocity = startVelocity;
            _hitAmount = hitAmount;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player")) return;
            
            Debug.Log(collision.gameObject.name,collision.gameObject);
            Explode();

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IObstacleInteraction>() == null) return;

            aoeTargets.Add(other.GetComponent<IObstacleInteraction>());

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IObstacleInteraction>() == null) return;

            aoeTargets.Remove(other.GetComponent<IObstacleInteraction>());
        }
        private void Explode()
        {
            if(aoeTargets.Count > 0)
            {
                for (int i = 0; i < aoeTargets.Count; i++)
                {
                    aoeTargets[i].HitObstacle(_hitAmount);
                }
            }


            HitExplosion();
            Destroy(gameObject);
        }
        private void HitExplosion()
        {

        }

    }
}
