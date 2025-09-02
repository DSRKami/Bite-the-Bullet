using UnityEngine;
using TMPro;

public class GamePlayUI : MonoBehaviour
{
    [Header("Current Player Information")]
    public TextMeshProUGUI currentHP;
    public TextMeshProUGUI currentPlayer;
    public TextMeshProUGUI currentOdds;
    private float hpPercentage;

    [Header("Game State")]
    public GameManager gameManager;
    public PlayerState playerA;
    public PlayerState playerB;
    public Revolver revolverA;
    public Revolver revolverB;

    void Update()
    {
        currentPlayer.text = gameManager.isPlayerATurn ? "Player A" : "Player B";
        hpPercentage = gameManager.isPlayerATurn ? playerA.GetHPPercentage() : playerB.GetHPPercentage();
        currentHP.text = $"HP: {hpPercentage:0}%";

        currentOdds.text = gameManager.isPlayerATurn ? $"Odds: {revolverA.GetLoadedToothCount()}/{revolverA.GetChamberSize()}" :
         $"Odds: {revolverB.GetLoadedToothCount()}/{revolverB.GetChamberSize()}";
    }
}
