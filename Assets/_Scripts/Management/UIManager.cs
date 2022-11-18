using HumanGun.Interactable;
using TMPro;
using UnityEngine;

namespace HumanGun.Managers
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI moneyText;

        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject preStartPanel;

        public void AddMoney(float moneyToAdd)
        {
            PlayerPrefs.SetFloat(GameManager.MoneyAmountKey, moneyToAdd+GameManager.MoneyAmount);
            moneyText.SetText("Money: "+GameManager.MoneyAmount.ToString());
        }

        private void OnEnable()
        {
            GameStateHandler.OnGameAwaitingStartState += InitializeGame;
            GameStateHandler.OnGameStartedState += StartGame;
            GameStateHandler.OnGameWonState += GameWon;
            GameStateHandler.OnGameLostState += GameLost;
        }
        private void OnDisable()
        {
            GameStateHandler.OnGameAwaitingStartState -= InitializeGame;
            GameStateHandler.OnGameStartedState -= StartGame;
            GameStateHandler.OnGameWonState -= GameWon;
            GameStateHandler.OnGameLostState -= GameLost;
        }

        private void InitializeGame()
        {
            levelText.SetText("Stage: "+(LevelMananger.CurrentLevel+1).ToString());
            moneyText.SetText("Money: "+GameManager.MoneyAmount.ToString());
            preStartPanel.SetActive(true);
            inGamePanel.SetActive(true);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
        }
        private void StartGame()
        {
            preStartPanel.SetActive(false);
        }
        private void GameWon()
        {
            winPanel.SetActive(true);
            inGamePanel.SetActive(false);
        }
        private void GameLost()
        {
            losePanel.SetActive(true);
            inGamePanel.SetActive(false);
        }
        



    }
}
