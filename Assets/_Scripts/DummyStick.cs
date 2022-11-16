using UnityEngine;

namespace HumanGun.Tools
{
    public class DummyStick : MonoBehaviour
    {
        [SerializeField] private int poseIndex;
        [SerializeField] private Animator animator;

        [SerializeField] private Renderer dummyRenderer;
        private void Awake()
        {
            Destroy(this);
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(1).gameObject);
        }
        private void OnValidate()
        {
            animator.Play("pose_0"+(poseIndex+1).ToString());
            animator.Update(Time.deltaTime);
        }
    }
}
