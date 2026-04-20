using UnityEngine;
using Zenject;

public abstract class BaseNPC : MonoBehaviour, IInteractable
{
    [SerializeField] protected DialogueMessage _firstMessage;
    [SerializeField] protected string _hintText = "Поговорить";

    protected DialogueUIManager _dialogueUI;
    protected InputHandler _inputHandler;
    protected bool _isInDialogue;

    [Inject]
    public void Construct(DialogueUIManager dialogueUI, InputHandler inputHandler)
    {
        _dialogueUI = dialogueUI;
        _inputHandler = inputHandler;
    }

    protected virtual void Update()
    {
        if (_isInDialogue && _inputHandler.IsInteractionPressed)
        {
            ExitDialogue();
        }
    }

    public virtual void Interact()
    {
        if (!_isInDialogue) StartDialogue();
    }

    protected virtual void StartDialogue()
    {
        _isInDialogue = true;
        _dialogueUI.ShowDialogue();
    }

    protected abstract void ExitDialogue();

    public string GetHintText() => _hintText;
}
