using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class RakanStats : MonoBehaviour
{
    public float range;
    public float movementSpeed;
    public float shield;
    public float armor;
    public float attack;
    public float attackSpd;
    public float critDmgBuff;
    public float critRateBuff;
    public float dmgBoost;
    public float health;
    public float healthRegen;
    public float ignoreRes;
    public float mana;
    public float manaRegen;
    public float resBuff;
    


    private void Start()
    {
        
    }
    public float getAttackBuff()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/Attack.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out attack))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }
        return attack;
    }

    public float getArmor()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/Armor.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out armor))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }
        return armor;
    }

    public float getAttackSpd()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/AttackSpd.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out attackSpd))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }
        return attackSpd;
    }

    public float getCritDmg()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/CritDmg.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out critDmgBuff))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }
        return critDmgBuff + 1;
    }

    public float getCritRate()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/CritRate.txt";

        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path).Trim();
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out critRateBuff))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }
        return critRateBuff;
    }

    public float getDmgBoost()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/DmgBoost.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out dmgBoost))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return dmgBoost + 1;
    }

    public float getHealth()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/Health.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out health))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }

        return health;
    }

    public float getHealthRegen()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/HealthRegen.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out healthRegen))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return healthRegen;
    }

    public float getIgnoreRes()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/IgnoreRes.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out ignoreRes))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return ignoreRes;
    }

    public float getMana()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/Mana.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.'); 
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out mana))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return mana;
    }

    public float getManaRegen()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/ManaRegen.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out manaRegen))
            {

            }
            else
            {
                Debug.LogError("Nothing inside file");
            }

        }
        else
        {
            Debug.LogError("Path not found");
        }

        return manaRegen;
    }

    public float getRes()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/rakan/Res.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out resBuff))
            {
                
            }
            else
            {
                Debug.LogError("Nothing inside file");
            }
        }
        else
        {
            Debug.LogError("Path not found");
        }
        return resBuff;
    }
    public float getRange()
    {
        return range;
    }
    public float getMovementSpeed()
    {
        return movementSpeed;
    }
}
