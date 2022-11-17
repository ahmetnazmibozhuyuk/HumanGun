using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
using HumanGun.GunRelated;
using System.Drawing;
using UnityEditor.Experimental.GraphView;

namespace HumanGun.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject PlayerObject { get { return playerObject; } }
        [SerializeField] private GameObject playerObject;

        [SerializeField] private GameObject bulletPrefab;

        public bool HasWon { get; private set; }

        private UIManager _uiManager;
        private LevelMananger _levelManager;

        #region String Keys and Saved Variables

        public static string MoneyAmountKey = "Money";
        public static float MoneyAmount { get { return PlayerPrefs.GetFloat(MoneyAmountKey); } }



        public static string CurrentLevelKey = "CurrentLevel";
        public static int CurrentLevel { get { return PlayerPrefs.GetInt(CurrentLevelKey); } }




        #endregion

        #region Color Related
        [SerializeField] private Material redMat;
        [SerializeField] private Material greenMat;
        [SerializeField] private Material blueMat;
        [SerializeField] private Material blackMat;
        [SerializeField] private Material whiteMat;
        [SerializeField] private Material yellowMat;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _uiManager = GetComponent<UIManager>();
            _levelManager= GetComponent<LevelMananger>();
            GameStateHandler.ChangeState(GameState.GameAwaitingStart);
        }
        private void Start()
        {
            StartLevel();
        }

        public void AddMoney(float amountToAdd)
        {
            _uiManager.AddMoney(amountToAdd);

            
        }
        public void StartLevel()
        {
            HasWon = false;

            _levelManager.LoadCurrentLevel();
        }
        public void PassedFinishLine()
        {
            HasWon = true;
        }

        public void RestartLevel()
        {

        }
        public void NextLevel()
        {
        }

        public Material StickMaterial(ColorList color)
        {
            switch (color)
            {
                case ColorList.Blue:
                    return blueMat;
                case ColorList.White:
                    return whiteMat;
                case ColorList.Yellow:
                    return yellowMat;
                case ColorList.Red:
                    return redMat;
                case ColorList.Black:
                    return blackMat;
                case ColorList.Green:
                    return greenMat;
                default:
                    return whiteMat;
            }
        }
    }
}
