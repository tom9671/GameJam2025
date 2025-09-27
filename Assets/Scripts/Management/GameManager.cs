using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StartGameParameters
{
    public float startDelay;
    public string[] dialogueOnStart;
}

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Vector3 mousePos;

    public float panSpeed = 10;
    public Vector2 panClamp;

    public StartGameParameters startGameParameters;

    Camera cam;
    EventManager _em; public EventManager em { get { return _em; } }

    GameObject pauseCanvas;

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
