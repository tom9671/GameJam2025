using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Canvas_GameHUD : MonoBehaviour
{
    GameManager gm;

    public float intervalMultiplier = 1;
    public TMP_Text rescueTimer;
    public TMP_Text debugDisplay;
    public GameObject bottomPanel;

    int rescueTime;
    int creatureTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = GameManager.gm;
    }

    void FixedUpdate()
    {
        debugDisplay.text = "Rescue Time: " + rescueTime + "\n" +
            "Creature Time: " + creatureTime + "\n";

        if (!gm.gameOver)
        {
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.y <= 30)
                bottomPanel.SetActive(true);
            else if (mousePos.y > 100)
                bottomPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer()
    {
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        gm = GameManager.gm;

        rescueTime = (int)gm.em.ParameterValue(gm.rescueTime);
        creatureTime = (int)gm.em.ParameterValue(gm.creatureTime);

        while(rescueTime > 0 && creatureTime > 0)
        {
            rescueTimer.text = GetTimeFromSeconds(rescueTime);
            yield return new WaitForSeconds(1 * (1f / intervalMultiplier));
            rescueTime = (int)gm.em.ParameterValue(gm.rescueTime);
            creatureTime = (int)gm.em.ParameterValue(gm.creatureTime);
            rescueTime -= 1;
            creatureTime -= 1;
            gm.em.SetParameter(gm.rescueTime, rescueTime);
            gm.em.SetParameter(gm.creatureTime, creatureTime);
        }

        rescueTimer.text = "0:00";
        if(rescueTime <= 0)
            gm.RescuedEnding();
        else
            gm.KilledEnding();
    }

    public string GetTimeFromSeconds(int _seconds)
    {
        int seconds = _seconds % 60;
        int minutes = (_seconds - seconds) / 60;

        if (seconds > 9)
            return minutes + ":" + seconds;
        else
            return minutes + ":0" + seconds;
    }
}
