using HumanGun.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanGun.Control
{
    public class CamControl : MonoBehaviour
    {
        [Tooltip("Higher the value is, lower the delay will be.")]
        [SerializeField] private float cameraPositionDelay = 1;

        private void LateUpdate()
        {
            CamPosition();
        }
        private void CamPosition()
        {
            transform.position =  Vector3.Lerp(transform.position, GameManager.Instance.PlayerObject.transform.position, Time.deltaTime * cameraPositionDelay);
        }
    }
}
