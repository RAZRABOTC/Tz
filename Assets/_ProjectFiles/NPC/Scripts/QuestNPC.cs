using UnityEngine;
using UnityEngine.UI;

public class QuestNPC : BaseNPC
{
    [SerializeField] private Toggle _questToggle;
    [SerializeField] private ItemParameters[] _possibleItems;
    [SerializeField] private float _checkRadius = 2f;
    [SerializeField] private DialogueMessage _messageWithItem;
    [SerializeField] private DialogueMessage _dialogueAfterRequest;
    private ItemParameters _requiredItem;
    private bool _questCompleted;
    private bool _questAccepted;
    private string _initialMessage;

    private void Start()
    {
        _questToggle.gameObject.SetActive(false);
        _requiredItem = _possibleItems[Random.Range(0, _possibleItems.Length)];
        _initialMessage = _messageWithItem.Text;
        _messageWithItem.Text += _requiredItem.Name;
    }

    private void OnDisable()
    {
        _messageWithItem.Text = _initialMessage;
    }

    public override void Interact()
    {
        if (_questAccepted && !_questCompleted)
        {
            CheckForItemAround();
        }
        base.Interact();
    }

    protected override void StartDialogue()
    {
        _isInDialogue = true;
        _dialogueUI.ShowDialogue();
        _dialogueUI.MessageDialogue(_questCompleted? _dialogueAfterRequest : _firstMessage, OnAnswerSelected);
    }

    private void CheckForItemAround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _checkRadius);

        foreach (var col in colliders)
        {
            if (col.TryGetComponent<Item>(out var item))
            {
                if (item.Parameters == _requiredItem)
                {
                    CompleteQuest();
                    break;
                }
            }
        }
    }

    private void OnAnswerSelected(DialogueMessage nextMessage)
    {
        if (nextMessage == null)
        {
            if (!_questAccepted) GiveQuest();
            ExitDialogue();
            return;
        }
        _dialogueUI.MessageDialogue(nextMessage, OnAnswerSelected);
    }

    private void GiveQuest()
    {
        if (_questCompleted) return;
        _questAccepted = true;
        _questToggle.gameObject.SetActive(true);
        _questToggle.isOn = false;
    }

    private void CompleteQuest()
    {
        _questCompleted = true;
        _questAccepted = false;
        if (_questToggle != null) _questToggle.isOn = true;
    }

    protected override void ExitDialogue()
    {
        _isInDialogue = false;
        _dialogueUI.HideDialogue();
    }
}
