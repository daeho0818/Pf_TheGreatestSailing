using UnityEngine;
using UnityEngine.UI;

public class RunningGameTimeUIView : MonoBehaviour
{
    public readonly struct Entity
    {
        public float TimeSeconds { get; }

        public Entity(float timeSeconds)
        {
            TimeSeconds = timeSeconds;
        }
    }

    [SerializeField]
    private Text _timeText;

    public void Apply(Entity entity)
    {
        _timeText.text = $"{entity.TimeSeconds:0.#}s";
    }
}
