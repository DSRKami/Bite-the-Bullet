using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TeethUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Teeth Elements")]
    public Image teethImage;
    public GameObject toothUI;
    public ToothType toothType;

    [Header("Player Information")]
    public PlayerState PlayerA;
    public PlayerState PlayerB;
    public Revolver revolverA;
    public Revolver revolverB;

    [Header("Teeth Information Display")]
    public GameObject toothNameText;
    public GameObject toothDescriptionText;
    public static bool toothHovering = false;
    public GameManager gameManager;

    void Start()
    {
        if (teethImage == null)
        {
            teethImage = GetComponent<Image>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (teethImage != null)
        {
            Color c = teethImage.color;
            c.a = 0.1f;
            teethImage.color = c;
        }
        toothHovering = true;

        ChamberHover.UpdateToothText(toothType, toothNameText, toothDescriptionText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (teethImage != null)
        {
            Color c = teethImage.color;
            c.a = 0f;
            teethImage.color = c;
        }

        toothHovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (teethImage != null)
        {
            Color c = teethImage.color;
            c.a = 0f;
            teethImage.color = c;
        }

        toothHovering = false;
        gameManager.PlayTurn(PlayerAction.Pliers, toothType);
        toothUI.SetActive(false);
    }

    public void DropPliers()
    {
        toothUI.SetActive(false);
    }
}
