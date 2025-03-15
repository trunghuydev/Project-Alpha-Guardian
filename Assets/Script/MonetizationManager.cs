using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonetizationManager : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI currentEChip;

    public TextMeshProUGUI costButtonText;
    public GameObject EchipImage;

    int currentElectricChip;

    string pathToCurrentEChip = "Assets/Data/ingame_data/electric_chip_amount.txt";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEChipQuantity();
        HideEChipImageIfPurchased();
    }

    void HideEChipImageIfPurchased()
    {
        if (costButtonText.text == "Đã mua")
        {
            EchipImage.SetActive(false);
        }
        else
        {
            EchipImage.SetActive(true);
        }
    }

    void UpdateEChipQuantity()
    {
        currentElectricChip = int.Parse(File.ReadAllText(pathToCurrentEChip));
        currentEChip.text = currentElectricChip.ToString();
    }

    public void BackToAdventure()
    {
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
