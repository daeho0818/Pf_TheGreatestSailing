using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OrderMatch : MonoBehaviour
{
    [field: SerializeField]
    private int CardAmount { get; set; }

    [field: SerializeField]
    private DialogData DialogData { get; set; }

    [field: SerializeField]
    private OrderMatchRoundData RoundData { get; set; }

    [field: SerializeField]
    private OrderMatchCard CardPrefab { get; set; }

    [field: SerializeField]
    private DialogBox DialogBox { get; set; }

    [field: SerializeField]
    private RectTransform CardParent { get; set; }

    [field: SerializeField]
    private RectTransform SelectedCardParent { get; set; }

    [field: SerializeField]
    private Animator ParrotAnimator { get; set; }

    [field: SerializeField]
    private Text TimeText { get; set; }

    [field: SerializeField]
    private float ShuffleDelay { get; set; }

    [field: SerializeField]
    private float ShuffleDuration { get; set; }

    private List<OrderMatchCard> _cards = new List<OrderMatchCard>();

    private HashSet<int> _usedCards = new HashSet<int>();

    private Dictionary<int, OrderMatchCard> _selectedCardMap
        = new Dictionary<int, OrderMatchCard>();

    private List<int> _selectedCards = new List<int>();

    private int _currentRound;

    private bool _isClickable;

    private const string CardSpritePath = "OrderMatch/TrumpCards";

    private readonly int _parrotSurprisedHash = Animator.StringToHash("Surprised");

    private readonly int _parrotIdleHash = Animator.StringToHash("Idle");

    private Coroutine _roundCoroutine;


    private void Awake()
    {
        _currentRound = 0;
        _cards.Capacity = CardAmount;
        _selectedCards.Capacity = CardAmount;
        ObjectPool<OrderMatchCard>.Initialize(CardPrefab, CardAmount);
        var sprites = Resources.LoadAll<Sprite>(CardSpritePath);

        for (int i = 0; i < CardAmount; ++i)
        {
            var card = ObjectPool<OrderMatchCard>.Instance.Rent();
            card.Sprite = sprites[i];
            card.gameObject.SetActive(false);

            _cards.Add(card);
        }

        var dialog = DialogData.GetDialog("Start");
        DialogBox.ShowDialog(dialog.Quotes, () => StartRound(_currentRound));
    }

    private void OnDestroy()
    {
        ObjectPool<OrderMatchCard>.Instance.Dispose();
    }

    private void StartRound(int round, bool isSurprised = false)
    {
        _currentRound = round;
        if (isSurprised)
        {
            ParrotAnimator.SetTrigger(_parrotIdleHash);
        }

        var info = RoundData.GetRoundInfo(round);
        for (int i = 0; i < info.SampleAmount; ++i)
        {
            int random;
            do
            {
                random = Random.Range(0, _cards.Count);
            }
            while (_usedCards.Contains(random));
            _usedCards.Add(random);

            var card = _cards[random];
            card.Order = i;
            card.OnClick += SelectCard;
            card.transform.SetParent(CardParent);
            card.transform.localScale = Vector3.one;
            card.gameObject.SetActive(true);
        }

        _roundCoroutine = StartCoroutine(CoStartRound(info.DisplayTime, info.TimeLimit));
    }

    private IEnumerator CoStartRound(float displayTime, float timeLimit)
    {
        var elapsed = 0f;
        while (elapsed < displayTime)
        {
            TimeText.text = $"카드 섞기까지... {Mathf.FloorToInt(displayTime - elapsed)}";
            TimeText.enabled = true;
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        var delay = new WaitForSeconds(ShuffleDelay);
        var cardCount = _usedCards.Count;

        while (elapsed < ShuffleDuration)
        {
            foreach (var index in _usedCards)
            {
                var random = Random.Range(0, cardCount);

                var card = _cards[index];
                card.transform.SetSiblingIndex(random);
            }

            elapsed += ShuffleDelay;
            yield return delay;
        }

        _isClickable = true;
        elapsed = 0f;
        while (elapsed < timeLimit)
        {
            TimeText.text = $"제한시간... {Mathf.FloorToInt(timeLimit - elapsed)}";
            elapsed += Time.deltaTime;
            yield return null;
        }

        CheckCards();
    }

    private void SelectCard(int order, OrderMatchCard card)
    {
        if (!_isClickable)
        {
            return;
        }

        if (_selectedCardMap.ContainsKey(order))
        {
            card.transform.SetParent(CardParent);
            card.transform.localScale = Vector3.one;
            _selectedCardMap.Remove(order);
            _selectedCards.Remove(order);
        }
        else
        {
            card.transform.SetParent(SelectedCardParent);
            card.transform.localScale = Vector3.one;
            _selectedCardMap[order] = card;
            _selectedCards.Add(order);

            if (_selectedCards.Count == _usedCards.Count)
            {
                var quote = DialogData.GetRandomQuote("Ask");
                _isClickable = false;
                DialogBox.ShowConfirm(quote, CheckCards, () =>
                {
                    _isClickable = true;
                    DialogBox.Close();
                });
            }
        }
    }

    private void CheckCards()
    {
        if (_roundCoroutine != null)
        {
            StopCoroutine(_roundCoroutine);
            _roundCoroutine = null;
        }

        TimeText.enabled = false;
        _isClickable = false;
        var index = 0;
        var equals = _selectedCards.Count == _usedCards.Count && 
            _selectedCards.All(value => value == index++);
        Clear();

        if (equals)
        {
            var newRound = _currentRound + 1;
            if (newRound >= RoundData.Count)
            {
                var quote = DialogData.GetRandomQuote($"Win");
                DialogBox.Show(quote, () =>
                    GameMgr.Instance.GameClear(Enum.TokenType.Alpha));
                return;
            }
            
            var dialog = DialogData.GetDialog($"Correct_{newRound}");
            if (dialog.IsSurprised)
            {
                ParrotAnimator.SetTrigger(_parrotSurprisedHash);
            }
            DialogBox.ShowDialog(dialog.Quotes, () =>
                StartRound(newRound, dialog.IsSurprised));
        }
        else
        {
            Lose();
        }
    }

    private void Lose()
    {
        _isClickable = false;
        var dialog = DialogData.GetDialog("Lose");
        DialogBox.ShowDialog(dialog.Quotes, () =>
            GameMgr.Instance.GameFail(Enum.TokenType.Alpha));
    }

    private void Clear()
    {
        foreach (var index in _usedCards)
        {
            var card = _cards[index];
            card.OnClick = null;
            card.transform.SetParent(null);
            card.gameObject.SetActive(false);
        }

        _selectedCards.Clear();
        _selectedCardMap.Clear();
        _usedCards.Clear();
    }
}
