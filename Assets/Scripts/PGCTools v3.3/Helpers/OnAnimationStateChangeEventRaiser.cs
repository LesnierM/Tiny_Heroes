using System;
using UnityEngine;

namespace PGCTools.Helpers.Animator
{
    public class OnAnimationStateChangeEventRaiser : StateMachineBehaviour
    {
        public event Action OnAnimationEnded;
        public event Action OnAnimationStarted;
        public override void OnStateEnter(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
                OnAnimationStarted?.Invoke();
        }
        public override void OnStateExit(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
                OnAnimationEnded?.Invoke();
        }
    }
}