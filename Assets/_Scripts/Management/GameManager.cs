using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanGun.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject PlayerObject { get { return playerObject; } }
        [SerializeField] private GameObject playerObject;
        protected override void Awake()
        {
            base.Awake();

        }

    }
}
