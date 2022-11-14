using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanGun.Managers;
using System;
using HumanGun.Interactable;

namespace HumanGun.GunRelated
{
    [RequireComponent(typeof(Animator))]
    public class GunHandler : MonoBehaviour
    {
        public static string[] PoseNames = { "Pose0", "Pose1", "Pose2", "Pose3" };
        [SerializeField] private GameObject bulletProjectile;

        private Animator _animator;

        private GunMode _currentGunMode;

        private int _attachedStickManAmount;

        private readonly int _pistolSwitchAmount = 1;
        private readonly int _rifleSwitchAmount = 10;
        private readonly int _shotgunSwitchAmount = 20;

        private List<IStickAdded> _stickManList = new List<IStickAdded>();

        [SerializeField] private StickMenInfo[] stickMenInfo;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            //InvokeRepeating("PistolShoot", 1,0.5f);
        }
        private void OnEnable()
        {
            GameStateHandler.OnGameStartedState += SetInitialPose;
        }
        private void OnDisable()
        {
            GameStateHandler.OnGameStartedState -= SetInitialPose;
        }
        private void SetInitialPose()
        {
            _animator.SetTrigger("IsRunning");
        }
        private void Update()
        {
            
            Shoot();
        }
        private void OnTriggerEnter(Collider other)
        {
            AddStickMan(other);
            HitObstacle(other);
        }
        private void AddStickMan(Collider other)
        {
            if (other.gameObject.GetComponent<IStickAdded>() == null) return;
            IStickAdded stickMan = other.gameObject.GetComponent<IStickAdded>();
            _stickManList.Add(stickMan);
            Debug.Log("added stickman, new count ="+_stickManList.Count);
            stickMan.AddStickMan(transform);
            stickMan.RepositionStickMan(stickMenInfo[_stickManList.Count].PoseIndex, stickMenInfo[_stickManList.Count].LocalTransform);
            CheckIfShouldSwitch();
            _animator.SetTrigger(PoseNames[0]);
        }
        private void HitObstacle(Collider other)
        {
            if (other.gameObject.GetComponent<IObstacleInteraction>() == null) return;
            RemoveStickMan(1);
        }

        public void RemoveStickMan(int stickManAmount)
        {
            if (_stickManList.Count > 0)
            {
                _stickManList[_stickManList.Count - 1].RemoveStickMan();
                _stickManList.RemoveAt(_stickManList.Count - 1);
                Debug.Log("added stickman, new count =" + _stickManList.Count);
                CheckIfShouldSwitch();
                return;
            }
            GameLost();

        }
        private void GameLost()
        {
            _animator.SetTrigger("IsDead");
            GameStateHandler.ChangeState(GameState.GameLost);
        }


        #region Gun Mode Switch and Configuration
        private void CheckIfShouldSwitch()
        {
            if(_stickManList.Count <= 0)
            {
                _animator.SetTrigger("IsRunning");
                return;
            }
            if (_stickManList.Count >= _shotgunSwitchAmount)
            {
                SwitchGunMode(GunMode.Shotgun);
                return;
            }
            if (_stickManList.Count >= _rifleSwitchAmount)
            {
                SwitchGunMode(GunMode.Rifle);
                return;
            }
            if (_stickManList.Count >= _pistolSwitchAmount)
            {
                SwitchGunMode(GunMode.Pistol);
                return;
            }
        }
        private void SwitchGunMode(GunMode gunModeToSwitch)
        {
            if (gunModeToSwitch == _currentGunMode) return;

            switch (gunModeToSwitch)
            {
                case GunMode.Pistol:
                    PistolConfiguration();
                    break;
                case GunMode.Rifle:
                    RifleConfiguration();
                    break;
                case GunMode.Shotgun:
                    ShotgunConfiguration();
                    break;
            }
        }
        private void PistolConfiguration()
        {
            
        }
        private void RifleConfiguration()
        {

        }
        private void ShotgunConfiguration()
        {

        }
        #endregion

        #region Shoot Methods
        private void Shoot()
        {
            if (_currentGunMode == GunMode.Idle) return;

            switch (_currentGunMode)
            {
                case GunMode.Pistol:
                    PistolShoot();
                    break;
                case GunMode.Rifle:
                    RifleShoot();
                    break;
                case GunMode.Shotgun:
                    ShotgunShoot();
                    break;
            }
        }
        private void PistolShoot()
        {
            GameObject spawnedBullet = Instantiate(bulletProjectile, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            spawnedBullet.GetComponent<BulletProjectile>().ShootBullet(new Vector3(0, 0, 20));
        }

        private void RifleShoot()
        {
            
        }
        private void ShotgunShoot()
        {

        }
        #endregion
    }
    public enum GunMode { Idle = 0, Pistol = 1, Rifle = 2, Shotgun = 3 }
    public interface IStickAdded
    {
        public void AddStickMan(Transform gunTransform);
        public void RepositionStickMan(int poseIndex, Transform newLocalPosition);
        public void RemoveStickMan();
    }
    [Serializable]
    public struct StickMenInfo
    {
        public Transform LocalTransform;
        public int PoseIndex;
    }
}
