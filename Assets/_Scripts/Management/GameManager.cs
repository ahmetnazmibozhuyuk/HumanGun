using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
using HumanGun.GunRelated;

namespace HumanGun.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject PlayerObject { get { return playerObject; } }
        [SerializeField] private GameObject playerObject;

        [SerializeField] private GameObject bulletPrefab;
        protected override void Awake()
        {
            base.Awake();
            GameStateHandler.ChangeState(GameState.GameAwaitingStart);
        }


    }
}
