using UnityEngine;
using UnityEngine.UI;

public class Chamber : MonoBehaviour
{
    [Header("Chamber Sprites ")]
    public Sprite[] toothSpritesImages;
    public Image[] chamberImages;

    [Header("Player Revolves")]
    public Revolver revolverA;
    public Revolver revolverB;
    public GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialise all chamber images to be blank
        for (int i = 0; i < chamberImages.Length; i++)
        {
            chamberImages[i].sprite = toothSpritesImages[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isPlayerATurn)
        {
            UpdateChamberDisplay(revolverA);
        }
        else
        {
            UpdateChamberDisplay(revolverB);
        }
    }

    void UpdateChamberDisplay(Revolver revolver)
    {
        for (int i = 0; i < chamberImages.Length; i++)
        {
            if (i < revolver.chamberedTeeth.Length)
            {
                ToothType tooth = revolver.chamberedTeeth[i];
                chamberImages[i].sprite = toothSpritesImages[(int)tooth];
            }
            else
            {
                chamberImages[i].sprite = toothSpritesImages[7]; // Used
            }
        }
    }
}
