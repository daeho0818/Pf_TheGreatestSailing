using System.Collections.Generic;
using UnityEngine;

public class InitScene : MonoBehaviour
{
    public static Dictionary<Enum.IslandType, string> _islandDic = new Dictionary<Enum.IslandType, string>();

    private void Start()
    {
        _islandDic.Add(Enum.IslandType.Kappa, "Yabawi");
        _islandDic.Add(Enum.IslandType.Eta, "StackGame");
        _islandDic.Add(Enum.IslandType.lota, "BrickBreadk");
        _islandDic.Add(Enum.IslandType.Theta, "Snake");
        _islandDic.Add(Enum.IslandType.Zeta, "Shooting");
        _islandDic.Add(Enum.IslandType.Alpha, "Voyage");
        _islandDic.Add(Enum.IslandType.Epsilon, "RunningGame");
        _islandDic.Add(Enum.IslandType.Beta, "OrderMatch");
    }
}
