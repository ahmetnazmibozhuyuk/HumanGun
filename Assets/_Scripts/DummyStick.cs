using HumanGun.GunRelated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanGun
{
    public class DummyStick : MonoBehaviour
    {
        [SerializeField] private bool validate;
        [SerializeField] private int poseIndex;
        [SerializeField] private Animator animator;
        private void OnValidate()
        {
            //animator.SetTrigger(GnHandler.PoseNames[poseIndex]);
            animator.Play("pose_0"+(poseIndex+1).ToString());
            animator.Update(Time.deltaTime);
        }
    }
}
