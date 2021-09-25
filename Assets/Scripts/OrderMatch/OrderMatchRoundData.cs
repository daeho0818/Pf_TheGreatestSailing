using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OrderMatchRoundData : ScriptableObject
{
    [Serializable]
    public struct RoundInfo
    {
        [field: SerializeField]
        public int SampleAmount { get; private set; }

        [field: SerializeField]
        public float DisplayTime { get; private set; }

        [field: SerializeField]
        public float TimeLimit { get; private set; }
    }

    [field: SerializeField]
    private List<RoundInfo> RoundInfos = null;

    public int Count => RoundInfos.Count;

    public RoundInfo GetRoundInfo(int round) => RoundInfos[round];
}
