using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float exp = 0;
    public float level = 1;
    public float expNeeded = 10;

    public float upgradePoints = 0;
    public float levelHealth = 1;
    public float levelEnergy = 1;
    public float levelEnergyRegen = 1;

    public float maxHealth = 5;
    public float maxEnergy = 10;
    public float energyRegen = 1;

    public void LevelUp()
    {
        level++;
        upgradePoints++;
        exp -= expNeeded;
        expNeeded += Random.Range(10f, 15f);
    }

    public void HealthUp()
    {
        if (upgradePoints >= 1)
        {
            maxHealth += 5;
            levelHealth++;
            upgradePoints--;
        }
    }

    public void EnergyUp()
    {
        if (upgradePoints >= 1)
        {
            maxEnergy += 3;
            levelEnergy++;
            upgradePoints--;
        }
    }

    public void EnergyRegenUp()
    {
        if (upgradePoints >= 1)
        {
            energyRegen++;
            levelEnergyRegen++;
            upgradePoints--;
        }
    }
}
