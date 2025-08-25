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
        if (chamberedTeeth.Length < 6)
        {
            chamberedTeeth.Add(ToothType.Blank);
            if (chamberedTeeth.Length < 6)
            {
                SpinChamber();
            }
        }
    }

    public ToothType Fire()
    {
        if (!hasGoldFillingLoaded)
        {
            // Find incides of teeth
            var indices = new System.Collections.Generic.List<int>();
            for (int i = 0; i < chamberedTeeth.Length; i++)
            {
                indices.Add(i);
            }

            // Randomly select one
            int randomIndex = indices[randomIndex.Range(0, indices.Count)];
            ToothType firedTooth = chamberedTeeth[randomIndex];

            // Remove the fired tooth from the array
            ToothType[] newChamber = new ToothType[chamberedTeeth.Length - 1];
            int newIndex = 0;
            for (int i = 0; i < chamberedTeeth.Length; i++)
            {
                if (i != randomIndex) newChamber[newIndex++] = chamberedTeeth[i];
            }

            chamberedTeeth = newChamber;
            return firedTooth;
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

            if (loadedIndices.Count == 0) return ToothType.Blank; // No loaded teeth, should not happen

            int randomIndex = loadedIndices[randomIndex.Range(0, loadedIndices.Count)];
            ToothType firedTooth = chamberedTeeth[randomIndex];

            // Remove the fired tooth from the array
            ToothType[] newChamber = new ToothType[chamberedTeeth.Length - 1];
            int newIndex = 0;
            for (int i = 0; i < chamberedTeeth.Length; i++)
            {
                if (i != randomIndex) newChamber[newIndex++] = chamberedTeeth[i];
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
}
