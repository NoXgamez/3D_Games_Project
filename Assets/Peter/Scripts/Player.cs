using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float exp = 0;
    public float level = 1;
    public float expNeeded = Random.Range(10f, 20f);

    public float upgradePoints = 0;
    public float levelHealth = 1;
    public float levelEnergy = 1;
    public float levelDamage = 1;

    public float maxHealth = 5;
    public float maxEnergy = 10;
    public float damage = 1;

    public void LevelUp()
    {
        level++;
        upgradePoints++;
        exp -= expNeeded;
        expNeeded = Random.Range(10f, 15f);
    }

    public void HealthUp()
    {
        maxHealth += 5;
        upgradePoints--;
    }

    public void EnergyUp()
    {
        maxEnergy += 10;
        upgradePoints--;
    }

    public void DamageUp()
    {
        damage++;
        upgradePoints--;
    }
}
