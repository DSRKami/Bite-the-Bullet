using UnityEngine;

public class Revolver : MonoBehaviour
{
    public ToothType[] chamberedTeeth = new ToothType[6];
    public bool hasGoldFillingLoaded = false;

    void Start()
    {
        for (int i = 0; i < chamberedTeeth.Length; i++)
        {
            chamberedTeeth[i] = ToothType.Blank;
        }
    }

    public bool LoadTooth(ToothType tooth)
    {
        SpinChamber();

        for (int i = 0; i < chamberedTeeth.Length; i++)
        {
            if (chamberedTeeth[i] == ToothType.Blank)
            {
                chamberedTeeth[i] = tooth;
                return true;
            }
        }
        return false; // no space
    }

    public void SpinChamber()
    {
        // Add blanks until chamber is size 6
        while (chamberedTeeth.Length < 6)
        {
            ToothType[] newChamber = new ToothType[chamberedTeeth.Length + 1];
            for (int i = 0; i < chamberedTeeth.Length; i++)
            {
                newChamber[i] = chamberedTeeth[i];
            }
            newChamber[chamberedTeeth.Length] = ToothType.Blank;
            chamberedTeeth = newChamber;
        }
    }

    public ToothType Fire()
    {
        if (chamberedTeeth.Length == 0) return ToothType.Blank;

        int randomIndex;

        if (!hasGoldFillingLoaded)
        {
            // Select any chamber
            randomIndex = Random.Range(0, chamberedTeeth.Length);
        }
        else
        {
            // Only select from loaded (non-blank) teeth
            var loadedIndices = new System.Collections.Generic.List<int>();
            for (int i = 0; i < chamberedTeeth.Length; i++)
            {
                if (chamberedTeeth[i] != ToothType.Blank)
                {
                    loadedIndices.Add(i);
                }
            }

            if (loadedIndices.Count == 0) return ToothType.Blank; // No loaded teeth

            randomIndex = loadedIndices[Random.Range(0, loadedIndices.Count)];
        }

        ToothType firedTooth = chamberedTeeth[randomIndex];

        // Shrink the array by removing the fired tooth
        ToothType[] newChamber = new ToothType[chamberedTeeth.Length - 1];
        int newIndex = 0;
        for (int i = 0; i < chamberedTeeth.Length; i++)
        {
            if (i != randomIndex)
            {
                newChamber[newIndex++] = chamberedTeeth[i];
            }
        }
        chamberedTeeth = newChamber;

        // If Gold Filling was fired, disable the effect
        if (firedTooth == ToothType.GoldFilling)
        {
            hasGoldFillingLoaded = false;
        }
        return firedTooth;
    }
}
