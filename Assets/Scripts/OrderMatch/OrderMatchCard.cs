using System;
using UnityEngine;
using UnityEngine.UI;

public class OrderMatchCard : MonoBehaviour
{
    public Action<int, OrderMatchCard> OnClick { get; set; }

    public int Order { get; set; }

    [field: SerializeField]
    private Image Image { get; set; }

    [field: SerializeField]
    private Button Button { get; set; }

    public Sprite Sprite
    {
        get => Image.sprite;
        set => Image.sprite = value;
    }

    private void Awake()
    {
        Button.onClick.AddListener(() => OnClick?.Invoke(Order, this));
    }

}
