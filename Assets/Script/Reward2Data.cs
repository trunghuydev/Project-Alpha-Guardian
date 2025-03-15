using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reward2Data : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject line3;
    public GameObject line4;
    public GameObject line5;
    public GameObject line6;

    string curioRewardPath = "Assets/Data/ingame_data/reward/curioselect.txt";
    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";

    int chipReward = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Line3Button()
    {
        int randomResult = Random.Range(0, 100);
        if (randomResult < 65) 
        {
            chipReward += 150;
            line4.SetActive(true);
        }
        else
        {
            chipReward += 300;
            File.WriteAllText(curioRewardPath, "1");
            line3.SetActive(false);
            line6.SetActive(true);
        }
    }

    public void Line4Button()
    {
        int randomResult = Random.Range(0, 100);
        if (randomResult < 35)
        {
            chipReward += 150;
            line5.SetActive(true);
        }
        else
        {
            chipReward += 300;
            File.WriteAllText(curioRewardPath, "1");
            line3.SetActive(false);
            line4.SetActive(false);
            line6.SetActive(true);
        }
    }

    public void Line5Button()
    {
        line3.SetActive(false);
        line4.SetActive(false);
        line5.SetActive(false);
        line6.SetActive(true);

        chipReward += 300;
        File.WriteAllText(curioRewardPath, "1");
        line3.SetActive(false);
        line4.SetActive(false);
        line6.SetActive(true);
    }

    public void Line6Button()
    {
        int.TryParse(File.ReadAllText(electricChipPath), out int currentChip);
        currentChip += chipReward;
        File.WriteAllText(electricChipPath, currentChip.ToString());

        LoadSceneWithDelay("Adventure", 1f);
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
