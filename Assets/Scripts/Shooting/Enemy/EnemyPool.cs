using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; } = null;

    GameObject Enemies;
    Stack<Enemy> enemyPool = new Stack<Enemy>();

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Enemies = GameObject.Find("Enemies");

        Enemy[] enemy = new Enemy[10];

        for (int i = 0; i < 10; i++)
        {
            enemy[i] = Instantiate(Resources.Load<Enemy>("Prefab/Enemy"), Enemies.transform);
            enemyPool.Push(enemy[i]);
        }
    }
    public Enemy GetEnemy()
    {
        Enemy enemy;
        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Pop();
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            if (!Enemies) Enemies = GameObject.Find("Enemies");
            enemy = Instantiate(Resources.Load<Enemy>("Prefab/Enemy"), Enemies.transform);
            return enemy;
        }
    }

    public void ReleaseEnemy(Enemy enemy)
    {
        enemyPool.Push(enemy);
        enemy.CancelInvoke("FireBullet");
        enemy.gameObject.SetActive(false);
    }

    public void ReleaseAll()
    {
        Transform Enemy;
        for (int i = 0; i < Enemies.transform.childCount; i++)
        {
            Enemy = Enemies.transform.GetChild(i);
            if (Enemy.gameObject.activeSelf)
            {
                ReleaseEnemy(Enemy.GetComponent<Enemy>());
            }
        }
    }
}
