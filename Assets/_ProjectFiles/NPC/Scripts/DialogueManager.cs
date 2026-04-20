using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DialogueUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Transform _buttonsContainer;
    [SerializeField] private Button[] _answerButtons;
    private PlayerController _playerController;

    [Inject]
    public void Construct(PlayerController playerController)
    {
        _playerController = playerController;
    }

    private void Start()
    {
        _dialoguePanel.SetActive(false);
    }

    public void MessageDialogue(DialogueMessage message, System.Action<DialogueMessage> onAnswerSelected)
    {
        ToggleDialogueRoutine(true);
        _dialogueText.text = message.Text;

        if (message.Answers != null && message.Answers.Length > 0)
        {
            for (int i = 0; i < message.Answers.Length && i < _answerButtons.Length; i++)
            {
                Button btn = _answerButtons[i];
                btn.gameObject.SetActive(true);
                TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
                btnText.text = message.Answers[i].Text;
                btn.onClick.RemoveAllListeners();
                int index = i;
                btn.onClick.AddListener(() => onAnswerSelected?.Invoke(message.Answers[index].NextMessage));
            }
        }
    }

    public void HideDialogue()
    {
        ToggleDialogueRoutine(false);
    }

    public void ShowDialogue()
    {
        ToggleDialogueRoutine(true);
    }

    private void ToggleDialogueRoutine(bool isOn)
    {
        _dialoguePanel.SetActive(isOn);
        _playerController.Toggle(!isOn);
        Cursor.lockState = isOn? CursorLockMode.None : CursorLockMode.Locked;
        HideAllButtons();
    }

    private void HideAllButtons()
    {
        foreach (var btn in _answerButtons)
        {
            if (btn != null) btn.gameObject.SetActive(false);
        }
    }
}