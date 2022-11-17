using HumanGun.GunRelated;
using UnityEngine;

namespace HumanGun.Tools
{
    public class DummyStick : MonoBehaviour
    {
        [SerializeField] private int poseIndex;
        [SerializeField] private Animator animator;

        [SerializeField] private Renderer dummyRenderer;
        [SerializeField] private ColorList color;

        [SerializeField] private Material redMat;
        [SerializeField] private Material greenMat;
        [SerializeField] private Material blueMat;
        [SerializeField] private Material blackMat;
        [SerializeField] private Material whiteMat;
        [SerializeField] private Material yellowMat;

        private void Awake()
        {
            DestroyMeshes();
        }
        private void OnValidate()
        {
            animator.Play("pose_0" + (poseIndex + 1).ToString());
            animator.Update(Time.deltaTime);
            dummyRenderer.material = StickMaterial();
        }
        private void DestroyMeshes()
        {
            Destroy(this);
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(1).gameObject);
        }
        private Material StickMaterial()
        {
            switch (color)
            {
                case ColorList.Blue:
                    return blueMat;
                case ColorList.White:
                    return whiteMat;
                case ColorList.Yellow:
                    return yellowMat;
                case ColorList.Red:
                    return redMat;
                case ColorList.Black:
                    return blackMat;
                case ColorList.Green:
                    return greenMat;
                default:
                    return whiteMat;
            }
        }
    }
}
