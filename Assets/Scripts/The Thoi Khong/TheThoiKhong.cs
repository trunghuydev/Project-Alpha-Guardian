using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLightCone", menuName = "Items/LightCone")]
public class TheThoiKhong : ScriptableObject
{
    public string ID;
    public string lightConeName;         
    public string description;
    
    public float baseAtk;               
    public float baseDef;
    public float baseHp;

    public float atkBonus;
    public float atkSpeed;
    public float hpBonus;
    public float critDmgBonus;
    public float critRateBonus;            
}
