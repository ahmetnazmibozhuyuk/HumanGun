using HumanGun.Interactable;
using System.Collections;
using UnityEngine;

namespace HumanGun.GunRelated
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletProjectile : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private int _hitAmount = 1;
        private WaitForSeconds _despawnDelay = new WaitForSeconds(2);
        private void Awake()
        {
            _rigidbody= GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            Invoke(nameof(DespawnWithTime),0.2f);
        }
        public void ShootBullet(Vector3 startVelocity, int hitAmount)
        {
            _rigidbody.velocity = startVelocity;
            _hitAmount = hitAmount;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IObstacleInteraction>() == null) return;

            other.GetComponent<IObstacleInteraction>().HitObstacle(_hitAmount);
            HitExplosion();
            Destroy(gameObject);
        }
        private void DespawnWithTime()
        {
            Destroy(gameObject);
        }
        private void HitExplosion()
        {

        }

    }
}
