using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ChamberHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int chamberIndex;
    public bool isHovering;
    public ToothType toothType;
    public GameObject toothNameText;
    public GameObject toothDescriptionText;
    public Revolver revolverA;
    public Revolver revolverB;
    public GameManager gameManager;

    void Update()
    {
        if (gameManager.isPlayerATurn)
        {
            if (revolverA != null && chamberIndex < revolverA.chamberedTeeth.Length)
                toothType = revolverA.chamberedTeeth[chamberIndex];
        }
        else
        {
            if (revolverB != null && chamberIndex < revolverB.chamberedTeeth.Length)
                toothType = revolverB.chamberedTeeth[chamberIndex];
        }

        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        toothNameText.SetActive(isHovering);
        toothDescriptionText.SetActive(isHovering);

        UpdateToothText(toothType, toothNameText, toothDescriptionText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        toothNameText.SetActive(isHovering);
        toothDescriptionText.SetActive(isHovering);
    }

    public static void UpdateToothText(ToothType updatedToothType, GameObject nameTextObj, GameObject descTextObj)
    {

        var nameText = nameTextObj.GetComponent<TextMeshProUGUI>();
        var descText = descTextObj.GetComponent<TextMeshProUGUI>();

        switch (updatedToothType)
        {
            case ToothType.Incisor:
                nameText.text = "INCISOR";
                descText.text = "IF THREE INCISORS ARE LOADED, DEAL +1 DAMAGE TO ENEMY NEXT TURN.";
                break;
            case ToothType.Canine:
                nameText.text = "CANINE";
                descText.text = "ON HIT, CAUSES BLEED (DISABLES OPPONENT'S TOOTH EFFECTS FOR 2 TURNS).";
                break;
            case ToothType.Premolar:
                nameText.text = "PREMOLAR";
                descText.text = "ON HIT, FORCEFULLY SPINS OPPONENT'S REVOLVER CHAMBER, RESETTING ODDS.";
                break;
            case ToothType.Molar:
                nameText.text = "MOLAR";
                descText.text = "ON HIT, DEALS 3 DAMAGE.";
                break;
            case ToothType.GoldFilling:
                nameText.text = "GOLD FILLING";
                descText.text = "GUARANTEES REVOLVER FIRES LOADED TEETH UNTIL GOLD FILLING IS FIRED.";
                break;
            case ToothType.Wisdom:
                nameText.text = "WISDOM";
                descText.text = "ON HIT, CAUSES THE LOSS OF ONE LOADED TOOTH IN OPPONENT'S CHAMBER.";
                break;
            case ToothType.Blank:
                nameText.text = "BLANK";
                descText.text = "FIRES A BLANK SHOT. NO EFFECT.";
                break;
            default:
                nameText.text = "USED";
                descText.text = "THIS CHAMBER HAS BEEN FIRED.";
                break;
        }
    }
}
