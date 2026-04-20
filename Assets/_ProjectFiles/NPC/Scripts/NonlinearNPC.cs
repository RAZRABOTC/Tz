using UnityEngine;

public class NonlinearNPC : BaseNPC
{
    private DialogueMessage _currentMessage;

    protected override void StartDialogue()
    {
        _isInDialogue = true;
        _currentMessage = _firstMessage;
        _dialogueUI.ShowDialogue();
        _dialogueUI.MessageDialogue(_currentMessage, OnAnswerSelected);
    }

    private void OnAnswerSelected(DialogueMessage nextMessage)
    {
        if (nextMessage == null)
        {
            ExitDialogue();
            return;
        }
        _currentMessage = nextMessage;
        _dialogueUI.MessageDialogue(_currentMessage, OnAnswerSelected);
    }

    protected override void ExitDialogue()
    {
        _isInDialogue = false;
        _dialogueUI.HideDialogue();
    }
}