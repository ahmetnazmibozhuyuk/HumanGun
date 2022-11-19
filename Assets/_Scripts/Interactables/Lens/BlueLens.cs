using DG.Tweening;
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

        private Vector3 _finalPosition { get { return new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), 0.7f, transform.position.z + Random.Range(1f, 3f)); } }

        private void Start()
        {
            additionAmountText.SetText("+"+amountToSpawn.ToString());
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Destroy(GetComponent<Collider>());
                for (int i = 0; i < amountToSpawn; i++)
                {
                    Transform temp = Instantiate(stickManPrefab, spawnTransform.position, Quaternion.identity).transform;
                    temp.DOJump(_finalPosition, 2, 1, 1);
                }
            }
        }
    }
}
