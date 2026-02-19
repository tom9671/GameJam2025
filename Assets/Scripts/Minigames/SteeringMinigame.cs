using UnityEngine;
using UnityEngine.Events;

public class SteeringMinigame : MonoBehaviour
{
    public Vector2 shipPosClamp;
    public float maxHeight;
    public float speed;

    public Transform submarine;
    public UnityEvent onCompleteMinigame;

    bool won;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (won)
        {
            if(submarine.localPosition.y < maxHeight)
                submarine.localPosition += Vector3.up * speed * 0.2f * Time.deltaTime;
        }
        else
        {
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                submarine.localPosition += Vector3.right * speed * Time.deltaTime;
            }
            else if (Input.GetAxis("Horizontal") < -0.1f)
            {
                submarine.localPosition -= Vector3.right * speed * Time.deltaTime;
            }

            submarine.localPosition = new Vector3(Mathf.Clamp(submarine.localPosition.x, shipPosClamp.x, shipPosClamp.y), submarine.localPosition.y, submarine.localPosition.z);
            if (submarine.localPosition.x >= shipPosClamp.y)
            {
                won = true;
                GameManager.gm.em.SetParameter(GameManager.gm.stuck, 0);
                onCompleteMinigame.Invoke();
            }
        }
    }
}
