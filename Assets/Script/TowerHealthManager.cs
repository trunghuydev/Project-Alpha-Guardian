using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthManager : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI HealthText;
    public GameObject CurrentHealth;
    string currentHealthPath = "Assets/Data/Hero_stat/ingame_stat/tower/TowerCurrentHealth.txt";
    string maxHealthPath = "Assets/Data/Hero_stat/ingame_stat/tower/TowerMaxHealth.txt";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetHealth();
    }

    void GetHealth()
    {
        int.TryParse(File.ReadAllText(currentHealthPath), out int currentHealth);
        int.TryParse(File.ReadAllText(maxHealthPath), out int maxHealth);

        HealthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        CurrentHealth.GetComponent<Image>().fillAmount = (float) currentHealth / maxHealth;
    }
}
