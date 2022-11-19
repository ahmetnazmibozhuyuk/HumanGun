using UnityEngine;
using HumanGun.GunRelated;
using ObjectPooling;
using System.Collections;

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

        [SerializeField] private GameObject explosionObject;
        [SerializeField] private GameObject cubeScatterGameObject;

        private WaitForSeconds _despawnDelay = new WaitForSeconds(1);

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

        }
        private void OnEnable()
        {
            GameStateHandler.OnGameAwaitingStartState += InitializeGame;
        }
        private void OnDisable()
        {
            GameStateHandler.OnGameAwaitingStartState -= InitializeGame;
        }
        private void Start()
        {
            GameStateHandler.ChangeState(GameState.GameAwaitingStart);
            StartLevel();
        }
        private void InitializeGame()
        {
            HasWon = false;
        }
        public void AddMoney(float amountToAdd)
        {
            _uiManager.AddMoney(amountToAdd);
        }
        public void StartLevel()
        {


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
        public void SpawnExplosionParticle(Vector3 position, float sizeMultiplier)
        {
            StartCoroutine(Co_SpawnParticle(explosionObject,position, sizeMultiplier));
        }
        public void CubeScatterParticle(Vector3 position, float sizeMultiplier)
        {
            StartCoroutine(Co_SpawnParticle(cubeScatterGameObject,position, sizeMultiplier));
        }
        private IEnumerator Co_SpawnParticle(GameObject spawnObject,Vector3 position, float sizeMultiplier)
        {
            GameObject particleToSpawn = ObjectPool.Spawn(spawnObject, position, Quaternion.identity);
            particleToSpawn.transform.localScale = Vector3.one* sizeMultiplier;
            yield return _despawnDelay;
            ObjectPool.Despawn(particleToSpawn);
        }
    }
}
