using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Vector3 mousePos;

    public float panSpeed = 10;
    public Vector2 panClamp;

    Camera cam;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        if (mousePos.x <= 0 && cam.transform.position.x > panClamp.x)
        {
            cam.transform.position += -Vector3.right * panSpeed * Time.deltaTime;
        }
        else if(mousePos.x >= Screen.width && cam.transform.position.x < panClamp.y)
        {
            cam.transform.position += Vector3.right * panSpeed * Time.deltaTime;
        }

        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
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
