using TMPro;
using UnityEngine;

namespace HumanGun.Managers
{
    public class UIManager : MonoBehaviour
    {


        [SerializeField] private TextMeshProUGUI moneyText;

        private void Start()
        {
            moneyText.SetText("Money: " + GameManager.MoneyAmount.ToString());
        }
        public void AddMoney(float moneyToAdd)
        {
            PlayerPrefs.SetFloat(GameManager.MoneyAmountKey, moneyToAdd+GameManager.MoneyAmount);
            moneyText.SetText("Money: "+GameManager.MoneyAmount.ToString());
        }

    }
}
