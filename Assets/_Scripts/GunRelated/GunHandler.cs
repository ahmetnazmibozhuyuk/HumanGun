using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanGun.Managers;
using System;
using HumanGun.Interactable;

namespace HumanGun.GunRelated
{
    public class GunHandler : MonoBehaviour
    {
        public static string[] PoseNames = { "Pose0", "Pose1", "Pose2", "Pose3" };
        [SerializeField] private GameObject bulletProjectile;
        [SerializeField] private GameObject explosiveProjectile;

        [SerializeField] private Animator animator;

        private GunMode _currentGunMode;

        [SerializeField]private WeaponInfo pistolInfo;
        [SerializeField]private WeaponInfo rifleInfo;
        [SerializeField]private WeaponInfo shotgunInfo;
        [SerializeField]private WeaponInfo grenadeLauncherInfo;

        [SerializeField] private float shotgunScatterAmount = 2f;

        private WeaponInfo _currentWeaponInfo;

        //private float _shootInterval = 1;

        private List<IStickAdded> _stickManList = new List<IStickAdded>();

        private StickMenConfiguration[] _currentStickMenConfiguration;

        [SerializeField] private StickMenConfiguration[] pistolStickMenConfiguration;
        [SerializeField] private StickMenConfiguration[] rifleStickMenConfiguration;
        [SerializeField] private StickMenConfiguration[] shotgunStickMenConfiguration;
        [SerializeField] private StickMenConfiguration[] grenadeLauncherStickMenConfiguration;

        private Action ShootAction;

        private float _passedTime;
        private RaycastHit hit;

        private void ShootLoop()
        {

            _passedTime += Time.deltaTime;
            if(_passedTime > _currentWeaponInfo.ShootInterval)
            {
                _passedTime = 0;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _currentWeaponInfo.ShootRange))
                    ShootAction?.Invoke();
            }
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
            animator.SetTrigger("IsRunning");
        }
        private void Update()
        {
            if(GameStateHandler.CurrentState != GameState.GameStarted) return;

            AssignShootAction();
            ShootLoop();
        }
        private void OnTriggerEnter(Collider other)
        {
            AddStickMan(other);
            HitDestructable(other);
            HitIndestructable(other);
        }
        private void AddStickMan(Collider other)
        {

            if (_stickManList.Count > 28)
            {
                Destroy(other.gameObject);
                GameManager.Instance.AddMoney(10);
                return;
            }

            if (!other.gameObject.TryGetComponent<IStickAdded>(out var stickMan)) return;
            
            _stickManList.Add(stickMan);
            CheckIfShouldSwitch();
            stickMan.AddStickMan(transform);
            stickMan.RepositionStickMan(_currentStickMenConfiguration[_stickManList.Count].PoseIndex, _currentStickMenConfiguration[_stickManList.Count].LocalTransform);
            animator.SetTrigger(PoseNames[0]);
        }
        private void HitDestructable(Collider other)
        {
            if (!other.gameObject.TryGetComponent<IObstacleInteraction>(out var interactedObstacle)) return;
            
            RemoveStickMan(interactedObstacle.CurrentLives);
            interactedObstacle.HitObstacle(_stickManList.Count);
        }
        private void HitIndestructable(Collider other)
        {
            if (!other.gameObject.TryGetComponent<IIndestructableObstacle>(out var indestructableObstacle)) return;

            RemoveStickMan(indestructableObstacle.HitAmount());
        }

        public void RemoveStickMan(int stickManAmount)
        {
            for(int i = 0; i < stickManAmount; i++)
            {
                if (_stickManList.Count > 0)
                {
                    _stickManList[_stickManList.Count - 1].RemoveStickMan();
                    _stickManList.RemoveAt(_stickManList.Count - 1);
                    CheckIfShouldSwitch();
                    continue;
                }
                GameLost();
                break;
            }
        }
        private void GameLost()
        {
            animator.SetTrigger("IsDead");
            GameStateHandler.ChangeState(GameState.GameLost);
        }

        #region Gun Mode Switch and Configuration
        private void CheckIfShouldSwitch()
        {
            if(_stickManList.Count <= 0)
            {
                animator.SetTrigger("IsRunning");
                ShootAction = null;
                return;
            }
            if (_stickManList.Count >= grenadeLauncherInfo.SwitchAmount)
            {
                SwitchGunMode(GunMode.GrenadeLaucher);
                return;
            }
            if (_stickManList.Count >= shotgunInfo.SwitchAmount)
            {
                SwitchGunMode(GunMode.Shotgun);
                return;
            }
            if (_stickManList.Count >= rifleInfo.SwitchAmount)
            {
                SwitchGunMode(GunMode.Rifle);
                return;
            }
            if (_stickManList.Count >= pistolInfo.SwitchAmount)
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
                case GunMode.GrenadeLaucher:
                    GrenadeLauncherConfiguration();
                    break;
            }
        }
        private void PistolConfiguration()
        {
            _currentGunMode = GunMode.Pistol;
            _currentWeaponInfo = pistolInfo;
            _currentStickMenConfiguration = pistolStickMenConfiguration;
            for(int i = 0; i < _stickManList.Count; i++)
            {
                _stickManList[i].RepositionStickMan(_currentStickMenConfiguration[i].PoseIndex, _currentStickMenConfiguration[i].LocalTransform);
            }
        }
        private void RifleConfiguration()
        {
            _currentGunMode = GunMode.Rifle;
            _currentWeaponInfo = rifleInfo;
            _currentStickMenConfiguration = rifleStickMenConfiguration;
            for (int i = 0; i < _stickManList.Count; i++)
            {
                _stickManList[i].RepositionStickMan(_currentStickMenConfiguration[i].PoseIndex, _currentStickMenConfiguration[i].LocalTransform);
            }
        }
        private void ShotgunConfiguration()
        {
            _currentGunMode = GunMode.Shotgun;
            _currentWeaponInfo = shotgunInfo;
            _currentStickMenConfiguration = shotgunStickMenConfiguration;
            for (int i = 0; i < _stickManList.Count; i++)
            {
                _stickManList[i].RepositionStickMan(_currentStickMenConfiguration[i].PoseIndex, _currentStickMenConfiguration[i].LocalTransform);
            }
        }
        private void GrenadeLauncherConfiguration()
        {
            _currentGunMode = GunMode.GrenadeLaucher;
            _currentWeaponInfo = grenadeLauncherInfo;
            _currentStickMenConfiguration = grenadeLauncherStickMenConfiguration;
            for (int i = 0; i < _stickManList.Count; i++)
            {
                _stickManList[i].RepositionStickMan(_currentStickMenConfiguration[i].PoseIndex, _currentStickMenConfiguration[i].LocalTransform);
            }
        }
        #endregion

        #region Shoot Methods
        private void AssignShootAction()
        {
            if (_currentGunMode == GunMode.Idle) return;

            switch (_currentGunMode)
            {
                case GunMode.Pistol:
                    ShootAction = PistolShoot;
                    break;
                case GunMode.Rifle:
                    ShootAction = RifleShoot;
                    break;
                case GunMode.Shotgun:
                    ShootAction = ShotgunShoot;
                    break;
                case GunMode.GrenadeLaucher:
                    ShootAction = GrenadeLauncherShoot;
                    break;
            }
        }
        private void PistolShoot()
        {
            GameObject spawnedBullet = Instantiate(bulletProjectile, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            spawnedBullet.GetComponent<BulletProjectile>().ShootBullet(new Vector3(0, 0, 20),1);
        }

        private void RifleShoot()
        {
            
            GameObject spawnedBullet = Instantiate(bulletProjectile, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            spawnedBullet.GetComponent<BulletProjectile>().ShootBullet(new Vector3(0, 0, rifleInfo.BulletSpeed),rifleInfo.BulletDamage);
        }
        private void ShotgunShoot()
        {
            for(int i = 0; i < UnityEngine.Random.Range(3,6); i++)
            {
                GameObject spawnedBullet = Instantiate(bulletProjectile, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                spawnedBullet.GetComponent<BulletProjectile>().ShootBullet(new Vector3
                    (UnityEngine.Random.Range(-shotgunScatterAmount, shotgunScatterAmount), 
                    UnityEngine.Random.Range(-shotgunScatterAmount, shotgunScatterAmount), 
                    shotgunInfo.BulletSpeed),
                    shotgunInfo.BulletDamage);
            }

        }
        private void GrenadeLauncherShoot()
        {

            GameObject spawnedBullet = Instantiate(explosiveProjectile, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            spawnedBullet.GetComponent<BulletProjectile>().ShootBullet(new Vector3(0, 3, grenadeLauncherInfo.BulletSpeed), grenadeLauncherInfo.BulletDamage);
        }
        #endregion
    }
    public enum GunMode { Idle = 0, Pistol = 1, Rifle = 2, Shotgun = 3, GrenadeLaucher = 4 }
    public interface IStickAdded
    {
        public void AddStickMan(Transform gunTransform);
        public void RepositionStickMan(int poseIndex, Transform newLocalTransform);
        public void RemoveStickMan();
    }
    [Serializable]
    public struct StickMenConfiguration
    {
        public Transform LocalTransform;
        public int PoseIndex;
    }
    [Serializable]
    public struct WeaponInfo
    {
        public int SwitchAmount;
        public float ShootInterval;
        public float ShootRange;
        public float BulletSpeed;
        public int BulletDamage;
    }
}
