using UnityEngine;
using DG.Tweening;
using TMPro;
using HumanGun.Managers;

namespace HumanGun.Interactable
{
    public class Obstacle : MonoBehaviour, IObstacleInteraction
    {
        [SerializeField] private int maxLivesAmount = 2;
        [SerializeField] private TextMeshProUGUI remainingHealthText;

        public int CurrentLives { get; set; }
        private void Awake()
        {
            CurrentLives = maxLivesAmount;

        }
        private void Start()
        {
            remainingHealthText.SetText(CurrentLives.ToString());
        }
        public void HitObstacle(int hitAmount)
        {
            CurrentLives -= hitAmount;
            remainingHealthText.SetText(CurrentLives.ToString());
            if (CurrentLives <= 0)
            {
                GameManager.Instance.CubeScatterParticle(transform.position, 1.5f);
                Destroy(gameObject);
            }
            else
            {
                transform.DOShakeScale(0.2f,0.5f,5);
                GameManager.Instance.CubeScatterParticle(transform.position, 0.8f);
            }
        }
    }
    public interface IObstacleInteraction
    {
        public void HitObstacle(int hitAmount);
        public int CurrentLives { get; set; }
    }
}
