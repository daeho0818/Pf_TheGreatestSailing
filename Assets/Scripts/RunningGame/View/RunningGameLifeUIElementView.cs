using System.Threading.Tasks;
using UnityEngine;

public class RunningGameLifeUIElementView : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private AnimatorStateMachineBehaviour _behaviour;

    private void Awake()
    {
        _behaviour = _animator.GetBehaviour<AnimatorStateMachineBehaviour>();
    }

    public async Task AnimateAsync()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Empty")) return;

        _animator.SetTrigger("Animate");

        await _behaviour.OnStateExitAsync("Animate");
    }
}
