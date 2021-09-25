using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public static BrickManager Instance { get; private set; } = null;

    [HideInInspector]
    public GameObject Board;
    public bool gameOver = false;
    public bool gameClear = false;
    public int BrickCount = 8 * 5;

    Ball[] Balls = new Ball[2];

    float speed = 5;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Balls[0] = GameObject.Find("Ball1").GetComponent<Ball>();
        Balls[1] = GameObject.Find("Ball2").GetComponent<Ball>();
    }

    void Update()
    {
        if (BrickCount <= 0)
        {
            Debug.Log("Game Clear!");
            gameClear = true;
            GameMgr.Instance.GameClear(Enum.TokenType.lota);
        }
        else if (Balls[0].destroy && Balls[1].destroy)
        {
            Debug.Log("Game Over!");
            gameOver = true;
            GameMgr.Instance.GameFail(Enum.TokenType.lota);
        }
        else
        {
            KeyEvent();
        }
    }

    void KeyEvent()
    {
        if (Input.GetKey(KeyCode.A) && Board.transform.position.x > -3.2f)
        {
            Board.transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) && Board.transform.position.x < 3.2f)
        {
            Board.transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }
}
