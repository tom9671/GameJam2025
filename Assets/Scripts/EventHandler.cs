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
    public bool byPassCondition;
    [Dropdown("events")] public int eventParameter;
    public eOperator pOperator;
    public float value;
    public bool checkAutomatically;
    [Header("Result")]
    public UnityEvent onConditionMet;
    public UnityEvent onConditionNotMet;
    [HideInInspector] public bool triggered;
    public ItemClassForDropdown[] acquireItems;

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
        if (em == null)
            em = FindFirstObjectByType<EventManager>();

        if (events != null)
        {
            foreach (Event ev in events)
            {
                ev.UpdateEvents(em.events);
                foreach(ItemClassForDropdown ic in ev.acquireItems)
                    ic.UpdateItems(em.events);
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
            if (!ev.triggered && ev.checkAutomatically)
            {
                if (ev.byPassCondition)
                {
                    TriggerEvent(ev);
                }
                else
                {
                    switch (ev.pOperator)
                    {
                        case eOperator.equals:
                            if (ev.value == em.ParameterValue(ev.eventParameter))
                                TriggerEvent(ev);
                            break;
                        case eOperator.less:
                            if (ev.value > em.ParameterValue(ev.eventParameter))
                                TriggerEvent(ev);
                            break;
                        case eOperator.greater:
                            if (ev.value < em.ParameterValue(ev.eventParameter))
                                TriggerEvent(ev);
                            break;
                    }
                }
            }
        }
    }

    void TriggerEvent(Event _ev)
    {
        foreach (ItemClassForDropdown it in _ev.acquireItems)
        {
            GameManager.gm.em.SetParameter(it.item, 1);
        }

        _ev.onConditionMet.Invoke();
        _ev.triggered = true;
    }

    public void TriggerCounterEvent(Event _ev)
    {
        _ev.onConditionNotMet.Invoke();
    }

    public void AcquireItem()
    {

    }
    
    public void ManualCheck()
    {
        foreach (Event ev in events)
        {
            if (!ev.triggered && !ev.checkAutomatically)
            {
                if (ev.byPassCondition)
                {
                    TriggerEvent(ev);
                }
                else
                {
                    switch (ev.pOperator)
                    {
                        case eOperator.equals:
                            if (ev.value == em.ParameterValue(ev.eventParameter))
                                TriggerEvent(ev);
                            else
                                TriggerCounterEvent(ev);
                            break;
                        case eOperator.less:
                            if (ev.value > em.ParameterValue(ev.eventParameter))
                                TriggerEvent(ev);
                            else
                                TriggerCounterEvent(ev);
                            break;
                        case eOperator.greater:
                            if (ev.value < em.ParameterValue(ev.eventParameter))
                                TriggerEvent(ev);
                            else
                                TriggerCounterEvent(ev);
                            break;
                    }
                }
            }
        }
    }

    public void CauseStuck() => GameManager.gm.CauseStuck();
}
