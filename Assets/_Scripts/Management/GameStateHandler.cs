namespace HumanGun.Managers
{
    using System;
    public static class GameStateHandler
    {
        public static GameState CurrentState { get; private set; }

        public static event Action OnGameAwaitingStartState;
        public static event Action OnGameStartedState;
        public static event Action OnGameLostState;
        public static event Action OnGameWonState;

        public static void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            switch (newState)
            {
                case GameState.GameAwaitingStart:
                    OnGameAwaitingStartState?.Invoke();
                    break;
                case GameState.GameStarted:
                    OnGameStartedState?.Invoke();
                    break;
                case GameState.GameLost:
                    OnGameLostState?.Invoke();
                    break;
                case GameState.GameWon:
                    OnGameWonState?.Invoke();
                    break;
                default:
                    throw new ArgumentException("Invalid game state selection.");
            }
        }
    }
    public enum GameState
    {
        GamePreStart = 0,
        GameAwaitingStart = 1,
        GameStarted = 2,
        GameLost = 3,
        GameWon = 4,
    }
}
namespace HumanGun.Managers
{
    using UnityEngine;
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this as T;
        }
    }
}