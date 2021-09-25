using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        PLAYER,
        ENEMY
    }
    public BulletType bulletType;

    public Vector2 dir;
    public float speed;

    void Start()
    {
        switch (bulletType)
        {
            case BulletType.PLAYER:
                dir = new Vector2(1, 0);
                dir = dir.normalized;
                break;
            case BulletType.ENEMY:
                dir = new Vector2(-1, 0);
                dir = dir.normalized;
                break;
        }
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bound"))
        {
            switch (bulletType)
            {
                case BulletType.PLAYER:
                    BulletPool.Instance.ReleaseBullet(this, BulletType.PLAYER);
                    break;
                case BulletType.ENEMY:
                    BulletPool.Instance.ReleaseBullet(this, BulletType.ENEMY);
                    break;
            }
        }
    }
}
