using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; } = null;
    GameObject Bullets;
    Queue<Bullet> p_bulletPool = new Queue<Bullet>();
    Queue<Bullet> e_bulletPool = new Queue<Bullet>();

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Bullets = GameObject.Find("Bullets");

        Bullet[] p_bullet = new Bullet[10];
        Bullet[] e_bullet = new Bullet[10];

        for (int i = 0; i < 10; i++)
        {
            p_bullet[i] = Instantiate(Resources.Load<Bullet>("Prefab/PlayerBullet"), Bullets.transform);
            ReleaseBullet(p_bullet[i], Bullet.BulletType.PLAYER);

            e_bullet[i] = Instantiate(Resources.Load<Bullet>("Prefab/EnemyBullet"), Bullets.transform);
            ReleaseBullet(e_bullet[i], Bullet.BulletType.ENEMY);
        }
    }
    public Bullet GetBullet(Bullet.BulletType bulletType)
    {
        Bullet bullet;
        switch (bulletType)
        {
            case Bullet.BulletType.PLAYER:
                if (e_bulletPool.Count > 0)
                {
                    bullet = p_bulletPool.Dequeue();
                    bullet.gameObject.SetActive(true);
                    return bullet;
                }
                else
                {
                    bullet = Instantiate(Resources.Load<Bullet>("Prefab/PlayerBullet"), Bullets.transform);
                    return bullet;
                }
            case Bullet.BulletType.ENEMY:
                if (e_bulletPool.Count > 0)
                {
                    bullet = e_bulletPool.Dequeue();
                    bullet.gameObject.SetActive(true);
                    return bullet;
                }
                else
                {
                    bullet = Instantiate(Resources.Load<Bullet>("Prefab/EnemyBullet"), Bullets.transform);
                    return bullet;
                }
            default:
                return null;
        }
    }

    public void ReleaseBullet(Bullet bullet, Bullet.BulletType bulletType)
    {
        switch (bulletType)
        {
            case Bullet.BulletType.PLAYER:
                p_bulletPool.Enqueue(bullet);
                break;
            case Bullet.BulletType.ENEMY:
                e_bulletPool.Enqueue(bullet);
                break;
        }
        bullet.transform.position = Vector2.zero;
        bullet.gameObject.SetActive(false);
    }
}
