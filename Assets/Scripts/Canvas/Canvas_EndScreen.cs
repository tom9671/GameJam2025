using UnityEngine;
using UnityEngine.Events;

public class Canvas_EndScreen : MonoBehaviour
{
    public DialogueSequence[] dialogue;

    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onDialogueStart.Invoke();
        GameManager.gm.DisplayDialogue(dialogue, gameObject, "DialogueOver");
    }

    public void DialogueOver() => onDialogueOver.Invoke();
}
