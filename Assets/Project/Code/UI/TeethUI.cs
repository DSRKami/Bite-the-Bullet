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
    public Animator pliersAnimator;

    [Header("Player Information")]
    public PlayerState PlayerA;
    public PlayerState PlayerB;
    public PlayerState currentPlayer;
    public Revolver revolverA;
    public Revolver revolverB;

    [Header("Teeth Information Display")]
    public GameObject toothNameText;
    public GameObject toothStatsText;
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

    void Update()
    { 
        currentPlayer = gameManager.isPlayerATurn ? PlayerA : PlayerB;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentPlayer.HasTooth(toothType))
        {
            if (teethImage != null)
            {
                Color c = teethImage.color;
                c.a = 0.1f;
                teethImage.color = c;
            }
        }
        else
        {
            if (teethImage != null)
            {
                teethImage.color = new Color(0f, 0f, 0f, 0.6f); // Pure black, fully transparent
            }
        }
        
        toothHovering = true;

        ChamberHover.UpdateToothText(toothType, toothNameText, toothStatsText, toothDescriptionText);
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
        if (currentPlayer.HasTooth(toothType))
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
    }
}
