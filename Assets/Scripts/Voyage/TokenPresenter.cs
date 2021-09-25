using UnityEngine;

public class TokenPresenter : MonoBehaviour
{
    [SerializeField]
    private TokenView _view;

    private static bool _ended;

    private void Start()
    {
        if (GameMgr.Instance.GameTokens.Count >= 7 && !_ended)
        {
            LoadingSceneController.LoadScene("Ending");
            _ended = true;
        }

        _view.Apply(new TokenView.Entity(GameMgr.Instance.GameTokens.Count));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameMgr.Instance.GameTokens.Add(Enum.TokenType.Alpha);
            _view.Apply(new TokenView.Entity(GameMgr.Instance.GameTokens.Count));
        }
    }

}
