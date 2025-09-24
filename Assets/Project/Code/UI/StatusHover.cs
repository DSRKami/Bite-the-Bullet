using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatusHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int statusIndex;
    public GameObject nameText;
    public GameObject descriptionText;
    public StatusUI statusUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hovering over status icon: " + statusIndex);

        nameText.SetActive(true);
        descriptionText.SetActive(true);

        var nameTMP = nameText.GetComponent<TextMeshProUGUI>();
        var descTMP = descriptionText.GetComponent<TextMeshProUGUI>();

        switch (statusIndex)
        {
            case 0: // Bleeding
                nameTMP.text = "Bleeding";
                descTMP.text = "Disables opponent's tooth effects for 2 turns.";
                break;
            case 1: // Gold Filling
                nameTMP.text = "Gold Filling";
                descTMP.text = "Guarantees revolver fires loaded teeth until gold is fired.";
                break;
            case 2: // Incisor Bonus
                nameTMP.text = "Incisor Bonus";
                descTMP.text = "If three incisors are loaded, deal +1 damage to enemy next turn.";
                break;
            // Add more cases if you have more statuses
            default:
                nameTMP.text = "Unknown";
                descTMP.text = "No description available.";
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nameText.SetActive(false);
        descriptionText.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
