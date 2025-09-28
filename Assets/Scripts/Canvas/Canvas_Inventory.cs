using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class ItemClassForDropdown 
{
    [Dropdown("events")] public int item;

    DropdownList<int> events;
    public void UpdateItems(DropdownList<int> _events)
    {
        events = _events;
    }
}

public class Canvas_Inventory : MonoBehaviour
{
    EventManager _em;

    public ItemClassForDropdown[] items;
    Widget_ItemListing[] itemWidgets;
    public Transform widgetGrid;

    DropdownList<int> events;

    public void Init()
    {
        if(itemWidgets == null)
        {
            itemWidgets = new Widget_ItemListing[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                itemWidgets[i] = Instantiate(Resources.Load("Widget/" + "Widget_ItemListing") as GameObject).GetComponent<Widget_ItemListing>();
                itemWidgets[i].transform.SetParent(widgetGrid);
                itemWidgets[i].transform.localScale = Vector3.one;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            itemWidgets[i].Init(items[i].item);
        }
    }

    void OnValidate()
    {
        if (_em == null)
            _em = FindFirstObjectByType<EventManager>();
        
        if (_em != null)
        {
            events = _em.events;
        }

        foreach(ItemClassForDropdown it in items)
        {
            it.UpdateItems(events);
        }
    }
}
