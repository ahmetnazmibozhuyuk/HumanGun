using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

namespace HumanGun.GunRelated
{
    [RequireComponent(typeof(Animator))]
    public class GunHandler : MonoBehaviour, IStickAdded
    {
        [SerializeField] private GameObject bulletProjectile;

        private Animator _animator;

        private GunMode _currentGunMode;

        private int _attachedStickManAmount;

        private readonly int _pistolSwitchAmount = 1;
        private readonly int _rifleSwitchAmount = 10;
        private readonly int _shotgunSwitchAmount = 20;

        private List<Transform> _stickManTransforms = new List<Transform>();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            
            Shoot();
        }
        public void AddStickMan(Transform stickManTransform)
        {
            CheckIfShouldSwitch();
        }
        public void RemoveStickMan(int stickManAmount)
        {
            CheckIfShouldSwitch();
        }

        #region Gun Mode Switch and Configuration
        private void CheckIfShouldSwitch()
        {
            if(_attachedStickManAmount <= 0) return;

            if(_attachedStickManAmount >= _shotgunSwitchAmount)
            {
                SwitchGunMode(GunMode.Shotgun);
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
        public void AddStickMan(Transform stickManTransform);
        public void RemoveStickMan(int stickManAmount);
    }
}
