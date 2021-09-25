using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeManager : MonoBehaviour
{
    public Text countText;
    public Text taleCount;

    enum PlayerDir
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        NONE
    }
    PlayerDir playerDir = PlayerDir.NONE;

    Vector2 playerPos;
    List<Vector2> tales = new List<Vector2>();

    List<GameObject> items = new List<GameObject>();
    GameObject[,] Cells = new GameObject[7, 7];

    bool gameClear = false;
    bool gameOver = false;

    int count = 3;

    void Start()
    {
        taleCount.text = 1.ToString();
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                Cells[x, y] = GameObject.Find($"{x}, {y}");
            }
        }

        playerPos = new Vector2(3, 3);

        StartCoroutine(CountDown());

        InvokeRepeating("Move", 5, 0.25f);
        Invoke("SpawnItem", 5);
    }

    void Update()
    {
        if (tales.Count >= 14)
        {
            gameClear = true;
            GameMgr.Instance.GameClear(Enum.TokenType.Theta);
        }
        else if (!gameClear && gameOver)
        {
            CancelInvoke("InvokeRepeating");
            CancelInvoke("SpawnItem");
            Debug.Log("Game Over");
        }
        else
        {
            SetDir();
            Coloring();
            EatItem();
        }
    }

    IEnumerator CountDown()
    {
        while (count >= -1)
        {
            playerDir = PlayerDir.NONE;
            if (count > 0)
                countText.text = count.ToString();
            else if (count == 0)
                countText.text = "START!";
            else if (count == -1)
                countText.text = "";
            yield return new WaitForSeconds(1);
            count--;
        }

    }

    void SetDir()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerDir == PlayerDir.DOWN) return;
            playerDir = PlayerDir.UP;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (playerDir == PlayerDir.RIGHT) return;
            playerDir = PlayerDir.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (playerDir == PlayerDir.UP) return;
            playerDir = PlayerDir.DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (playerDir == PlayerDir.LEFT) return;
            playerDir = PlayerDir.RIGHT;
        }
    }

    private void Move()
    {
        for (int i = tales.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                tales[i] = playerPos;
            }
            else
            {
                tales[i] = tales[i - 1];
            }
        }
        switch (playerDir)
        {
            case PlayerDir.LEFT:
                playerPos.x--;
                break;
            case PlayerDir.RIGHT:
                playerPos.x++;
                break;
            case PlayerDir.UP:
                playerPos.y++;
                break;
            case PlayerDir.DOWN:
                playerPos.y--;
                break;
        }
        cSoundManager.Instance.AudioPlay(cSoundManager.Instance.snakeMove);

        foreach (Vector2 tale in tales)
        {
            if (tale == playerPos)
            {
                gameOver = true;
                GameMgr.Instance.GameFail(Enum.TokenType.Theta);
            }
        }
        if (playerPos.x < 0 || playerPos.x >= 7 || playerPos.y < 0 || playerPos.y >= 7)
        {
            gameOver = true;
            GameMgr.Instance.GameFail(Enum.TokenType.Theta);
        }
    }

    GameObject GetCell(Vector2 pos)
    {
        return Cells[(int)pos.x, (int)pos.y];
    }

    SpriteRenderer GetCellSpriteRenderer(Vector2 pos)
    {
        return GetCell(pos).GetComponent<SpriteRenderer>();
    }

    void Coloring()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                GetCellSpriteRenderer(new Vector2(x, y)).color = Color.white;
            }
        }

        foreach (GameObject item in items)
        {
            GetCellSpriteRenderer(item.transform.position).color = Color.green;
        }

        GetCellSpriteRenderer(playerPos).color = Color.red;
        foreach (Vector2 tale in tales)
        {
            GetCellSpriteRenderer(tale).color = Color.red;
        }
    }

    void SpawnItem()
    {
        float time = Random.Range(2, 5);
        if (items.Count <= 0)
        {
            GameObject item = new GameObject();

            item.transform.position = new Vector2(Random.Range(0, 7), Random.Range(0, 7));

            items.Add(item);
        }

        Invoke("SpawnItem", time);
    }

    void EatItem()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (playerPos == (Vector2)items[i].transform.position)
            {
                cSoundManager.Instance.AudioPlayOneShot(cSoundManager.Instance.snakeGetScore);
                tales.Add(playerPos);
                taleCount.text = (tales.Count + 1).ToString();
                items.RemoveAt(i);
            }
        }
    }
}
