using UnityEngine;

public class Canvas_EndScreen : MonoBehaviour
{
    public string[] dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.gm.DisplayDialogue(dialogue);
    }
}
