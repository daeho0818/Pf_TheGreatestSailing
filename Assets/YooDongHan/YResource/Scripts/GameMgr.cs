using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    #region Singleton.
    private static GameMgr _instance = null;

    public static GameMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    // 스테이지 클리어 징표? 체크 등 기능 추가하기. 
    public HashSet<Enum.TokenType> GameTokens => _getGameTokens;
    private HashSet<Enum.TokenType> _getGameTokens = new HashSet<Enum.TokenType>();
    private Queue<Enum.TokenType> _orderGameTokens = new Queue<Enum.TokenType>();
    public Queue<Enum.TokenType> OrderGameTokens { get => _orderGameTokens; set => _orderGameTokens = value; }

    public void GetToken(Enum.TokenType name)
    {
        _getGameTokens.Add(name);
        OrderGameTokens.Dequeue();
    }

    public void GameClear(Enum.TokenType name)
    {
        GetToken(name);
        LoadingSceneController.LoadScene("Voyage");
    }

    public void GameFail(Enum.TokenType name)
    {
        LoadingSceneController.LoadScene("Voyage");
    }

    private void mixOrderToken()
    {
        int tokenLength = Enum.TokenType.GetNames(typeof(Enum.TokenType)).Length;

        for (int i = 0; i < tokenLength;)
        {
            Enum.TokenType temp = (Enum.TokenType)(UnityEngine.Random.Range(0, Enum.TokenType.GetNames(typeof(Enum.TokenType)).Length));

            if (!OrderGameTokens.Contains(temp))
            {
                OrderGameTokens.Enqueue(temp);
                i++;
            }
        }
    }

    private void Start()
    {
        mixOrderToken();
    }
}
