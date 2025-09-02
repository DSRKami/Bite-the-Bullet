using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            c.a = 1.0f;
            teethImage.color = c;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (teethImage != null)
        {
            Color c = teethImage.color;
            c.a = 0f;
            teethImage.color = c;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        toothUI.SetActive(false);
        gameManager.PlayTurn(PlayerAction.Pliers, toothType);
    }
}
