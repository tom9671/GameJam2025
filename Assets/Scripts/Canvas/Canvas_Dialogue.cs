using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class DialogueSequence 
{
    public string dialogue;
    public string characterName;
    public Sprite displayImage;
}

public class Canvas_Dialogue : MonoBehaviour
{
    Color invisColor = new Vector4(0, 0, 0, 0);

    GameManager gm;
    Animator anim;

    DialogueSequence[] dialogue;
    public TMP_Text displayText;
    public TMP_Text characterNameText;
    public Image[] transitionImages = new Image[2];
    public float textInterval = 0.1f;
    float intervalMult;
    bool writing;

    int messageIndex;
    int characterIndex;

    void Start()
    {
        gm = GameManager.gm;
        anim = GetComponent<Animator>();
        if (!writing)
        {
            Init(dialogue);
        }
    }

    public void Init(DialogueSequence[] _dialogue)
    {
        gm = GameManager.gm;
        anim = GetComponent<Animator>();

        dialogue = _dialogue;

        if (dialogue.Length < 1 || dialogue[0].dialogue == "")
            Destroy(gameObject);
        else
        {
            Canvas_Dialogue[] previousDialoque = FindObjectsByType<Canvas_Dialogue>(FindObjectsSortMode.None);
            for (int i = 0; i < previousDialoque.Length; i++)
            {
                if (previousDialoque[i] != null && previousDialoque[i] != this)
                    previousDialoque[i].StopWriting();
            }

            anim.SetTrigger("Start");
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
        characterNameText.text = dialogue[messageIndex].characterName;
        writing = true;
        characterIndex = 0;
        StartCoroutine(WriteText());

        transitionImages[1].sprite = transitionImages[0].sprite;
        transitionImages[0].sprite = dialogue[messageIndex].displayImage;
        if (transitionImages[0].sprite != null)
            anim.SetTrigger("Enter");
        else if (transitionImages[1].sprite != null)
            anim.SetTrigger("Exit");
    }

    IEnumerator WriteText()
    {
        //displayText.text += dialogue[messageIndex].Substring(characterIndex, 1); 
        displayText.text = "<color=white>" + dialogue[messageIndex].dialogue.Substring(0, characterIndex) + "</color>";
        displayText.text += "<color=#ffffff00>" + dialogue[messageIndex].dialogue.Substring(Mathf.Clamp(characterIndex, 0, 
            dialogue[messageIndex].dialogue.Length), dialogue[messageIndex].dialogue.Length - characterIndex) + "</color>";

        yield return new WaitForSeconds(textInterval * intervalMult);

        characterIndex++;
        if (messageIndex < dialogue.Length && characterIndex == dialogue[messageIndex].dialogue.Length)
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
        displayText.text = dialogue[messageIndex].dialogue;
    }

    void AdvanceText()
    {
        messageIndex++;
        if (messageIndex == dialogue.Length)
            StopWriting();
        else
        {
            StartWriting();
        }
    }

    public void StopWriting()
    {
        writing = false;
        anim.SetTrigger("End");
        Destroy(gameObject, 5);
        Destroy(this);
    }
}
