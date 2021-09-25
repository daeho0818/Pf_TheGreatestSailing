using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingManager : MonoBehaviour
{
    public static ShootingManager Instance { get; private set; } = null;

    public Text HpText;
    public Text ScoreText;

    [HideInInspector]
    public bool gameStart = false;
    public bool gameClear = false;
    public bool gameFail = false;
    public int score = 0;
    public Player player;
    public GameObject Explosion;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Vector2 targetPos = player.transform.position;
        CameraManager.Instance.ZoomCam(new Vector3(targetPos.x, targetPos.y, -10), 0.5f, 1.5f);

        Explosion = Resources.Load<GameObject>("Prefab/Explosion");
            
        StartCoroutine(SpawnEnemy());

        Invoke("GameStart", 1);
    }

    void GameStart()
    {
        gameStart = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            score = 3000;
        }
        ScoreText.text = "SCORE : " + score;
        if (score >= 3000)
        {
            gameClear = true;
            EnemyPool.Instance.ReleaseAll();
            Destroy(player);

            GameMgr.Instance.GameClear(Enum.TokenType.Gamma);
        }
    }

    IEnumerator SpawnEnemy()
    {
        float randomTime;
        while (true)
        {
            randomTime = Random.Range(1, 3);
            yield return new WaitForSeconds(randomTime);
            Enemy enemy = EnemyPool.Instance.GetEnemy();
            enemy.transform.position = new Vector2(11, Random.Range(-3, 6));

            if (gameClear || gameFail) yield break;
        }
    }
}
