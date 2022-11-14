using HumanGun.GunRelated;
using UnityEngine;
using DG.Tweening;

namespace HumanGun.Interactable
{
    public class StickMan : MonoBehaviour, IStickAdded
    {
        private Animator _animator;
        private Collider _collider;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
        }
        public void AddStickMan(Transform gunTransform)
        {
            transform.parent = gunTransform;
        }
        public void RepositionStickMan(int poseIndex, Transform newLocalPosition)
        {
            _animator.SetTrigger(GunHandler.PoseNames[poseIndex]);
        }

        public void RemoveStickMan()
        {
            transform.parent = null;
            Destroy(_collider);
            transform.DOLocalJump(new Vector3(transform.position.x+Random.Range(-4f,4f), transform.position.y-2, transform.position.z+ Random.Range(-4f, 4f)), 3, 1, 1.5f, false).
                OnComplete(() => Destroy(gameObject));
        }
    }
}
