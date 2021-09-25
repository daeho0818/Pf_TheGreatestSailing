using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector]
    public bool destroy = false;
    float speed = 5;
    Vector2 dir;
    void Start()
    {
        dir = new Vector2(0, -1).normalized;
    }

    void Update()
    {
        if (transform.position.x >= 4.5f) transform.position = new Vector2(4.5f, transform.position.y);
        if (!BrickManager.Instance.gameClear && !BrickManager.Instance.gameOver)
            transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bound"))
        {
            string Name = collision.name;
            switch (Name)
            {
                case "Bound_Left":
                    dir.x *= -1;
                    break;
                case "Bound_Right":
                    dir.x *= -1;
                    break;
                case "Bound_Up":
                    dir.y *= -1;
                    break;
                case "Bound_Down":
                    destroy = true;
                    return;
            }
        }

        if (collision.CompareTag("Board"))
        {
            Debug.Log("Bound Collider");
            dir.y *= -1;
        }

        if (collision.CompareTag("Brick"))
        {
            BrickManager.Instance.BrickCount--;
            Destroy(collision.gameObject);
            dir = (transform.position - collision.transform.position).normalized;
            cSoundManager.Instance.AudioPlay(cSoundManager.Instance.brickBreak);
        }
    }
}
