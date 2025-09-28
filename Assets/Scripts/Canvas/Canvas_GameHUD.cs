using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Canvas_GameHUD : MonoBehaviour
{
    GameManager gm;

    public int flashlightEffectiveness = 30;
    public Vector2 buoyancyClamp;
    float buoyancyChange;
    public TMP_Text rescueTimer;
    public TMP_Text debugDisplay;
    public GameObject bottomPanel;
    public Animator vignetteAnim;
    public Animator flashAnim;

    int rescueTime;
    int creatureTime;
    float buoyancy;
    int maxCreatureTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = GameManager.gm;
    }

    void FixedUpdate()
    {
        buoyancy = gm.em.ParameterValue(gm.buoyancy);
        buoyancy = Mathf.Clamp(buoyancy + buoyancyChange, buoyancyClamp.x, buoyancyClamp.y);
        gm.em.SetParameter(gm.buoyancy, buoyancy);

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
        maxCreatureTime = (int)gm.em.ParameterValue(gm.creatureTime);
        StartCoroutine("CountDownRescue");
        StartCoroutine("CountDownCreature");
    }

    IEnumerator CountDownRescue()
    {
        gm = GameManager.gm;

        rescueTime = (int)gm.em.ParameterValue(gm.rescueTime);

        while(rescueTime > 0)
        {
            while (gm.em.ParameterValue(gm.stuck) == 1)
            {
                gm.em.SetParameter(gm.buoyancy, 1);
                buoyancyChange = 0;
                yield return new WaitForSeconds(1);
            }

            buoyancy = gm.em.ParameterValue(gm.buoyancy);
            rescueTimer.text = GetTimeFromSeconds(rescueTime);
            yield return new WaitForSeconds(1 * (1f / buoyancy));
            rescueTime = (int)gm.em.ParameterValue(gm.rescueTime);
            rescueTime -= 1;
            gm.em.SetParameter(gm.rescueTime, rescueTime);
        }

        gm.RescuedEnding();
    }

    IEnumerator CountDownCreature()
    {
        gm = GameManager.gm;

        creatureTime = (int)gm.em.ParameterValue(gm.creatureTime);

        while (creatureTime > 0)
        {
            yield return new WaitForSeconds(1);
            creatureTime = (int)gm.em.ParameterValue(gm.creatureTime);
            vignetteAnim.SetFloat("Distance", ((float)creatureTime / 120f));
            creatureTime -= 1;
            gm.em.SetParameter(gm.creatureTime, creatureTime);
        }

        gm.KilledEnding();
    }

    string GetTimeFromSeconds(int _seconds)
    {
        int seconds = _seconds % 60;
        int minutes = (_seconds - seconds) / 60;

        if (seconds > 9)
            return minutes + ":" + seconds;
        else
            return minutes + ":0" + seconds;
    }

    public void UseFlashlight()
    {
        flashAnim.SetTrigger("Flash");
        int flashlightsLeft = (int)gm.em.ParameterValue(gm.flashlightUses);
        if(flashlightsLeft > 0)
        {
            flashlightsLeft--;
            gm.em.SetParameter(gm.flashlightUses, flashlightsLeft);
            float newCreatureTime = Mathf.Clamp(gm.em.ParameterValue(gm.creatureTime) + flashlightEffectiveness, 0, maxCreatureTime);
            gm.em.SetParameter(gm.creatureTime, newCreatureTime);
        }
    }

    public void Inventory() => gm.Inventory();

    public void ChangeBuoyancy(int _difference)
    {
        buoyancyChange += _difference * 2;
        buoyancyChange = Mathf.Clamp(buoyancyChange, -Mathf.Abs(_difference), Mathf.Abs(_difference));
    }

    public void EndCountDown()
    {
        rescueTimer.text = "0:00";
        StopCoroutine("CountDownRescue");
        StopCoroutine("CountDownCreature");
    }
}
