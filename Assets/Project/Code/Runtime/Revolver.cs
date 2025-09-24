using UnityEngine;

public class Revolver : MonoBehaviour
{
    public ToothType[] chamberedTeeth = new ToothType[6];
    public bool hasIncisorBonus = false;
    public int incisorCount = 0;
    public bool hasGoldFillingLoaded = false;
    public bool isBlank = false;

    void Start()
    {
        for (int i = 0; i < chamberedTeeth.Length; i++)
        {
            chamberedTeeth[i] = ToothType.Blank;
        }
    }

    void Update()
    {

    }

    public bool LoadTooth(ToothType tooth)
    {
        SpinChamber();

        for (int i = 0; i < chamberedTeeth.Length; i++)
        {
            if (chamberedTeeth[i] == ToothType.Blank)
            {
                chamberedTeeth[i] = tooth;
                if (tooth == ToothType.Incisor) incisorCount++;

                hasIncisorBonus = incisorCount >= 3; 
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

        // Update incisor count and bonus status
        if (firedTooth == ToothType.Incisor)
        {
            incisorCount--;
            hasIncisorBonus = incisorCount >= 3;
        }

        if (firedTooth == ToothType.Blank)
        {
            isBlank = true;
        }
        else
        {
            isBlank = false;
        }

        return firedTooth;
    }

    public int GetLoadedToothCount()
    {
        int count = 0;
        foreach (var tooth in chamberedTeeth)
        {
            if (tooth != ToothType.Blank)
            {
                count++;
            }
        }
        return count;
    }

    public int GetChamberSize()
    {
        return chamberedTeeth.Length;
    }
}
