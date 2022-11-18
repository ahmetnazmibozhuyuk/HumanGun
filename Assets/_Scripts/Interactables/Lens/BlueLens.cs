using TMPro;
using UnityEngine;

namespace HumanGun.Interactable
{
    public class BlueLens : MonoBehaviour
    {
        [SerializeField] private int amountToSpawn = 1;
        [SerializeField] private GameObject stickManPrefab;
        [SerializeField] private TextMeshProUGUI additionAmountText;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Destroy(GetComponent<Collider>());
                for(int i = 0; i < amountToSpawn; i++)
                {
                    Instantiate(stickManPrefab, new Vector3(transform.position.x, 2, transform.position.z),Quaternion.identity);
                }
            }
        }

    }
}
