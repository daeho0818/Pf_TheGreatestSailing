using UnityEngine;
using UnityEngine.UI;

public class StackGameScoreUIView : MonoBehaviour
{
    public readonly struct Entity
    {
        public int Score { get; }

        public Entity(int score)
        {
            Score = score;
        }
    }

    [SerializeField]
    private Text _scoreText;

    public void Apply(Entity entity)
    {
        _scoreText.text = entity.Score.ToString();
    }
}
