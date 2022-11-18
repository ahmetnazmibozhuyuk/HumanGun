using UnityEngine;

namespace HumanGun.Managers
{
    public class LevelMananger : MonoBehaviour
    {
        public static string CurrentLevelKey = "CurrentLevel";
        public static int CurrentLevel { get { return PlayerPrefs.GetInt(CurrentLevelKey); } }

        [SerializeField] private GameObject[] levels;
        private GameObject _currentLevelObject;

        public void RestartLevel()
        {
            LoadLevel();
            GameStateHandler.ChangeState(GameState.GameAwaitingStart);
        }
        public void NextLevel()
        {
            PlayerPrefs.SetInt(CurrentLevelKey, CurrentLevel + 1);
            LoadLevel();
            GameStateHandler.ChangeState(GameState.GameAwaitingStart);
        }
        public void LoadCurrentLevel()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            if(_currentLevelObject != null)
            {
                Destroy(_currentLevelObject);
            }
            if (CurrentLevel >= levels.Length)
            {
                _currentLevelObject = Instantiate(levels[Random.Range(0, levels.Length)]);
            }
            else
            {
                _currentLevelObject = Instantiate(levels[CurrentLevel]);
            }
        }
    }
}
