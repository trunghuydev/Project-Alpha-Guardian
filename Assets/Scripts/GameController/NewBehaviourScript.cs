using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public TheThoiKhong currentLightCone;
    public void EquipLightCone()
    {
        if (currentLightCone != null)
        {
            XayahStats xayahStats = FindObjectOfType<XayahStats>();
            if (xayahStats != null)
            {
                xayahStats.EquipLightCone(currentLightCone); 
            }
        }
    }
}
