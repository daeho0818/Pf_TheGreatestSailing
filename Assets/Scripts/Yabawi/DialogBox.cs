using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [field: SerializeField]
    private Button Button { get; set; }

    [field: SerializeField]
    private Button YesButton { get; set; }

    [field: SerializeField]
    private Button NoButton { get; set; }

    [field: SerializeField]
    private Text DialogText { get; set; }

    private Action _onClick = null;
    
    private Action _onFinished = null;

    private Action _onYes = null;

    private Action _onNo = null;

    private List<string> _dialogs = null;

    private int _currentIndex = 0;

    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
        YesButton.onClick.AddListener(() => _onYes?.Invoke());
        NoButton.onClick.AddListener(() => _onNo?.Invoke());
    }

    public void Show(string dialog, Action onFinished = null)
    {
        _onClick = onFinished;
        DialogText.text = dialog;

        Button.interactable = true;
        gameObject.SetActive(true);
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
    }

    public void ShowConfirm(string dialog, Action onYes, Action onNo)
    {
        _onYes = onYes;
        _onNo = onNo;
        DialogText.text = dialog;

        Button.interactable = false;
        gameObject.SetActive(true);
        YesButton.gameObject.SetActive(true);
        NoButton.gameObject.SetActive(true);
    }

    public void ShowDialog(List<string> dialogs, Action onFinished = null, bool closeOnFinish = true)
    {
        _onFinished = onFinished;
        _dialogs = dialogs;
        _currentIndex = 0;
        _onClick = () => ProgressDialog(closeOnFinish);

        ProgressDialog(closeOnFinish);
        gameObject.SetActive(true);
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
    }

    public void ProgressDialog(bool closeOnFinish)
    {
        Button.interactable = true;
        DialogText.text = _dialogs[_currentIndex];
        ++_currentIndex;

        if (_currentIndex >= _dialogs.Count)
        {
            _onClick = () => FinishDialog(closeOnFinish);
        }
    }

    public void Push(string dialog, Action onFinished = null)
    {
        _onClick = () => Show(dialog, onFinished);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void FinishDialog(bool close)
    {
        _onFinished?.Invoke();
        if (close)
        {
            Close();
        }
    }

    private void OnClick()
    {
        if (_onClick is null)
        {
            Close();
        }
        else
        {
            _onClick?.Invoke();
        }
    }
}
