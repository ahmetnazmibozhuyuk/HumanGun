using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HumanGun.Interactable
{
    public class Obstacle : MonoBehaviour, IObstacleInteraction
    {
        [SerializeField] private int maxLivesAmount = 2;

        public int CurrentLives { get; set; }
        private void Awake()
        {
            CurrentLives = maxLivesAmount;
        }
        public void HitObstacle(int hitAmount)
        {
            CurrentLives -= hitAmount;
            if (CurrentLives <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.DOShakeScale(0.2f,0.5f,5);
            }
        }
    }
    public interface IObstacleInteraction
    {
        public void HitObstacle(int hitAmount);
        public int CurrentLives { get; set; }
    }
}
