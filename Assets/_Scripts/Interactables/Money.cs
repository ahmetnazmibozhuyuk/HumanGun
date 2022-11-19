using HumanGun.Managers;
using UnityEngine;

namespace HumanGun.Interactable
{
    public class Money : MonoBehaviour
    {
        [SerializeField] private float moneyValue = 10;
        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Player"))
        //    {
        //        GameManager.Instance.AddMoney(moneyValue);
        //        Destroy(gameObject);
        //    }
        //}
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.AddMoney(moneyValue);
                Destroy(gameObject);
            }
        }
    }
}
