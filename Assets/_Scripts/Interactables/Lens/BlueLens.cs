using TMPro;
using UnityEngine;

namespace HumanGun.Interactable
{
    public class BlueLens : MonoBehaviour
    {
        [SerializeField] private int amountToSpawn = 1;
        [SerializeField] private GameObject stickManPrefab;
        [SerializeField] private TextMeshProUGUI additionAmountText;

        [SerializeField] private Transform spawnTransform;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Destroy(GetComponent<Collider>());
                for(int i = 0; i < amountToSpawn; i++)
                {
                    Instantiate(stickManPrefab, new Vector3(spawnTransform.position.x, spawnTransform.position.y, spawnTransform.position.z),Quaternion.identity);
                    // @todo spawn pozisyonları ve animasyonunu düzelt
                }
            }
        }

    }
}
