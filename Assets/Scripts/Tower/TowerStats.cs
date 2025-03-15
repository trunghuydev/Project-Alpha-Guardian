using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TowerStats : MonoBehaviour
{ 
    [SerializeField] private Image hpBar; //Drag Hp image to this in editor

    public float hpBuff;
    public float currentHpBuff;
    float hpRegen;

    [SerializeField] private GameObject ashe;
    GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Start()
    {
        string path = "Assets/Data/Hero_stat/curio_stat/TowerMaxHealth.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out hpRegen))
            {

            }
            else
            {
                Debug.Log("Nothing inside file");
            }

        }
        currentHpBuff = GetCurrentHp() + hpRegen;
        hpBuff = GetMaxHp();
    }

    private void Update()
    {
        Vector3 position = transform.position;

        // Get camera bounds
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        // Clamp the position to stay within the camera bounds
        position.x = Mathf.Clamp(position.x, -cameraWidth / 2, cameraWidth / 2);
        position.y = Mathf.Clamp(position.y, -cameraHeight / 2, cameraHeight / 2);

        transform.position = position;
        UpdateHPBar();
    }

    public void UpdateHP(float amount)
    {
        if (this == null)
        {
            return;
        }
            
        currentHpBuff += amount;
        currentHpBuff = Mathf.Clamp(currentHpBuff, 0f, hpBuff); //When update hp, this wont make hp overflow the max HP, ex: when healing
        
        if(currentHpBuff <= 0)
        {
            Destroy(gameObject);
            if (ashe != null)
            {
                Destroy(ashe);
            }
            gameController.Defeated();
        }
        else
        {
            UpdateHPBar();
        }
    }


    public void UpdateHPBar()
    {
        if (this != null && hpBar != null)
        {
            float targetFillAmount = currentHpBuff / hpBuff;
            hpBar.fillAmount = targetFillAmount;
        }
    }

    private float GetCurrentHp()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/tower/TowerCurrentHealth.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out currentHpBuff))
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
        return currentHpBuff;
    }

    private float GetMaxHp()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/tower/TowerMaxHealth.txt";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace(',', '.');
            if (float.TryParse(fileContent, NumberStyles.Float, CultureInfo.InvariantCulture, out hpBuff))
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
        return hpBuff;
    }

    public void SaveCurrentHp()
    {
        string path = "Assets/Data/Hero_stat/Ingame_stat/tower/TowerCurrentHealth.txt";
        try
        {            
            int roundedHp = (int)Math.Round(currentHpBuff);
            string hpString = roundedHp.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
            File.WriteAllText(path, hpString);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error saving current HP: " + ex.Message);
        }
    }

    
    

}
