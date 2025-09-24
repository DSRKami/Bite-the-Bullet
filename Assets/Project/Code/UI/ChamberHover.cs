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
    public GameObject toothStatsText;
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
            else toothType = ToothType.Used;
        }
        else
        {
            if (revolverB != null && chamberIndex < revolverB.chamberedTeeth.Length)
                toothType = revolverB.chamberedTeeth[chamberIndex];
            else toothType = ToothType.Used;
        }

        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        toothNameText.SetActive(isHovering);
        toothStatsText.SetActive(isHovering);
        toothDescriptionText.SetActive(isHovering);

        UpdateToothText(toothType, toothNameText, toothStatsText, toothDescriptionText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        toothNameText.SetActive(isHovering);
        toothStatsText.SetActive(isHovering);
        toothDescriptionText.SetActive(isHovering);
    }

    public static void UpdateToothText(ToothType updatedToothType, GameObject nameTextObj, GameObject statsTextObj, GameObject descTextObj)
    {

        var nameText = nameTextObj.GetComponent<TextMeshProUGUI>();
        var statsText = statsTextObj.GetComponent<TextMeshProUGUI>();
        var descText = descTextObj.GetComponent<TextMeshProUGUI>();

        switch (updatedToothType)
        {
            case ToothType.Incisor:
                nameText.text = "INCISOR";
                statsText.text = "DAMAGE: 2 | SELF-DAMAGE: 1";
                descText.text = "IF THREE INCISORS ARE LOADED, DEAL +1 DAMAGE TO ENEMY NEXT TURN.";
                break;
            case ToothType.Canine:
                nameText.text = "CANINE";
                statsText.text = "DAMAGE: 2 | SELF-DAMAGE: 2";
                descText.text = "ON HIT, CAUSES BLEED (DISABLES OPPONENT'S TOOTH EFFECTS FOR 2 TURNS).";
                break;
            case ToothType.Premolar:
                nameText.text = "PREMOLAR";
                statsText.text = "DAMAGE: 2 | SELF-DAMAGE: 1";
                descText.text = "ON HIT, FORCEFULLY SPINS OPPONENT'S REVOLVER CHAMBER, RESETTING ODDS.";
                break;
            case ToothType.Molar:
                nameText.text = "MOLAR";
                statsText.text = "DAMAGE: 6 | SELF-DAMAGE: 2";
                descText.text = "ON HIT, DEALS 3 DAMAGE.";
                break;
            case ToothType.GoldFilling:
                nameText.text = "GOLD FILLING";
                statsText.text = "DAMAGE: 2 | SELF-DAMAGE: 3";
                descText.text = "GUARANTEES REVOLVER FIRES LOADED TEETH UNTIL GOLD FILLING IS FIRED.";
                break;
            case ToothType.Wisdom:
                nameText.text = "WISDOM";
                statsText.text = "DAMAGE: 2 | SELF-DAMAGE: 2";
                descText.text = "ON HIT, CAUSES THE LOSS OF ONE LOADED TOOTH IN OPPONENT'S CHAMBER.";
                break;
            case ToothType.Blank:
                nameText.text = "BLANK";
                statsText.text = "DAMAGE: 0 | SELF-DAMAGE: 0";
                descText.text = "FIRES A BLANK SHOT. NO EFFECT.";
                break;
            case ToothType.Used:
                nameText.text = "USED";
                statsText.text = "DAMAGE: 0 | SELF-DAMAGE: 0";
                descText.text = "THIS CHAMBER HAS BEEN FIRED.";
                break;
        }
    }
}
