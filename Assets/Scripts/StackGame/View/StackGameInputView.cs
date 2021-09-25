using System;
using UnityEngine;

public class StackGameInputView : MonoBehaviour
{
    public event Action OnMouseClick = () => { };

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick.Invoke();
        }
    }
}
