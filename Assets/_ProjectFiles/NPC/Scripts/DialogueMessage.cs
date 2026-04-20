using UnityEngine;

[CreateAssetMenu(fileName = "DialogueMessage", menuName = "DialogueMessage")]
public class DialogueMessage : ScriptableObject
{
    [field: SerializeField, TextArea] public string Text { get; set; }
    [field: SerializeField] public DialogueAnswer[] Answers { get; private set; }
}
