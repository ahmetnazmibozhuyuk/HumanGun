using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace HumanGun.UI
{
    public class CanvasAnimations : MonoBehaviour
    {
        [SerializeField] private GameObject[] growAndRotateIndefinitelyElement;
        [SerializeField] private GameObject[] growBounceElement;
        [SerializeField] private GameObject[] leftRightMoveElements;
        [SerializeField] private Image[] fadeInElement;

        private Vector3 _rotation = new Vector3(0, 0, 360);

        private List<Vector3> leftRightSignInitialPosition;
        private void Awake()
        {
            leftRightSignInitialPosition = new List<Vector3>();
            for (int i = 0; i < leftRightMoveElements.Length; i++)
            {
                leftRightSignInitialPosition.Add(leftRightMoveElements[i].transform.position);
            }
        }

        private void OnEnable()
        {
            InitializeObjects();
            TriggerObjectAnimations();
        }
        private void OnDisable()
        {
            for (int i = 0; i < leftRightMoveElements.Length; i++)
            {
                DOTween.Kill(leftRightMoveElements[i].transform);
            }
        }
        private void InitializeObjects()
        {
            for (int i = 0; i < growAndRotateIndefinitelyElement.Length; i++)
            {
                growAndRotateIndefinitelyElement[i].transform.localScale = Vector3.zero;
            }
            for (int i = 0; i < growBounceElement.Length; i++)
            {
                growBounceElement[i].transform.localScale = Vector3.zero;
            }
            for (int i = 0; i < fadeInElement.Length; i++)
            {
                fadeInElement[i].GetComponent<Image>().DOFade(0, 0);
            }
            for (int i = 0; i < leftRightMoveElements.Length; i++)
            {
                leftRightMoveElements[i].transform.position = new Vector3(
                    leftRightSignInitialPosition[i].x - 100,
                    leftRightSignInitialPosition[i].y,
                    leftRightSignInitialPosition[i].z);
            }
        }
        private void RecursiveBounce(Transform objectTransform)
        {
            objectTransform.DOScale(Vector3.one * 0.2f, 0.6f).SetRelative().OnComplete(() =>
            {
                objectTransform.DOScale(Vector3.one * -0.2f, 0.4f).SetRelative().OnComplete(() =>
                {
                    RecursiveBounce(objectTransform);
                });
            });
        }
        private void TriggerObjectAnimations()
        {
            if (growAndRotateIndefinitelyElement.Length >= 0)
            {
                for (int i = 0; i < growAndRotateIndefinitelyElement.Length; i++)
                {
                    growAndRotateIndefinitelyElement[i].transform.DOScale(1, 1);
                    growAndRotateIndefinitelyElement[i].transform.transform.DOLocalRotate(_rotation, 5f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear).SetRelative();

                }
            }
            for (int i = 0; i < fadeInElement.Length; i++)
            {

                fadeInElement[i].DOFade(0.5f, 0.5f);
            }
            for (int i = 0; i < growBounceElement.Length; i++)
            {
                growBounceElement[i].transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
            }

            for (int i = 0; i < leftRightMoveElements.Length; i++)
            {
                leftRightMoveElements[i].transform.DOLocalMoveX(leftRightMoveElements[i].transform.localPosition.x + 200, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
        }
    }
}