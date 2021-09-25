using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [field: SerializeField]
    private DialogData DialogData { get; set; }

    [field: SerializeField]
    private DialogBox DialogBox { get; set; }

    [field: SerializeField]
    private Animator ParrotAnimator { get; set; }

    private readonly int _parrotSurprisedHash = Animator.StringToHash("Surprised");

    private void Awake()
    {
        var dialog = DialogData.GetDialog("Start");
        DialogBox.ShowDialog(dialog.Quotes, Ask, false);
    }

    private void Ask()
    {
        var quote = DialogData.GetDialog("Ask").Quotes[0];
        DialogBox.ShowConfirm(quote, Yes, No);
    }

    private void Yes()
    {
        ParrotAnimator.SetTrigger(_parrotSurprisedHash);
        var dialog = DialogData.GetDialog("Yes");
        DialogBox.ShowDialog(dialog.Quotes, GameOver);
    }

    private void No()
    {
        ParrotAnimator.SetTrigger(_parrotSurprisedHash);
        var dialog = DialogData.GetDialog("No");
        DialogBox.ShowDialog(dialog.Quotes, GameOver);
    }

    private void GameOver()
    {
        LoadingSceneController.LoadScene("Voyage");
    }
}
