namespace HumanGun.Managers
{
    using System;
    public static class GameStateHandler
    {
        public static GameState CurrentState { get; private set; }

        public static event Action OnGameAwaitingStartState;
        public static event Action OnPoseControlState;
        public static event Action OnFightState;
        public static event Action OnFailState;
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
                case GameState.PoseControlState:
                    OnPoseControlState?.Invoke();
                    break;
                case GameState.FightState:
                    OnFightState?.Invoke();
                    break;
                case GameState.FailState:
                    OnFailState?.Invoke();
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
        PoseControlState = 2,
        FightState = 3,
        FailState = 4,
        GameWon = 5,
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