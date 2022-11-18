using TMPro;
using UnityEngine;

namespace HumanGun.Interactable
{
    public class RedLens : MonoBehaviour, IIndestructableObstacle
    {
        [SerializeField] private TextMeshProUGUI lifeText;
        [SerializeField] private int hitAmount = 1;
        private bool _isHit = false;

        private void Start()
        {
            lifeText.SetText("-" + hitAmount);
        }
        public int HitAmount()
        {
            if (_isHit) return 0;
            _isHit = true;
            return hitAmount;
        }
    }
}
