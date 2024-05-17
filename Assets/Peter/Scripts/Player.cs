using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    Stats stats = GameUtilities.Load<Stats>($"{Application.dataPath}/StatData.json");

    float maxHealth;
    public float health;
    float maxEnergy;
    public float energy;
    float energyRegen;
    public float level;
    public float statPoints;
    public float exp;
    public float expNeeded;

    public TMPro.TMP_Text levelText;
    public TMPro.TMP_Text healthText;
    public TMPro.TMP_Text energyText;
    public TMPro.TMP_Text expText;

    public float swordDamage = 1;
    public float gunDamage = 2;

    void Start()
    {
        maxHealth = stats.maxHealth;
        health = maxHealth;
        maxEnergy = stats.maxEnergy;
        energy = maxEnergy;
        energyRegen = stats.energyRegen;
        level = stats.level;
        statPoints = stats.upgradePoints;
        exp = stats.exp;
        expNeeded = stats.expNeeded;
    }

    void Update()
    {
        levelText.text = "LV " + level.ToString();
        healthText.text = "HP " + ((int)health).ToString() + "/" + maxHealth.ToString();
        expText.text = "XP " + ((int)exp).ToString() + "/" + ((int)expNeeded).ToString();

        if (exp >= expNeeded)
        {
            LevelUp();
        }

        if (energy < maxEnergy)
        {
            energy += energyRegen * Time.deltaTime;
            energyText.text = "ENERGY " + ((int)energy).ToString() + "/" + maxEnergy.ToString();
        }
    }

    public void GainExp(float amount)
    {
        exp += amount;

        Stats newStats = new Stats()
        {
            exp = exp,
            expNeeded = expNeeded,
            level = level,
            upgradePoints = statPoints,
            levelHealth = stats.levelHealth,
            levelEnergy = stats.levelEnergy,
            levelEnergyRegen = stats.levelEnergyRegen,
            maxHealth = maxHealth,
            maxEnergy = maxEnergy,
            energyRegen = energyRegen
        };
        GameUtilities.Save<Stats>(newStats, $"{Application.dataPath}/StatData.json");
    }

    public void LevelUp()
    {
        level++;
        statPoints++;
        exp -= expNeeded;
        expNeeded += Random.Range(10f, 15f);

        Stats newStats = new Stats()
        {
            exp = exp,
            expNeeded = expNeeded,
            level = level,
            upgradePoints = statPoints,
            levelHealth = stats.levelHealth,
            levelEnergy = stats.levelEnergy,
            levelEnergyRegen = stats.levelEnergyRegen,
            maxHealth = maxHealth,
            maxEnergy = maxEnergy,
            energyRegen = energyRegen
        };
        GameUtilities.Save<Stats>(newStats, $"{Application.dataPath}/StatData.json");
    }

    public void GetHit()
    {
        health -= 1f;
    }

    public void DrainEnergy()
    {
        energy -= 5f;
    }
}
