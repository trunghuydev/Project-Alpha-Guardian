using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XayahUltimate : MonoBehaviour
{
    public float energy;
    public float currentEnergy = 0;
    private float cooldownTime;

    public Image currentEnergyImg;
    private XayahStats xayah;

    private void Awake()
    {
        xayah = FindObjectOfType<XayahStats>();
    }

    private void Start()
    {
        currentEnergyImg.fillAmount = currentEnergy;
        cooldownTime = 0;
    }
    private void Update()
    {
        GainEnergy();
        
    }

    private float GainEnergy()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, energy);
        UpdateEnergy();
        cooldownTime += Time.deltaTime;
        if(cooldownTime >= 2)
        {
            currentEnergy += (1 * xayah.getManaRegen());
            cooldownTime = 0;
        }
        
        return currentEnergy;
    }
    public void GainEnergyNA()
    {   
        currentEnergy += (1 * xayah.getManaRegen());
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

    public void UltAnimation()
    {
        XayahAttack xayahAttack = FindObjectOfType<XayahAttack>();
        xayahAttack.ShootFeatherUlt(-100);
        xayahAttack.ShootFeatherUlt(-102);
        xayahAttack.ShootFeatherUlt(-104);
        xayahAttack.ShootFeatherUlt(-106);
        xayahAttack.ShootFeatherUlt(-108);
        xayahAttack.ShootFeatherUlt(-110);
        xayahAttack.ShootFeatherUlt(-112);
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
