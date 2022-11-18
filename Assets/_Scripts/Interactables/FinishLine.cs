using HumanGun.Managers;
using UnityEngine;

namespace HumanGun
{
    public class FinishLine : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            GameManager.Instance.PassedFinishLine();
        }
    }
}
