using UnityEngine;
using TMPro;
using System.Collections;
public class GameManager : MonoBehaviour
{
    [Header("Players")]
    public PlayerState playerA;
    public PlayerState playerB;

    [Header("Revolvers")]
    public Revolver revolverA;
    public Revolver revolverB;

    [Header("Interactable Colliders")]
    public Collider revolverACollider;
    public Collider revolverBCollider;
    public Collider pliersACollider;
    public Collider pliersBCollider;


    [Header("Game State")]
    public bool isPlayerATurn = true;
    public int currentTurnCount = 0;
    public RayCast rayCast;

    [Header("UI Elements")]
    public TextMeshProUGUI displayMessage;
    public float fadeDuration = 1.5f;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (RevolverInteract.revolverAnimating || PliersInteract.pliersAnimating)
        {
            revolverACollider.enabled = false;
            pliersACollider.enabled = false;
            revolverBCollider.enabled = false;
            pliersBCollider.enabled = false;
        }
        else
        {
            UpdateColliders();
        }
    }

    public void StartGame()
    {
        isPlayerATurn = Random.Range(0, 2) == 0;
        currentTurnCount = 0;
        UpdateColliders();
        Debug.Log("Coinflip! " + (isPlayerATurn ? "Player A starts" : "Player B starts"));
    }

    public void NextTurn()
    {
        isPlayerATurn = !isPlayerATurn;
        currentTurnCount++;
        PlayerState currentPlayer = isPlayerATurn ? playerA : playerB;
        currentPlayer.UpdateStatusEffects();
        UpdateColliders();
        Debug.Log("Turn " + currentTurnCount + ": " + (isPlayerATurn ? "Player A's turn" : "Player B's turn"));
    }

    private void CheckGameOver()
    {
        if (playerA.Health <= 0)
        {
            Debug.Log("Player B wins!");
            EndGame();
        }
        else if (playerB.Health <= 0)
        {
            Debug.Log("Player A wins!");
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over!");
        enabled = false; // Disable further game actions
    }

    public void PlayTurn(PlayerAction action, ToothType selectedTooth = ToothType.Blank)
    {
        PlayerState currentPlayer = isPlayerATurn ? playerA : playerB;
        Revolver currentRevolver = isPlayerATurn ? revolverA : revolverB;
        PlayerState opponentPlayer = isPlayerATurn ? playerB : playerA;
        Revolver opponentRevolver = isPlayerATurn ? revolverB : revolverA;

        switch (action)
        {
            case PlayerAction.Pliers:
                // Pluck tooth and load into revolver
                if (currentPlayer.Pluck(selectedTooth))
                {
                    if (currentRevolver.LoadTooth(selectedTooth))
                    {
                        if (selectedTooth == ToothType.GoldFilling)
                        {
                            currentRevolver.hasGoldFillingLoaded = true;
                        }
                    }
                    rayCast.pliersAnimator.SetTrigger("Drop Tooth");
                    rayCast.revolverAnimator.SetTrigger("Load");
                    ShowAndFade($"Plucked {selectedTooth}");
                    CheckIncisorBonus(currentPlayer);
                    CheckForGoldFilling(currentPlayer);
                }
                break;

            case PlayerAction.Revolver:
                // Fire Revolver at Opponent
                ToothType firedTooth = currentRevolver.Fire();
                ApplyToothEffect(firedTooth, currentPlayer, opponentPlayer, opponentRevolver);
                ShowAndFade($"Fired {firedTooth}");
                break;

            case PlayerAction.EndTurn:
                // End turn
                Debug.Log($"{(isPlayerATurn ? "Player A" : "Player B")} ended their turn.");
                NextTurn();
                break;

        }
    }

    // Handles effects based on tooth type
    private void ApplyToothEffect(ToothType tooth, PlayerState shooter, PlayerState target, Revolver targetRevolver)
    {
        int bonusDamage = 0;
        if (shooter.incisorBonusNextTurn && !shooter.effectsDisabled)
        {
            bonusDamage = 1;
            shooter.incisorBonusNextTurn = false; // Reset bonus
            Debug.Log($"{shooter.gameObject.name} Incisor bonus applied! +1 damage.");
        }


        switch (tooth)
        {
            case ToothType.Incisor:
                target.TakeDamage(2 + bonusDamage);
                if (shooter.effectsDisabled) break;
                break;
            case ToothType.Canine:
                target.TakeDamage(2 + bonusDamage);
                if (shooter.effectsDisabled) break;
                ApplyBleedEffect(target);
                break;
            case ToothType.Premolar:
                target.TakeDamage(2 + bonusDamage);
                if (shooter.effectsDisabled) break;
                SpinOpponentChamber(targetRevolver);
                break;
            case ToothType.Molar:
                target.TakeDamage(6 + bonusDamage);
                break;
            case ToothType.Wisdom:
                target.TakeDamage(2 + bonusDamage);
                if (shooter.effectsDisabled) break;
                RemoveRandomToothFromChamber(targetRevolver);
                break;
            case ToothType.GoldFilling:
                target.TakeDamage(2 + bonusDamage);
                if (shooter.effectsDisabled) break;
                break;
            case ToothType.Blank:
                // No effect
                break;
        }

        CheckGameOver();
    }

    private void SpinOpponentChamber(Revolver opponentRevolver)
    {
        opponentRevolver.SpinChamber();
        Debug.Log("Opponent's revolver chamber spun due to Premolar effect!");
    }

    private void CheckIncisorBonus(PlayerState shooter)
    {
        // Count loaded incisors in shooter's revolver
        Revolver shooterRevolver = isPlayerATurn ? revolverA : revolverB;
        int incisorCount = 0;

        foreach (var tooth in shooterRevolver.chamberedTeeth)
        {
            if (tooth == ToothType.Incisor) incisorCount++;
        }

        if (incisorCount >= 3)
        {
            shooter.incisorBonusNextTurn = true;
            Debug.Log($"{shooter.gameObject.name} will deal +1 damage next turn due to Incisor bonus!");
        }
    }

    private void ApplyBleedEffect(PlayerState target)
    {
        target.isBleeding = true;
        target.bleedTurnsRemaining = 2;
        target.effectsDisabled = true;
        target.effectsDisabledTurns = 2;
        Debug.Log($"{target.gameObject.name} is bleeding and tooth effects are disabled for 2 turns!");
    }

    private void RemoveRandomToothFromChamber(Revolver revolver)
    {
        // Find indices of non-blank teeth
        var loadedIndices = new System.Collections.Generic.List<int>();
        for (int i = 0; i < revolver.chamberedTeeth.Length; i++)
        {
            if (revolver.chamberedTeeth[i] != ToothType.Blank)
            {
                loadedIndices.Add(i);
            }
        }

        if (loadedIndices.Count == 0) return; // No teeth to remove

        // Randomly select one and turn it into a blank
        int randomIndex = loadedIndices[Random.Range(0, loadedIndices.Count)];
        ToothType removedTooth = revolver.chamberedTeeth[randomIndex];
        revolver.chamberedTeeth[randomIndex] = ToothType.Blank;
        Debug.Log($"Wisdom Tooth effect: Removed {removedTooth} from opponent's chamber!");
    }

    private void CheckForGoldFilling(PlayerState shooter)
    {
        Revolver shooterRevolver = isPlayerATurn ? revolverA : revolverB;
        // Check for Gold Filling in the chamber
        foreach (var tooth in shooterRevolver.chamberedTeeth)
        {
            if (tooth == ToothType.GoldFilling)
            {
                shooterRevolver.hasGoldFillingLoaded = true;
                Debug.Log($"{shooter.gameObject.name}'s revolver is now guaranteed to fire loaded teeth until the Gold Filling is fired!");
                return;
            }
        }

        shooterRevolver.hasGoldFillingLoaded = false;
    }

    public void ShowAndFade(string message)
    {
        StopAllCoroutines();
        displayMessage.text = message;
        displayMessage.alpha = 1f;
        displayMessage.gameObject.SetActive(true);
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            displayMessage.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        displayMessage.alpha = 0f;
        displayMessage.gameObject.SetActive(false);
    }

    private void UpdateColliders()
    {
        revolverACollider.enabled = isPlayerATurn;
        pliersACollider.enabled = isPlayerATurn;
        revolverBCollider.enabled = !isPlayerATurn;
        pliersBCollider.enabled = !isPlayerATurn;
    }
}

public enum PlayerAction
{
    Pliers,
    Revolver,
    EndTurn
}
