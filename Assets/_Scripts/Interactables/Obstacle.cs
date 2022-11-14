using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HumanGun.Interactable
{
    public class Obstacle : MonoBehaviour, IObstacleInteraction
    {
        [SerializeField] private int maxLivesAmount = 2;
        private int _currentLives;

        private void Awake()
        {
            _currentLives = maxLivesAmount;
        }
        public void HitObstacle(int hitAmount)
        {
            _currentLives-= hitAmount;
            if (_currentLives <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.DOShakeScale(0.2f,1,20);
            }
        }
    }
    public interface IObstacleInteraction
    {
        public void HitObstacle(int hitAmount);
    }
}
