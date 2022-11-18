using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanGun.Managers;
using System;
using HumanGun.Interactable;
using DG.Tweening;

namespace HumanGun.GunRelated
{
    public class GunHandler : MonoBehaviour
    {
        public static string[] PoseNames = { "Pose0", "Pose1", "Pose2", "Pose3" };
        [SerializeField] private GameObject bulletProjectile;
        [SerializeField] private GameObject explosiveProjectile;

        [SerializeField] private Animator animator;
        private BoxCollider _collider;

        private GunMode _currentGunMode;

        [SerializeField]private WeaponInfo pistolInfo;
        [SerializeField]private WeaponInfo rifleInfo;
        [SerializeField]private WeaponInfo shotgunInfo;
        [SerializeField]private WeaponInfo grenadeLauncherInfo;

        [SerializeField] private float shotgunScatterAmount = 2f;
        [SerializeField] private float grenadeLauncherVerticalVelocity = 2f;

        private WeaponInfo _currentWeaponInfo;

        private List<IStickAdded> _stickManList = new List<IStickAdded>();
        private List<Transform> _attachedStickManTransform = new List<Transform>();

        private StickMenConfiguration[] _currentStickMenConfiguration;

        [SerializeField] private StickMenConfiguration[] pistolStickMenConfiguration;
        [SerializeField] private StickMenConfiguration[] rifleStickMenConfiguration;
        [SerializeField] private StickMenConfiguration[] shotgunStickMenConfiguration;
        [SerializeField] private StickMenConfiguration[] grenadeLauncherStickMenConfiguration;

        private Action ShootAction;

        private float _passedTime;
        private RaycastHit hit;

        private Transform _currentShootingTransform;

        #region Collider Properties
        private readonly Vector3 defaultColliderCenter = new Vector3(0, 0.25f, 0);
        private readonly Vector3 pistolColliderCenter = new Vector3(0, 0.25f, 0.25f);
        private readonly Vector3 rifleColliderCenter = new Vector3(0, 0.25f, 0.5f);
        private readonly Vector3 shotgunColliderCenter = new Vector3(0, 0.25f, 0.5f);
        private readonly Vector3 grenadeLauncherColliderCenter = new Vector3(0, 0.25f, 0.4f);

        private readonly Vector3 defaultColliderSize = new Vector3(0.5f, 1, 0.5f);
        private readonly Vector3 pistolColliderSize = new Vector3(0.5f, 1, 0.7f);
        private readonly Vector3 rifleColliderSize = new Vector3(0.5f, 1, 0.9f);
        private readonly Vector3 shotgunColliderSize = new Vector3(0.6f, 1, 0.9f);
        private readonly Vector3 grenadeLauncherColliderSize = new Vector3(0.6f, 1, 0.9f);
        #endregion


        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }
        private void OnEnable()
        {
            GameStateHandler.OnGameAwaitingStartState += SetInitialProperties;
            GameStateHandler.OnGameStartedState += SetInitialPose;
        }
        private void OnDisable()
        {
            GameStateHandler.OnGameStartedState -= SetInitialPose;
            GameStateHandler.OnGameAwaitingStartState -= SetInitialProperties;
        }
        private void SetInitialPose()
        {
            animator.SetTrigger("IsRunning");

        }
        private void SetInitialProperties()
        {
            AssignColliderPorperites(defaultColliderCenter, defaultColliderSize);
            AssignShootAction();
        }
        private void Update()
        {
            if(GameStateHandler.CurrentState != GameState.GameStarted) return;
            ShootLoop();
        }

        private void ShootLoop()
        {
            _passedTime += Time.deltaTime;
            if (_passedTime > _currentWeaponInfo.ShootInterval)
            {
                _passedTime = 0;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _currentWeaponInfo.ShootRange))
                    ShootAction?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            AddStickMan(other);
            HitDestructable(other);
            HitIndestructable(other);
        }
        private void AddStickMan(Collider other)
        {
            if (!other.gameObject.TryGetComponent<IStickAdded>(out var stickMan)) return;

            if (_stickManList.Count > 30)
            {
                Destroy(other.gameObject);
                GameManager.Instance.AddMoney(10);
                return;
            }
            _stickManList.Add(stickMan);
            _attachedStickManTransform.Add(other.transform);
            CheckIfShouldSwitch();
            stickMan.AddStickMan(transform);
            stickMan.RepositionStickMan(_currentStickMenConfiguration[_stickManList.Count].PoseIndex,
                _currentStickMenConfiguration[_stickManList.Count].LocalTransform, _currentStickMenConfiguration[_stickManList.Count].ColorList);
            animator.SetTrigger(PoseNames[0]);
            AssignShootAction();
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
                    _attachedStickManTransform.RemoveAt(_stickManList.Count - 1);
                    _stickManList.RemoveAt(_stickManList.Count - 1);
                    
                    continue;
                }
                GameIsFinished();
                break;
            }
            CheckIfShouldSwitch();
            AssignShootAction();
        }
        private void GameIsFinished()
        {
            animator.SetTrigger("IsDead");
            if (GameManager.Instance.HasWon)
            {
                GameStateHandler.ChangeState(GameState.GameWon);
            }
            else
            {
                GameStateHandler.ChangeState(GameState.GameLost);
            }
        }

        #region Gun Mode Switch and Configuration
        private void CheckIfShouldSwitch()
        {
            if(_stickManList.Count <= 0)
            {
                SwitchGunMode(GunMode.Idle);
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
                case GunMode.Idle:
                    _currentGunMode= GunMode.Idle;
                    animator.SetTrigger("IsRunning");
                    AssignColliderPorperites(defaultColliderCenter, defaultColliderSize);
                    break;
                case GunMode.Pistol:
                    AssignConfiguration(GunMode.Pistol, pistolInfo, pistolStickMenConfiguration, pistolColliderCenter, pistolColliderSize);
                    break;
                case GunMode.Rifle:
                    AssignConfiguration(GunMode.Rifle, rifleInfo, rifleStickMenConfiguration, rifleColliderCenter, rifleColliderSize);
                    break;
                case GunMode.Shotgun:
                    AssignConfiguration(GunMode.Shotgun, shotgunInfo, shotgunStickMenConfiguration, shotgunColliderCenter, shotgunColliderSize);
                    break;
                case GunMode.GrenadeLaucher:
                    AssignConfiguration(GunMode.GrenadeLaucher, grenadeLauncherInfo, grenadeLauncherStickMenConfiguration, grenadeLauncherColliderCenter, grenadeLauncherColliderSize);
                    break;
            }
            for (int i = 0; i < _stickManList.Count; i++)
            {
                _stickManList[i].RepositionStickMan(_currentStickMenConfiguration[i].PoseIndex, _currentStickMenConfiguration[i].LocalTransform, _currentStickMenConfiguration[i].ColorList);
            }
        }
        private void AssignConfiguration(GunMode gunMode, WeaponInfo weaponInfo, StickMenConfiguration[] stickmenConfiguration, Vector3 colliderCenter, Vector3 colliderSize)
        {
            _currentGunMode = gunMode;
            _currentWeaponInfo = weaponInfo;
            _currentStickMenConfiguration = stickmenConfiguration;
            AssignColliderPorperites(colliderCenter, colliderSize);
        }
        private void AssignColliderPorperites(Vector3 center, Vector3 size)
        {
            _collider.center = center;
            _collider.size = size;
        }
        #endregion

        #region Shoot Methods
        private void AssignShootAction()
        {
            switch (_currentGunMode)
            {
                case GunMode.Idle:
                    ShootAction = null;
                    break;
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
            GameObject spawnedBullet = Instantiate(bulletProjectile, pistolInfo.ShootingTransform.position, Quaternion.identity);
            spawnedBullet.GetComponent<BulletProjectile>().ShootBullet(new Vector3(0, 0, 20),1);
            for(int i = 1; i <= 4; i++)
            {
                if (i < _attachedStickManTransform.Count)
                {
                    _attachedStickManTransform[i].DOLocalMove((_attachedStickManTransform[i].transform.localPosition - Vector3.forward * 0.1f), 0.1f);
                    StartCoroutine(Co_StickRecoilReset(_attachedStickManTransform[i]));
                }
            }
        }

        private void RifleShoot()
        {
            
            GameObject spawnedBullet = Instantiate(bulletProjectile, rifleInfo.ShootingTransform.position, Quaternion.identity);
            spawnedBullet.GetComponent<BulletProjectile>().ShootBullet(new Vector3(0, 0, rifleInfo.BulletSpeed),rifleInfo.BulletDamage);

            for (int i = 2; i <= 8; i++)
            {
                if (i < _attachedStickManTransform.Count)
                {
                    _attachedStickManTransform[i].DOLocalMove((_attachedStickManTransform[i].transform.localPosition - Vector3.forward * 0.1f), 0.1f);
                    StartCoroutine(Co_StickRecoilReset(_attachedStickManTransform[i]));
                }
            }
        }
        private void ShotgunShoot()
        {
            for(int i = 0; i < UnityEngine.Random.Range(3,6); i++)
            {
                GameObject spawnedBullet = Instantiate(bulletProjectile, shotgunInfo.ShootingTransform.position, Quaternion.identity);
                spawnedBullet.GetComponent<BulletProjectile>().ShootBullet(new Vector3
                    (UnityEngine.Random.Range(-shotgunScatterAmount, shotgunScatterAmount), 
                    UnityEngine.Random.Range(-shotgunScatterAmount, shotgunScatterAmount), 
                    shotgunInfo.BulletSpeed),
                    shotgunInfo.BulletDamage);
            }

            for (int i = 2; i <= 10; i++)
            {
                if (i < _attachedStickManTransform.Count)
                {
                    _attachedStickManTransform[i].DOLocalMove((_attachedStickManTransform[i].transform.localPosition - Vector3.forward * 0.1f), 0.1f);
                    StartCoroutine(Co_StickRecoilReset(_attachedStickManTransform[i]));
                }
            }

        }
        private void GrenadeLauncherShoot()
        {
            GameObject spawnedBullet = Instantiate(explosiveProjectile, grenadeLauncherInfo.ShootingTransform.position, Quaternion.identity);
            spawnedBullet.GetComponent<ExplosiveProjectile>().ShootBullet(new Vector3(0, grenadeLauncherVerticalVelocity, grenadeLauncherInfo.BulletSpeed), grenadeLauncherInfo.BulletDamage);

            for (int i = 2; i <= 18; i++)
            {
                if (i < _attachedStickManTransform.Count)
                {
                    _attachedStickManTransform[i].DOLocalMove((_attachedStickManTransform[i].transform.localPosition - Vector3.forward * 0.1f), 0.1f);
                    StartCoroutine(Co_StickRecoilReset(_attachedStickManTransform[i]));
                }
            }
        }

        private IEnumerator Co_StickRecoilReset(Transform stickTransform)
        {
            yield return new WaitForSeconds(0.1f);
            stickTransform.DOLocalMove((stickTransform.transform.localPosition + Vector3.forward * 0.1f), 0.1f);
        }
        #endregion
    }

    #region Related enums, Interfaces and Structs
    public enum GunMode { Idle = 0, Pistol = 1, Rifle = 2, Shotgun = 3, GrenadeLaucher = 4 }
    public enum ColorList { Black = 0, White = 1, Red = 2, Green = 3, Blue = 4, Yellow = 5 }
    public interface IStickAdded
    {
        public void AddStickMan(Transform gunTransform);
        public void RepositionStickMan(int poseIndex, Transform newLocalTransform, ColorList colorList);
        public void RemoveStickMan();
    }
    [Serializable]
    public struct StickMenConfiguration
    {
        public Transform LocalTransform;
        public int PoseIndex;
        public ColorList ColorList;
    }
    [Serializable]
    public struct WeaponInfo
    {
        public int SwitchAmount;
        public float ShootInterval;
        public float ShootRange;
        public float BulletSpeed;
        public int BulletDamage;
        public Transform ShootingTransform;
    }
    #endregion
}