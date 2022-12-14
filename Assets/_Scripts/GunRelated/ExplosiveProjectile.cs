using HumanGun.Interactable;
using HumanGun.Managers;
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
        private List<GameObject> aoeTargetObjects = new List<GameObject>();
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            Invoke(nameof(Explode), 3f);
        }
        public void ShootBullet(Vector3 startVelocity, int hitAmount)
        {
            _rigidbody.velocity = startVelocity;
            _hitAmount = hitAmount;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player")) return;

            Explode();

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IObstacleInteraction>() == null) return;

            aoeTargets.Add(other.GetComponent<IObstacleInteraction>());
            aoeTargetObjects.Add(other.gameObject);

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IObstacleInteraction>() == null) return;

            aoeTargets.Remove(other.GetComponent<IObstacleInteraction>());
            aoeTargetObjects.Remove(other.gameObject);
        }
        private void Explode()
        {
            if (aoeTargets.Count > 0)
            {
                for (int i = 0; i < aoeTargets.Count; i++)
                {
                    if (aoeTargetObjects[i] != null)
                        aoeTargets[i].HitObstacle(_hitAmount);
                }
            }

            HitExplosion();
            Destroy(gameObject);
        }
        private void HitExplosion()
        {
            GameManager.Instance.SpawnExplosionParticle(transform.position, 0.5f);
        }

    }
}
