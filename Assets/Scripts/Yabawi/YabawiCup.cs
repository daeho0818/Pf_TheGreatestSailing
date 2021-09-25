using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class YabawiCup : MonoBehaviour
{
    public Action<bool> OnClick { get; set; }

    private bool IsHighlighted { get; set; }

    [field: SerializeField]
    private Image Image { get; set; }

    [field: SerializeField]
    private Button Button { get; set; }

    public RectTransform RectTransform => Image.rectTransform;

    private void Awake()
    {
        Button.onClick.AddListener(() => OnClick?.Invoke(IsHighlighted));
    }

    public void Highlight()
    {
        IsHighlighted = true;
        StartCoroutine(CoHighlight());
    }

    public void Clear()
    {
        IsHighlighted = false;
    }

    private IEnumerator CoHighlight()
    {
        foreach (var _ in Enumerable.Repeat(0, 4))
        {
            var tween = Image.DOColor(Color.black, 1f);
            yield return new WaitForSeconds(0.5f);
            tween.PlayBackwards();
            yield return new WaitForSeconds(0.5f);
        }

        Image.color = Color.white;
    }
}
