using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public int hp;

    public float speed;
    Vector2 dir;
    private void Start()
    {
    }
    void OnEnable()
    {
        dir = new Vector2(-1, 0);
        hp = 2;
        InvokeRepeating("FireBullet", 0, 1);
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if (hp <= 0)
        {
            Instantiate(ShootingManager.Instance.Explosion, transform.position, Quaternion.identity);
            ShootingManager.Instance.score += 100;
            cSoundManager.Instance.AudioPlay(cSoundManager.Instance.shotExplosion);
            EnemyPool.Instance.ReleaseEnemy(this);
        }
    }

    void FireBullet()
    {
        Bullet bullet = BulletPool.Instance.GetBullet(Bullet.BulletType.ENEMY);
        bullet.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bound"))
        {
            if (collision.name == "Bound_Left")
            {
                Invoke("DestroyThis", 0.5f);
            }
        }

        if (collision.CompareTag("PlayerBullet"))
        {
            hp--;
            BulletPool.Instance.ReleaseBullet(collision.GetComponent<Bullet>(), Bullet.BulletType.PLAYER);
        }
    }

    void DestroyThis()
    {
        CameraManager.Instance.ShakeCam(0.3f);
        ShootingManager.Instance.player.hp--;
        EnemyPool.Instance.ReleaseEnemy(this);
    }
}
