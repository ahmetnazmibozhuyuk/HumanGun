using UnityEngine;

namespace HumanGun.Interactable
{
    public class StationaryObstacle : MonoBehaviour, IIndestructableObstacle
    {
        [SerializeField] private int hitAmount = 1;
        private bool _isHit = false;


        public int HitAmount()
        {
            if (_isHit) return 0;
            _isHit = true;
            return hitAmount;
        }
    }
    public interface IIndestructableObstacle
    {
        public int HitAmount();
    }
}
