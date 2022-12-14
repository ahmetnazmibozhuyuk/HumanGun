using HumanGun.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanGun.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxControlSpeed;
        [SerializeField] private float maxForwardSpeed;
        [SerializeField] private float horizontalClampLimit;

        private float _hitDownPositionx;
        private float _offsetx;

        [SerializeField] private Vector3 initialPosition;
        private void Update()
        {
#if UNITY_EDITOR
            SetMouseControl();
#endif
            SetTouchControl();

            if (GameStateHandler.CurrentState != GameState.GameStarted) return;

            AssignMovement(_offsetx * Time.deltaTime * maxControlSpeed);
        }
        private void SetTouchControl()
        {
            if (Input.touchCount <= 0) return;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                FirstTouch();
                _hitDownPositionx = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                _offsetx = touch.position.x - _hitDownPositionx;
                _hitDownPositionx = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _offsetx = 0;
            }
        }
#if UNITY_EDITOR
        private void SetMouseControl()
        {
            if (Input.GetMouseButtonDown(0))
            {
                FirstTouch();
                _hitDownPositionx = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                _offsetx = Input.mousePosition.x - _hitDownPositionx;
                _hitDownPositionx = Input.mousePosition.x;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _offsetx = 0;
            }
        }
#endif
       

        private void AssignMovement(float xDisplacement)
        {
            transform.position = (new Vector3(
                Mathf.Clamp(transform.position.x + (250*xDisplacement)/Screen.width, -horizontalClampLimit, horizontalClampLimit),
                transform.position.y,
                transform.position.z + maxForwardSpeed*Time.deltaTime));
        }
        private void FirstTouch()
        {
            if(GameStateHandler.CurrentState == GameState.GameAwaitingStart)
            {
                GameStateHandler.ChangeState(GameState.GameStarted);
            }
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
