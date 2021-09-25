using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Yabawi : MonoBehaviour
{
    [Serializable]
    private struct RoundInfo
    {
        public int cupAmount;

        public int mixCount;

        public float difficultyMultiplier;

        public List<string> roundStartDialogs;

        public bool isSurprised;
    }

    [field: SerializeField]
    private RectTransform CupParent { get; set; }

    [field: SerializeField]
    private List<YabawiCup> Cups { get; set; }

    [field: SerializeField]
    private AnimationCurve YShakeCurve { get; set; }

    [field: SerializeField]
    private float YShakeBorder { get; set; }

    [field: SerializeField]
    private float ShakeDuration { get; set; }

    [field: SerializeField]
    private DialogBox Dialog { get; set; }

    [field: SerializeField]
    private Animator ParrotAnimator { get; set; }

    [field: SerializeField]
    private RoundInfo[] RoundInfos { get; set; }

    [field: SerializeField]
    private List<string> RoundDefeatDialogs { get; set; }

    [field: SerializeField]
    private string WinDialog { get; set; }

    [field: SerializeField]
    private string OneMoreChanceDialog { get; set; }

    [field: SerializeField]
    private string DefeatDialog { get; set; }

    private int _life;

    private int _currentRound;

    private int _dialogIndex;

    private int _parrotSurprisedHash = Animator.StringToHash("Surprised");

    private int _parrotIdleHash = Animator.StringToHash("Idle");

    private void Awake()
    {
        _life = 2;
        StartDialog(0);
    }

    private void StartDialog(int round)
    {
        if (round >= RoundInfos.Length)
        {
            Dialog.Show(WinDialog, () =>
                GameMgr.Instance.GameClear(Enum.TokenType.Kappa));
            return;
        }

        var roundInfo = RoundInfos[round];
        _dialogIndex = 0;
        _currentRound = round;

        if (roundInfo.isSurprised)
        {
            ParrotAnimator.SetTrigger(_parrotSurprisedHash);
        }

        ProgressDialog();
    }

    public void ProgressDialog()
    {
        var roundInfo = RoundInfos[_currentRound];

        if (_dialogIndex >= roundInfo.roundStartDialogs.Count)
        {
            if (roundInfo.isSurprised)
            {
                ParrotAnimator.SetTrigger(_parrotIdleHash);
            }

            Dialog.Close();
            StartRound(roundInfo);
            return;
        }

        Dialog.Show(roundInfo.roundStartDialogs[_dialogIndex], ProgressDialog);
        ++_dialogIndex;
    }

    private void StartRound(RoundInfo info)
    {
        InitializeCups(info.cupAmount);
        StartCoroutine(CoRandomlyShakeCup(info));
    }

    private void InitializeCups(int count)
    {
        var cupAreaWidth = CupParent.rect.width;
        var xDiff = cupAreaWidth / count;

        for (int i = 0; i < count; ++i)
        {
            var cupRectTransform = Cups[i].RectTransform;

            var x = xDiff * (i - count / 2f + 0.5f);
            var pos = new Vector2(x, cupRectTransform.anchoredPosition.y);

            cupRectTransform.anchoredPosition = pos;
            cupRectTransform.gameObject.SetActive(true);
        }
    }

    private IEnumerator CoRandomlyShakeCup(RoundInfo info)
    {
        yield return ChooseAndHighlightCup(info.cupAmount);

        var count = 0;
        while (count < info.mixCount)
        {
            var a = Random.Range(0, info.cupAmount);
            var b = Random.Range(0, info.cupAmount);

            while (a == b)
            {
                b = Random.Range(0, info.cupAmount);
            }

            var duration = ShakeDuration * Mathf.Pow(info.difficultyMultiplier, count);
            yield return CoShakeCup(a, b, duration);
            ++count;
        }

        foreach (var cup in Cups)
        {
            cup.OnClick = OnClickCup;
        }
    }

    private IEnumerator ChooseAndHighlightCup(int cupAmount)
    {
        var index = Random.Range(0, cupAmount);
        Cups[index].Highlight();
        yield return new WaitForSeconds(4f);
    }

    private IEnumerator CoShakeCup(int a, int b, float duration)
    {
        var elapsed = 0f;
        var cupA = Cups[a].RectTransform;
        var cupB = Cups[b].RectTransform;
        var aPos = cupA.anchoredPosition;
        var bPos = cupB.anchoredPosition;
        var isPositive = (Random.Range(0, 2) & 1) == 0;

        while (elapsed < duration)
        {
            var t = elapsed / duration;
            var yPos = YShakeCurve.Evaluate(t) * YShakeBorder;
            var yMul = isPositive ? 1 : -1;

            var xPos = Mathf.Lerp(aPos.x, bPos.x, t);
            cupA.anchoredPosition = new Vector2(xPos, yPos * yMul);

            xPos = Mathf.Lerp(bPos.x, aPos.x, t);
            cupB.anchoredPosition = new Vector2(xPos, yPos * -yMul);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cupA.anchoredPosition = bPos;
        cupB.anchoredPosition = aPos;
    }

    private void OnClickCup(bool win)
    {
        ClearAllCups();

        if (_currentRound >= RoundInfos.Length)
        {

            return;
        }

        if (win)
        {
            StartDialog(_currentRound + 1);
        }
        else
        {
            --_life;
            StartDefeatDialog();
        }
    }

    private void StartDefeatDialog()
    {
        var index = Random.Range(0, RoundDefeatDialogs.Count);
        Dialog.Show(RoundDefeatDialogs[index]);

        if (_life == 1)
        {
            Dialog.Push(OneMoreChanceDialog, RestartRound);
        }
        else if (_life == 0)
        {
            Dialog.Push(DefeatDialog, () => GameMgr.Instance.GameFail(Enum.TokenType.Kappa));
        }
    }

    private void RestartRound()
    {
        Dialog.Close();

        var roundInfo = RoundInfos[_currentRound];
        StartRound(roundInfo);
    }

    private void ClearAllCups()
    {
        foreach (var cup in Cups)
        {
            cup.Clear();
            cup.OnClick = null;
        }
    }
}
