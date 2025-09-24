using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public GameObject[] statusIconsUI; // UI GameObjects with Image components
    public Sprite[] statusIcons;       // Sprites for each status effect

    [Header("Player Information")]
    public PlayerState playerA;
    public PlayerState playerB;
    public Revolver revolverA;
    public Revolver revolverB;
    private Revolver revolverCurrent;
    private PlayerState currentPlayer;
    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayer = gameManager.isPlayerATurn ? playerA : playerB;
        revolverCurrent = gameManager.isPlayerATurn ? revolverA : revolverB;

        // Track active statuses
        int iconIndex = 0;

        // Bleeding status
        if (currentPlayer.isBleeding && iconIndex < statusIconsUI.Length)
        {
            statusIconsUI[iconIndex].SetActive(true);
            statusIconsUI[iconIndex].GetComponent<Image>().sprite = statusIcons[0]; // Bleeding sprite
            statusIconsUI[iconIndex].GetComponent<StatusHover>().statusIndex = 0;   // Set index for hover
            iconIndex++;
        }

        // Gold Filling status
        if (revolverCurrent.hasGoldFillingLoaded && iconIndex < statusIconsUI.Length)
        {
            statusIconsUI[iconIndex].SetActive(true);
            statusIconsUI[iconIndex].GetComponent<Image>().sprite = statusIcons[1]; // Gold Filling sprite
            statusIconsUI[iconIndex].GetComponent<StatusHover>().statusIndex = 1;   // Set index for hover
            iconIndex++;
        }

        // Incisor Bonus status
        if (revolverCurrent.hasIncisorBonus && iconIndex < statusIconsUI.Length)
        {
            statusIconsUI[iconIndex].SetActive(true);
            statusIconsUI[iconIndex].GetComponent<Image>().sprite = statusIcons[2]; // Incisor Bonus sprite
            statusIconsUI[iconIndex].GetComponent<StatusHover>().statusIndex = 2;   // Set index for hover
            iconIndex++;
        }

        // Hide unused icons
        for (int i = iconIndex; i < statusIconsUI.Length; i++)
        {
            statusIconsUI[i].SetActive(false);
        }
    }
}
