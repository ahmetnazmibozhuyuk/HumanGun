using HumanGun.GunRelated;
using UnityEngine;
using DG.Tweening;

namespace HumanGun.Interactable
{
    public class StickMan : MonoBehaviour, IStickAdded
    {
        private Animator _animator;
        private Collider _collider;

        private readonly float _scatterRange = 2f;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
        }
        public void AddStickMan(Transform gunTransform)
        {
            transform.parent = gunTransform;
        }
        public void RepositionStickMan(int poseIndex, Transform newLocalTransform)
        {
            _animator.SetTrigger(GunHandler.PoseNames[poseIndex]);
            transform.DOLocalMove(newLocalTransform.localPosition, 0.5f);
            transform.DOLocalRotateQuaternion(newLocalTransform.localRotation, 0.5f);
        }

        public void RemoveStickMan()
        {
            transform.parent = null;
            Destroy(_collider);
            transform.DORotate(new Vector3(Random.Range(-300, 300), Random.Range(-300, 300), Random.Range(-300, 300)),1.5f);
            transform.DOLocalJump(new Vector3(transform.position.x+Random.Range(-_scatterRange, _scatterRange),
                transform.position.y, transform.position.z+ Random.Range(-_scatterRange, _scatterRange)), 2, 1, 1f, false).
                OnComplete(() => Destroy(gameObject));
        }
    }
}
