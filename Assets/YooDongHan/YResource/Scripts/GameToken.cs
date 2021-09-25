using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameToken : MonoBehaviour
{
    private Enum.TokenType _token;

    public Enum.TokenType Token { get => _token; set => _token = value; }

    public GameToken(Enum.TokenType token)
    {
        _token = token;
    }
}
