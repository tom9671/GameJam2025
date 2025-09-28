using UnityEngine;
using UnityEngine.Events;

public class WeldingMinigame : MonoBehaviour
{
    public Transform welder;
    public Vector2 gameDimensions;

    public int totalCracks;
    int fixedCracks;

    public UnityEvent onCompleteMinigame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        welder.position = mousePos;
        Vector3 newPos = welder.localPosition;
        newPos.x = Mathf.Clamp(newPos.x, -gameDimensions.x, gameDimensions.x);
        newPos.y = Mathf.Clamp(newPos.y, -gameDimensions.y, gameDimensions.y);
        welder.localPosition = newPos;
    }

    public void FixCrack()
    {
        fixedCracks++;
        if(fixedCracks >= totalCracks)
        {
            GameManager.gm.em.SetParameter(GameManager.gm.leak, 0);
            onCompleteMinigame.Invoke();
        }
    }
}
