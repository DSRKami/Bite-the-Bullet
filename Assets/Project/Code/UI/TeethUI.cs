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
        Debug.Log("Clicked on tooth: " + toothType);

        toothUI.SetActive(false);
        PlayerState currentPlayer = gameManager.isPlayerATurn ? PlayerA : PlayerB;
        Revolver currentRevolver = gameManager.isPlayerATurn ? revolverA : revolverB;
        if (currentPlayer.Pluck(toothType))
        {
            if (currentRevolver.LoadTooth(toothType))
            {
                if (toothType == ToothType.GoldFilling)
                {
                    currentRevolver.hasGoldFillingLoaded = true;
                }
            }
            else
            {
                Debug.Log("Revolver is full, cannot load more teeth.");
            }
        }
        else
        {
            Debug.Log("Cannot pluck tooth: " + toothType);
        }
    }
}
