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

        private List<IObstacleInteraction> aoeTargets;
        private void Awake()
        {
            _rigidbody= GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            Invoke(nameof(DespawnWithTime),3f);
        }
        public void ShootBullet(Vector3 startVelocity, int hitAmount)
        {
            _rigidbody.velocity = startVelocity;
            _hitAmount = hitAmount;
        }
        private void OnCollisionEnter(Collision collision)
        {
            DespawnWithTime();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IObstacleInteraction>() == null) return;

            aoeTargets.Remove(other.GetComponent<IObstacleInteraction>());

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IObstacleInteraction>() == null) return;

            aoeTargets.Add(other.GetComponent<IObstacleInteraction>());
        }
        private void TriggerExplosion()
        {

        }
        private void DespawnWithTime()
        {

            for (int i = 0; i < aoeTargets.Count; i++)
            {
                aoeTargets[i].HitObstacle(_hitAmount);
            }
            HitExplosion();
            Destroy(gameObject);
        }
        private void HitExplosion()
        {

        }

    }
}
