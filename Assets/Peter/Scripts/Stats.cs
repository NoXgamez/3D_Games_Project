using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public float exp;
    public float level;
    public float expNeeded;

    public float upgradePoints;
    public float levelHealth;
    public float levelEnergy;
    public float levelEnergyRegen;

    public float maxHealth;
    public float maxEnergy;
    public float energyRegen;

    // Move to player once stats are implemented
    public void LevelUp()
    {
        if (exp >= expNeeded)
        {
            level++;
            upgradePoints++;
            exp -= expNeeded;
            expNeeded += Random.Range(10f, 15f);
        }
    }
}