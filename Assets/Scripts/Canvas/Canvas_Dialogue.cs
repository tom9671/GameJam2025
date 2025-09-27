using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Canvas_Dialogue : MonoBehaviour
{
    Color invisColor = new Vector4(0, 0, 0, 0);

    GameManager gm;

    public string[] dialogue;
    AudioClip[] clips;
    public TMP_Text displayText;
    public float textInterval = 0.1f;
    float intervalMult;
    bool writing;

    int messageIndex;
    int characterIndex;

    void Start()
    {
        gm = GameManager.gm;
        if (!writing)
        {
            Init(dialogue);
        }
    }

    public void Init(string[] _dialogue)
    {
        gm = GameManager.gm;

        dialogue = _dialogue;

        if (dialogue.Length < 1 || dialogue[0] == "")
            Destroy(gameObject);
        else
        {
            Canvas_Dialogue[] previousDialoque = FindObjectsByType<Canvas_Dialogue>(FindObjectsSortMode.None);
            for (int i = 0; i < previousDialoque.Length; i++)
            {
                if (previousDialoque[i] != null && previousDialoque[i] != this)
                    previousDialoque[i].StopWriting();
            }

            StartWriting();
        }
    }

    public void Update()
    {
        if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Tab)))
        {
            if (!writing)
            {
                AdvanceText();
            }
        }
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.Tab))
            intervalMult = 0.2f;
        else
            intervalMult = 1;
    }

    void StartWriting()
    {
        displayText.text = "";
        writing = true;
        characterIndex = 0;
        StartCoroutine(WriteText());
    }

    IEnumerator WriteText()
    {
        //displayText.text += dialogue[messageIndex].Substring(characterIndex, 1); 
        displayText.text = "<color=white>" + dialogue[messageIndex].Substring(0, characterIndex) + "</color>";
        displayText.text += "<color=#ffffff00>" + dialogue[messageIndex].Substring(Mathf.Clamp(characterIndex, 0, dialogue[messageIndex].Length), dialogue[messageIndex].Length - characterIndex) + "</color>";

        yield return new WaitForSeconds(textInterval * intervalMult);

        characterIndex++;
        if (messageIndex < dialogue.Length && characterIndex == dialogue[messageIndex].Length)
        {
            int curIdx = messageIndex;

            SkipToEnd();
        }
        else if (writing)
        {
            StartCoroutine(WriteText());
        }
    }

    void SkipToEnd()
    {
        writing = false;
        displayText.text = dialogue[messageIndex];
    }

    void AdvanceText()
    {
        messageIndex++;
        if (messageIndex == dialogue.Length)
            StopWriting();
        else
        {
            displayText.text = "";
            writing = true;
            characterIndex = 0;
            StartCoroutine(WriteText());
        }
    }

    public void StopWriting()
    {
        writing = false;
        Destroy(gameObject, 5);
        Destroy(this);
    }
}
