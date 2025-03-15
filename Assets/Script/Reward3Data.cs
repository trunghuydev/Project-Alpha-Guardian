using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reward3Data : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI currentEChipText;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";

    string potionDataPath = "Assets/Data/ingame_data/potion";

    string curioRewardPath = "Assets/Data/ingame_data/reward/curioselect.txt";

    string systemPointPath = "Assets/Data/ingame_data/system_point.txt";


    void Start()
    {
        int.TryParse(File.ReadAllText(electricChipPath), out int currentChip);
        File.WriteAllText(electricChipPath, (currentChip+1).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEChipAmount();
        CheckForAvailable();
    }

    void CheckForAvailable()
    {
        int.TryParse(File.ReadAllText(electricChipPath), out int currentChip);
        if (currentChip < 1)
        {
            button1.interactable = false;
            button2.interactable = false;
            button3.interactable = false;
        }
    }

    void UpdateEChipAmount()
    {
        currentEChipText.text = "Chip Điện tử hiện tại: " + int.Parse(File.ReadAllText(electricChipPath)).ToString();
    }

    public void PurchaseItem(int selection)
    {
        int.TryParse(File.ReadAllText(electricChipPath), out int currentChip);
        currentChip += -1;
        File.WriteAllText(electricChipPath, currentChip.ToString());

        switch (selection)
        {
            case 1:
                int potion1 = Random.Range(0, 3);
                int potion2 = Random.Range(0, 3);
                AddLimitedPotion(potion1);
                AddLimitedPotion(potion2);
                button1.interactable = false;
                break;
            case 2:
                File.WriteAllText(curioRewardPath, "1");
                button2.interactable = false;
                break;
            case 3:
                int currentsystempoint = int.Parse(File.ReadAllText(systemPointPath));
                currentsystempoint += 20;
                if (currentsystempoint > 100)
                {
                    currentsystempoint = 100;
                }
                File.WriteAllText(systemPointPath, currentsystempoint.ToString());
                button3.interactable = false;
                break;
            case 4:
                LoadSceneWithDelay("Adventure", 1f);
                break;           
        }
    }

    void AddLimitedPotion (int randomResult)
    {
        string potion = "/potion";
        switch(randomResult)
        {
            case 0:
                potion += "3.txt";
                break;
            case 1:
                potion += "8.txt";
                break;
            case 2:
                potion += "11.txt";
                break;        
        }

        string path = potionDataPath + potion;

        if(File.Exists(path))
        {
            int currentPotion = int.Parse(File.ReadAllText(path));
            File.WriteAllText(path, (currentPotion + 1).ToString());
        }
        else
        {
            File.WriteAllText(path, "1");
        }
        
    }

    public void LoadSceneWithDelay(string sceneName, float delay)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, delay));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
