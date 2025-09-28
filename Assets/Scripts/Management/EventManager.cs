using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

[System.Serializable]
public class EventModification 
{

}

[System.Serializable]
public class EventParameter 
{
    public string name;
    public float currentValue;

    [HideInInspector] public int index;
}

public class EventManager : MonoBehaviour
{
    public EventParameter[] eventParameters;
    DropdownList<int> _events; public DropdownList<int> events { get { return _events; }}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnValidate()
    {
        CheckIndeces();

        if (eventParameters != null && eventParameters.Length > 0)
        {
            _events = new DropdownList<int>();
            for (int i = 0; i < eventParameters.Length; i++)
            {
                _events.Add(eventParameters[i].name, eventParameters[i].index);
            }
        }
    }

    void CheckIndeces()
    {
        if (eventParameters != null && eventParameters.Length > 0)
        {
            //Checks that all internal indeces and names are unique
            int highest = 0;
            List<EventParameter> allEvents = new List<EventParameter>();
            for (int i = 0; i < eventParameters.Length; i++)
            {
                //Collects point markers into a list and tracks the highest
                allEvents.Add(eventParameters[i]);
                if (highest < eventParameters[i].index)
                    highest = eventParameters[i].index;
            }

            bool perfect;
            do
            {
                perfect = true;
                for (int i = 0; i < allEvents.Count; i++)
                {
                    int checkMarker = allEvents[i].index;
                    for (int j = i + 1; j < allEvents.Count; j++)
                    {
                        if (checkMarker == allEvents[j].index)
                        {
                            perfect = false;
                            highest++;
                            allEvents[j].index = highest;
                            i = allEvents.Count;
                            j = allEvents.Count;
                        }
                    }
                }
            } while (!perfect);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float ParameterValue(int _index)
    {
        foreach(EventParameter ep in eventParameters)
        {
            if (_index == ep.index)
                return ep.currentValue;
        }

        return 0;
    }

    public string ParameterName(int _index)
    {
        foreach (EventParameter ep in eventParameters)
        {
            if (_index == ep.index)
                return ep.name;
        }

        return "";
    }

    public void SetParameter(int _index, float _value)
    {
        foreach (EventParameter ep in eventParameters)
        {
            if (_index == ep.index)
                ep.currentValue = _value;
        }
    }
}
