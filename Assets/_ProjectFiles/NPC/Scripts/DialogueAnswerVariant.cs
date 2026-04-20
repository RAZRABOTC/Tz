using UnityEngine;

[System.Serializable]
public class DialogueAnswer
{
    [field: SerializeField, TextArea] public string Text { get; private set; }
    [field: SerializeField] public DialogueMessage NextMessage { get; private set; }
}
