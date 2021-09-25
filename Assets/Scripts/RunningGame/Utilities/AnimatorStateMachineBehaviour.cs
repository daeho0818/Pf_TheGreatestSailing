using System;
using System.Threading.Tasks;
using UnityEngine;

public class AnimatorStateMachineBehaviour : StateMachineBehaviour
{
    public Action<AnimatorStateInfo> OnStateEnterTrigger = info => { };
    public Action<AnimatorStateInfo> OnStateExitTrigger = info => { };


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateEnterTrigger.Invoke(stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateExitTrigger.Invoke(stateInfo);
    }

    public async Task<AnimatorStateInfo> OnStateEnterAsync(string stateName)
    {
        var completionSource = new TaskCompletionSource<AnimatorStateInfo>();

        Action<AnimatorStateInfo> onStateEnter = default;

        onStateEnter = info =>
        {
            if (info.shortNameHash == Animator.StringToHash(stateName))
            {
                completionSource.SetResult(info);
                OnStateEnterTrigger -= onStateEnter;
            }
        };

        OnStateEnterTrigger += onStateEnter;

        return await completionSource.Task;
    }

    public async Task<AnimatorStateInfo> OnStateExitAsync(string stateName)
    {
        var completionSource = new TaskCompletionSource<AnimatorStateInfo>();

        Action<AnimatorStateInfo> onStateExit = default;

        onStateExit = info =>
        {
            if (info.shortNameHash == Animator.StringToHash(stateName))
            {
                completionSource.SetResult(info);
                OnStateExitTrigger -= onStateExit;
            }
        };

        OnStateExitTrigger += onStateExit;

        return await completionSource.Task;
    }
}
