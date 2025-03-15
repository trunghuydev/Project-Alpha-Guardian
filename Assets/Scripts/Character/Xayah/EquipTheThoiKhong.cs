using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTheThoiKhong : MonoBehaviour
{
    // Start is called before the first frame update
    public XayahStats xayahStats;      // Reference to XayahStats (to equip Light Cone)
    public TheThoiKhong lightCone;     // The Light Cone to equip

    // This method could be called from a UI button or other game logic
    public void Equip()
    {
        // Equip the Light Cone to XayahStats
        xayahStats.theThoiKhong = lightCone;

        // Apply the Light Cone effects
        xayahStats.ApplyLightConeEffects();
        
    }
}
