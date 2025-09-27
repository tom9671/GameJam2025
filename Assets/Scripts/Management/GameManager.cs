using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

[System.Serializable]
public class StartGameParameters
{
    public float startDelay;
    public string[] dialogueOnStart;
}

[System.Serializable]
public class DialogueParams 
{
    public string name;
    public string[] dialogue;

    [HideInInspector] public int index;
}

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Vector3 mousePos;

    public float panSpeed = 10;
    public Vector2 panClamp;

    public StartGameParameters startGameParameters;
    public DialogueParams[] dialogueParameters;

    Camera cam;
    EventManager _em; public EventManager em { get { return _em; } }

    GameObject pauseCanvas;

    DropdownList<int> _dialogue; public DropdownList<int> dialogue { get { return _dialogue; } }
    [Dropdown("_dialogue")] public int dialogues;

    void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
        else if(gm != this)
        {
            Destroy(gameObject);
        }

        cam = FindFirstObjectByType<Camera>();
        _em = GetComponent<EventManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnValidate()
    {
        CheckIndeces();

        if (dialogueParameters != null && dialogueParameters.Length > 0)
        {
            _dialogue = new DropdownList<int>();
            for (int i = 0; i < dialogueParameters.Length; i++)
            {
                _dialogue.Add(dialogueParameters[i].name, dialogueParameters[i].index);
            }
        }
    }

    void CheckIndeces()
    {
        if (dialogueParameters != null && dialogueParameters.Length > 0)
        {
            //Checks that all internal indeces and names are unique
            int highest = 0;
            List<DialogueParams> allDialogue = new List<DialogueParams>();
            for (int i = 0; i < dialogueParameters.Length; i++)
            {
                //Collects point markers into a list and tracks the highest
                allDialogue.Add(dialogueParameters[i]);
                if (highest < dialogueParameters[i].index)
                    highest = dialogueParameters[i].index;
            }

            bool perfect;
            do
            {
                perfect = true;
                for (int i = 0; i < allDialogue.Count; i++)
                {
                    int checkMarker = allDialogue[i].index;
                    for (int j = i + 1; j < allDialogue.Count; j++)
                    {
                        if (checkMarker == allDialogue[j].index)
                        {
                            perfect = false;
                            highest++;
                            allDialogue[j].index = highest;
                            i = allDialogue.Count;
                            j = allDialogue.Count;
                        }
                    }
                }
            } while (!perfect);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        if (mousePos.x <= 10 && cam.transform.position.x > panClamp.x)
        {
            cam.transform.position += -Vector3.right * panSpeed * Time.deltaTime;
        }
        else if(mousePos.x >= (Screen.width - 10) && cam.transform.position.x < panClamp.y)
        {
            cam.transform.position += Vector3.right * panSpeed * Time.deltaTime;
        }

        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    IEnumerator GameStartSequence()
    {
        yield return new WaitForSeconds(startGameParameters.startDelay);

        if(startGameParameters.dialogueOnStart != null)
            DisplayDialogue(startGameParameters.dialogueOnStart);
    }

    public void DisplayDialogue(string[] dialogue)
    {
        Canvas_Dialogue newDialogue = Instantiate(Resources.Load("Canvas/" + "Canvas_Dialogue") as GameObject).GetComponent<Canvas_Dialogue>();
        newDialogue.Init(dialogue);
    }

    void Pause()
    {
        if(pauseCanvas == null)
        {
            pauseCanvas = Instantiate(Resources.Load("Canvas/" + "Canvas_Pause") as GameObject);
        }
        else
        {
            pauseCanvas.SetActive(!pauseCanvas.active);
        }
    }
}
