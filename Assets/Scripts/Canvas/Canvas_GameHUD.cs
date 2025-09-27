using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Canvas_GameHUD : MonoBehaviour
{
    GameManager gm;

    public float intervalMultiplier = 1;
    public TMP_Text rescueTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = GameManager.gm;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer(int _time)
    {
        StartCoroutine("CountDown", _time);
    }

    IEnumerator CountDown(int _time)
    {
        int timeLeft = _time;

        while(timeLeft > 0)
        {
            rescueTimer.text = GetTimeFromSeconds(timeLeft);
            yield return new WaitForSeconds(1 * (1f / intervalMultiplier));
            timeLeft--;
        }

        rescueTimer.text = "0:00";
        gm.RescuedEnding();
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
