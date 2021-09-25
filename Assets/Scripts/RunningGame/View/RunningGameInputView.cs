using System;
using UnityEngine;

public class RunningGameInputView : MonoBehaviour
{
    public event Action<KeyCode> OnKeyDown = keyCode => { };
    public event Action<KeyCode> OnKeyUp = keyCode => { };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnKeyDown.Invoke(KeyCode.W);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnKeyDown.Invoke(KeyCode.S);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            OnKeyUp.Invoke(KeyCode.W);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            OnKeyUp.Invoke(KeyCode.S);
        }
    }
}
