using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class StatComponent : MonoBehaviour
{
    public Stats Data;
    string path = $"{Application.dataPath}/StatData.json";

    public TMPro.TMP_Text statPoints;
    public TMPro.TMP_Text healthLevel;
    public TMPro.TMP_Text energyLevel;
    public TMPro.TMP_Text energyRegenLevel;

    void Start()
    {
        if (File.Exists(path))
        {
            Data = GameUtilities.Load<Stats>(path);
        }
        else
        {
            Data = new Stats()
            {
                exp = 0,
                level = 1,
                expNeeded = 10,
                upgradePoints = 0,
                levelHealth = 1,
                levelEnergy = 1,
                levelEnergyRegen = 1,
                maxHealth = 5,
                maxEnergy = 10,
                energyRegen = 4
            };

            GameUtilities.Save<Stats>(Data, path);
        }

        healthLevel.text = Data.levelHealth.ToString();
        energyLevel.text = Data.levelEnergy.ToString();
        energyRegenLevel.text = Data.levelEnergyRegen.ToString();
        statPoints.text = Data.upgradePoints.ToString();
    }

    public void HealthUp()
    {
        if (Data.upgradePoints >= 1 && Data.levelHealth < 10)
        {
            Data.maxHealth += 5;
            Data.levelHealth++;
            Data.upgradePoints--;

            healthLevel.text = Data.levelHealth.ToString();
            statPoints.text = Data.upgradePoints.ToString();

            GameUtilities.Save<Stats>(Data, path);
        }
    }

    public void EnergyUp()
    {
        if (Data.upgradePoints >= 1 && Data.levelEnergy < 10)
        {
            Data.maxEnergy += 3;
            Data.levelEnergy++;
            Data.upgradePoints--;

            energyLevel.text = Data.levelEnergy.ToString();
            statPoints.text = Data.upgradePoints.ToString();

            GameUtilities.Save<Stats>(Data, path);
        }
    }

    public void EnergyRegenUp()
    {
        if (Data.upgradePoints >= 1 && Data.levelEnergyRegen < 10)
        {
            Data.energyRegen++;
            Data.levelEnergyRegen++;
            Data.upgradePoints--;

            energyRegenLevel.text = Data.levelEnergyRegen.ToString();
            statPoints.text = Data.upgradePoints.ToString();

            GameUtilities.Save<Stats>(Data, path);
        }
    }
}
