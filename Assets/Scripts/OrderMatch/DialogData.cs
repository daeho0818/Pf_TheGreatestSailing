using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogData : ScriptableObject
{
    [Serializable]
    public struct Dialog
    {
        [field: SerializeField]
        public string Key { get; set; }

        [field: SerializeField]
        public List<string> Quotes { get; set; }

        [field: SerializeField]
        public bool IsSurprised { get; set; }
    }

    private void OnEnable()
    {
        _dialogMap = new Dictionary<string, Dialog>();

        foreach (var dialog in Dialogs)
        {
            _dialogMap[dialog.Key] = dialog;
        }
    }

    [field: SerializeField]
    private List<Dialog> Dialogs = null;

    private Dictionary<string, Dialog> _dialogMap;

    public Dialog GetDialog(string key) => _dialogMap[key];

    public string GetRandomQuote(string key)
    {
        var dialog = _dialogMap[key];
        var rnd = UnityEngine.Random.Range(0, dialog.Quotes.Count);
        return dialog.Quotes[rnd];
    }
}
