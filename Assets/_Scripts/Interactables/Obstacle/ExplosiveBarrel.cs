using DG.Tweening;
using HumanGun.Interactable;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HumanGun
{
    public class ExplosiveBarrel : MonoBehaviour, IObstacleInteraction
    {
        public int CurrentLives { get; set; }

        [SerializeField]private int explosionDamage = 3;

        [SerializeField] private int maxLivesAmount = 2;
        [SerializeField] private TextMeshProUGUI remainingHealthText;

        [SerializeField] private LayerMask _layerMask;

        private void Start()
        {
            CurrentLives = maxLivesAmount;
            remainingHealthText.SetText(CurrentLives.ToString());
        }

        public void HitObstacle(int hitAmount)
        {
            CurrentLives -= hitAmount;
            remainingHealthText.SetText(CurrentLives.ToString());
            if (CurrentLives <= 0)
            {
                Explode();
            }
            else
            {
                transform.DOShakeScale(0.2f, 0.5f, 5);
            }
        }
        private void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2,  _layerMask);
            for(int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.TryGetComponent<IObstacleInteraction>(out var destructable))
                    destructable.HitObstacle(explosionDamage);
            }
            HitExplosion();
            Destroy(gameObject);
        }
        private void HitExplosion()
        {

        }
    }
}
