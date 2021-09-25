using UnityEngine;

[CreateAssetMenu]
public class StackGameData : ScriptableObject
{
    [field: SerializeField]
    public int SuccessScore { get; private set; }
    [field: SerializeField]
    public int InitialLife { get; private set; }
}