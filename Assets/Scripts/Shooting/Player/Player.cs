using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public int hp;

    public float speed;

    bool invincibility = false;
    void Start()
    {
        hp = 3;
    }

    void Update()
    {
        ShootingManager.Instance.HpText.text = "HP : " + hp;
        if (hp <= 0)
        {
            Explosion();
            ShootingManager.Instance.gameFail = true;

            CameraManager.Instance.ZoomCam(transform.position, 0.5f, 3f);

            GameMgr.Instance.GameFail(Enum.TokenType.Gamma);
        }

        KeyEvent();
    }

    void KeyEvent()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0) && ShootingManager.Instance.gameStart)
        {
            InvokeRepeating("FireBullet", 0, 0.25f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("FireBullet");
        }
    }

    void FireBullet()
    {
        Bullet bullet = BulletPool.Instance.GetBullet(Bullet.BulletType.PLAYER);
        bullet.transform.position = transform.position;

        cSoundManager.Instance.AudioPlay(cSoundManager.Instance.shot);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bound"))
        {
            string Name = collision.name;
            switch (Name)
            {
                case "Bound_Up":
                    Debug.Log("UP");
                    transform.Translate(Vector2.down * speed * Time.deltaTime);
                    break;
                case "Bound_Down":
                    Debug.Log("DOWN");
                    transform.Translate(Vector2.up * speed * Time.deltaTime);
                    break;
                case "Bound_Left":
                    Debug.Log("LEFT");
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                    break;
                case "Bound_Right":
                    Debug.Log("RIGHT");
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                    break;
            }
        }

        if (collision.CompareTag("Enemy") && !invincibility)
        {
            CameraManager.Instance.ShakeCam(0.3f);
            hp--;
            EnemyPool.Instance.ReleaseEnemy(collision.GetComponent<Enemy>());
            StartCoroutine(Invincibility());
        }

        if (collision.CompareTag("EnemyBullet") && !invincibility)
        {
            CameraManager.Instance.ShakeCam(0.3f);
            hp--;
            BulletPool.Instance.ReleaseBullet(collision.GetComponent<Bullet>(), Bullet.BulletType.ENEMY);
            StartCoroutine(Invincibility());
        }
    }

    IEnumerator Invincibility()
    {
        invincibility = true;
        yield return new WaitForSeconds(3);
        invincibility = false;
    }

    void Explosion()
    {
        Debug.Log("Explosion");
        Instantiate(ShootingManager.Instance.Explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
