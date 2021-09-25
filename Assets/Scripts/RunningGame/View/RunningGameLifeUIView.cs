using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RunningGameLifeUIView : MonoBehaviour
{
    public readonly struct Entity
    {
        public int Life { get; }

        public Entity(int life)
        {
            Life = life;
        }
    }

    [SerializeField]
    private Transform _placeholder;
    [SerializeField]
    private RunningGameLifeUIElementView _template;

    private bool _initialized;

    public async Task ApplyAsync(Entity entity)
    {
        if (_initialized)
        {
            var elementView = _placeholder.GetComponentsInChildren<RunningGameLifeUIElementView>()[entity.Life];
            await elementView.AnimateAsync();
        }
        else
        {
            foreach (var _ in Enumerable.Repeat(0, entity.Life))
            {
                var instance = Instantiate(_template, _placeholder);
                instance.gameObject.SetActive(true);
            }

            _initialized = true;
        }
    }
}
