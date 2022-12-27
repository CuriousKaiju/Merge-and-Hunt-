using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : StateMachineBehaviour
{
    [SerializeField] private float _timneUntilBored;
    [SerializeField] private int _numberOfIdleAnimationsCount;

    private bool _isBored;
    private float _idleTime;
    private int _boredAnimation;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isBored == false)
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timneUntilBored && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _boredAnimation = Random.Range(1, _numberOfIdleAnimationsCount + 1);
                _isBored = true;
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }

        animator.SetFloat("Blend", _boredAnimation, 0.2f, Time.deltaTime);
    }
    private void ResetIdle()
    {
        _isBored = false;
        _idleTime = 0;
        _boredAnimation = 0;

    }
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
