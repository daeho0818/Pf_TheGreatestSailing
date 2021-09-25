using System;
using UnityEngine;

public class DangerZoneElementView : MonoBehaviour
{
    public event Action OnBoxCollide = () => { };

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var boxView = collision.gameObject.GetComponent<BoxView>();
        if (boxView)
        {
            OnBoxCollide.Invoke();
        }
    }
}
