using UnityEngine;
using System.Collections.Generic;

public class PlayerState : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 12;
    private int currentHealth;
    public bool isBleeding = false;
    public int bleedTurnsRemaining = 0;
    public bool effectsDisabled = false;
    public int effectsDisabledTurns = 0;
    public bool incisorBonusNextTurn = false;

    [Header("Starting Teeth")]
    [SerializeField] private int incisors = 4;
    [SerializeField] private int canines = 2;
    [SerializeField] private int premolars = 4;
    [SerializeField] private int molars = 3;
    [SerializeField] private int wisdom = 2;
    [SerializeField] private int goldFillings = 1;

    // Mouth Dictionary
    private Dictionary<ToothType, int> mouth;

    // Removal Damage Map
    private static readonly Dictionary<ToothType, int> removalDamageMap = new Dictionary<ToothType, int>
    {
        { ToothType.Incisor, 1 },
        { ToothType.Canine, 2 },
        { ToothType.Premolar, 1 },
        { ToothType.Molar, 3 },
        { ToothType.Wisdom, 2 },
        { ToothType.GoldFilling, 3 }
    };

    public int Health => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;

        mouth = new Dictionary<ToothType, int>
        {
            { ToothType.Incisor, incisors },
            { ToothType.Canine, canines },
            { ToothType.Premolar, premolars },
            { ToothType.Molar, molars },
            { ToothType.Wisdom, wisdom },
            { ToothType.GoldFilling, goldFillings }
        };
    }

    public bool Pluck(ToothType tooth)
    {
        if (mouth.ContainsKey(tooth) && mouth[tooth] > 0)
        {
            // Don't allow plucking if it would reduce health to 0 or below
            if (currentHealth - removalDamageMap[tooth] <= 0) return false;

            mouth[tooth]--;
            currentHealth -= removalDamageMap[tooth];
            return true;
        }
        return false;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0); // Prevent negative health
        Debug.Log($"{gameObject.name} took {amount} damage! Health: {currentHealth}/{maxHealth}");
    }

    public void UpdateStatusEffects()
    {
        if (isBleeding)
        {
            bleedTurnsRemaining = Mathf.Max(bleedTurnsRemaining - 1, 0);
            if (bleedTurnsRemaining == 0)
            {
                isBleeding = false;
                Debug.Log($"{gameObject.name} has stopped bleeding.");
            }
        }

        if (effectsDisabled)
        {
            effectsDisabledTurns = Mathf.Max(effectsDisabledTurns - 1, 0);
            if (effectsDisabledTurns == 0)
            {
                effectsDisabled = false;
                Debug.Log($"{gameObject.name}'s tooth effects are re-enabled.");
            }
        }
    }
}
