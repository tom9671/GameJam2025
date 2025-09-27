using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public enum eOperator { equals, less, greater}

[System.Serializable]
public class Event 
{
    [Header("Condition")]
    [Dropdown("events")] public int eventParameter;
    public eOperator pOperator;
    public float value;
    [Header("Result")]
    public UnityEvent onConditionMet;

    DropdownList<int> events;
    public void UpdateEvents(DropdownList<int> _events)
    {
        events = _events; 
    }
}

public class EventHandler : MonoBehaviour
{
    EventManager em;

    public Event[] events;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        em = GameManager.gm.em;

        StartCoroutine("LazyCheck");
    }

    void OnValidate()
    {
        if(em == null)
            em = FindFirstObjectByType<GameManager>().GetComponent<EventManager>();

        if (em == null)
            Debug.Log("Not found");

        if (events != null)
        {
            foreach (Event ev in events)
            {
                ev.UpdateEvents(em.events);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LazyCheck()
    {
        while (true)
        {
            CheckEvents();

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void CheckEvents()
    {
        foreach (Event ev in events)
        {
            switch (ev.pOperator) 
            {
                case eOperator.equals:
                    if (ev.value == em.ParameterValue(ev.eventParameter))
                    {
                        ev.onConditionMet.Invoke();
                    }
                    break;
                case eOperator.less:
                    if (ev.value > em.ParameterValue(ev.eventParameter))
                        ev.onConditionMet.Invoke();
                    break;
                case eOperator.greater:
                    if (ev.value < em.ParameterValue(ev.eventParameter))
                        ev.onConditionMet.Invoke();
                    break;
            }
        }
    }

    public void Add()
    {

    }
}
