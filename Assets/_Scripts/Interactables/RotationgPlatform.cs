using DG.Tweening;
using UnityEngine;

namespace HumanGun.Interactable
{
    public class RotationgPlatform : MonoBehaviour, IIndestructableObstacle
    {
        [SerializeField] private float rotationRate = 5f;
        [SerializeField] private int hitAmount = 1;
        private readonly Vector3 _rotation = new Vector3(0, 360, 0);
        private bool _isHit = false;

        private void Start()
        {
            transform.DOLocalRotate(_rotation, rotationRate, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear).SetRelative();
        }
        public int HitAmount()
        {
            if (_isHit) return 0;
            _isHit = true;
            return hitAmount;
        }
    }
}
