using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public float panSpeed = 10;
    public Vector2 panClamp;
    public RectTransform pivot;
    RectTransform thisRect;
    public Slider buoyancySlider;
    Canvas_GameHUD hud;

    Vector3 mousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisRect = GetComponent<RectTransform>();
        hud = FindObjectOfType<Canvas_GameHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        buoyancySlider.value = GameManager.gm.em.ParameterValue(GameManager.gm.buoyancy);


        /*
        panClamp = new Vector2(-thisRect.sizeDelta.x * 0.5f, thisRect.sizeDelta.x * 0.5f) * 10;
        mousePos = Input.mousePosition;
        if (mousePos.x >= (Screen.width - 60) && pivot.transform.position.x > (panClamp.x + transform.position.x))
        {
            pivot.transform.position += -Vector3.right * panSpeed * Time.deltaTime;
        }
        else if( mousePos.x <= 60 && pivot.transform.position.x < (panClamp.y + transform.position.x))
        {
            pivot.transform.position += Vector3.right * panSpeed * Time.deltaTime;
        }*/
    }

    public void ChangeBuoyancy(int _difference) => hud.ChangeBuoyancy(_difference);

    public void SelfDestruct() => GameManager.gm.SelfDestructEnding();
}
