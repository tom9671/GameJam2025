using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Widget_ItemListing : MonoBehaviour
{
    int itemIndex;
    TMP_Text displayText;
    Button unityButton;

    public void Init(int _itemIndex)
    {
        itemIndex = _itemIndex;

        displayText = GetComponentInChildren<TMP_Text>();
        displayText.text = GameManager.gm.em.ParameterName(itemIndex);

        unityButton = GetComponent<Button>();
        unityButton.interactable = (GameManager.gm.em.ParameterValue(itemIndex) == 1);
    }
}
