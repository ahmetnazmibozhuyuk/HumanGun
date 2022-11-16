using HumanGun.Managers;
using UnityEngine;

namespace HumanGun.Control
{
    public class CamControl : MonoBehaviour
    {
        [Tooltip("Higher the value is, lower the delay will be.")]
        [SerializeField] private float cameraPositionDelay = 1;

        [SerializeField] private Vector3 initialPosition;
        private void LateUpdate()
        {
            CamPosition();
        }
        private void CamPosition()
        {
            transform.position =  Vector3.Lerp(transform.position, GameManager.Instance.PlayerObject.transform.position, Time.deltaTime * cameraPositionDelay);
        }
        private void OnEnable()
        {
            GameStateHandler.OnGameAwaitingStartState += InitializePosition;
        }
        private void OnDisable()
        {
            GameStateHandler.OnGameAwaitingStartState -= InitializePosition;
        }

        private void InitializePosition()
        {

            transform.position = initialPosition;
        }
    }
}
