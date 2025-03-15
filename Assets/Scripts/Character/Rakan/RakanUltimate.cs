using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RakanUltimate : MonoBehaviour
{
    public float energy;
    public float currentEnergy = 0;
    private float cooldownTime;
    private SpawnRakan spawnRakan;
    private RakanStats rakanStats;

    public Image currentEnergyImg;

    private void Start()
    {
        currentEnergyImg.fillAmount = currentEnergy;
        cooldownTime = 0;
        spawnRakan = FindObjectOfType<SpawnRakan>();
        rakanStats = FindObjectOfType<RakanStats>();
    }
    private void Update()
    {
        if (!spawnRakan.isUltActive)
        {
            GainEnergy();
        }
    }

    private float GainEnergy()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, energy);
        UpdateEnergy();
        cooldownTime += Time.deltaTime;
        if (cooldownTime >= 2)
        {
            currentEnergy += 1 * rakanStats.getManaRegen();
            cooldownTime = 0;
        }

        return currentEnergy;
    }
    public void GainEnergyNA()
    {
        currentEnergy += 1 * rakanStats.getManaRegen();
    }

    public void GainEnergySkill()
    {
        currentEnergy += 4 * rakanStats.getManaRegen();
    }

    public void GainEnergySkillEachEnemy()
    {
        currentEnergy += 2 * rakanStats.getManaRegen();
    }

    private void UpdateEnergy()
    {
        float targetFillAmount = currentEnergy / energy;
        currentEnergyImg.fillAmount = targetFillAmount;
    }

    public void ResetEnergy()
    {
        currentEnergy = 0;
        UpdateEnergy();
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public float GetMaxEnergy()
    {
        return energy;
    }
}
