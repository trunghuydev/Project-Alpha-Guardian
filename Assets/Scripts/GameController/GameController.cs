using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CharacterType
{
    None,
    Minion,
    Ranged_Minion,
    Siege_Minion,
    Super_Minion,
    Xayah,
    Rakan
}

public class GameController : MonoBehaviour
{
    //Pause
    [Header("Pause")]
    private bool isPause;
    public GameObject pausePanel;
    public GameObject blur;

    //Show stats related variable
    [Header("Characters")]
    public GameObject showStats;
    private ShowStats showStatsComponent;

    public XayahStats selectedXayah;
    public RakanSetUp selectedRakan;
    public MeleeMinionStats selectedMinion;
    public RangedMinionSetUp selectedRangedMinion;
    public SiegeMinionSetUp selectedSiegeMinion;
    public SuperMinionSetUp selectedSuperMinion;

    
    public CharacterType currentCharacterType = CharacterType.None;

    private void Awake()
    {
        showStatsComponent = FindObjectOfType<ShowStats>();
    }
    private void Start()
    {
        showStats.SetActive(false);
    }

    private void Update()
    {
        //Alway update the stats panel
        if (selectedMinion != null)
        {
            showStatsComponent.UpdateMinionStats(selectedMinion);
        }
        else if (selectedXayah != null)
        {
            showStatsComponent.UpdateXayahStats(selectedXayah);
        }
        else if (selectedRangedMinion != null)
        {
            showStatsComponent.UpdateRangedMinionStats(selectedRangedMinion);
        }
        else if(selectedSiegeMinion != null)
        {
            showStatsComponent.UpdateSiegeMinionStats(selectedSiegeMinion);
        }
        else if(selectedSuperMinion != null)
        {
            showStatsComponent.UpdateSuperMinionStats(selectedSuperMinion);
        }
        else if(selectedRakan != null)
        {
            showStatsComponent.UpdateRakanStats(selectedRakan);
        }
    }

    public void SetSelectedMinion(MeleeMinionStats minion)
    {
        selectedXayah = null;
        selectedRangedMinion = null;
        selectedSiegeMinion = null;
        selectedSuperMinion = null;
        selectedRakan = null;

        selectedMinion = minion;
        
        currentCharacterType = CharacterType.Minion;
        showStatsComponent.UpdateMinionStats(minion); // Update the stats immediately when selected
    }

    public void SetSelectedMinion(RangedMinionSetUp rangedMinion)
    {
        selectedMinion = null;
        selectedXayah = null;
        selectedSiegeMinion = null;
        selectedSuperMinion = null;
        selectedRakan = null;

        selectedRangedMinion = rangedMinion;
        
        currentCharacterType = CharacterType.Ranged_Minion;
        showStatsComponent.UpdateRangedMinionStats(rangedMinion);
    }

    public void SetSelectedMinion(SiegeMinionSetUp siegeMinion)
    {
        selectedMinion = null;
        selectedXayah = null;
        selectedRangedMinion = null;
        selectedSuperMinion = null;
        selectedRakan = null;

        selectedSiegeMinion = siegeMinion;

        currentCharacterType = CharacterType.Siege_Minion;
        showStatsComponent.UpdateSiegeMinionStats(siegeMinion);
    }

    public void SetSelectedMinion(SuperMinionSetUp superMinion)
    {
        selectedMinion = null;
        selectedXayah = null;
        selectedRangedMinion = null;
        selectedSiegeMinion = null;
        selectedRakan = null;

        selectedSuperMinion = superMinion;

        currentCharacterType = CharacterType.Siege_Minion;
        showStatsComponent.UpdateSuperMinionStats(superMinion);
    }

    public void SetSelectedXayah(XayahStats xayah)
    {
        selectedRangedMinion = null;
        selectedMinion = null;
        selectedSiegeMinion = null;
        selectedSuperMinion = null;
        selectedRakan = null;

        selectedXayah = xayah;
        
        currentCharacterType = CharacterType.Xayah;
        showStatsComponent.UpdateXayahStats(xayah); // Update Xayah stats immediately when selected
    }

    public void SetSelectedRakan(RakanSetUp rakan)
    {
        selectedRangedMinion = null;
        selectedMinion = null;
        selectedSiegeMinion = null;
        selectedSuperMinion = null;
        selectedXayah = null;

        selectedRakan = rakan;

        currentCharacterType = CharacterType.Rakan;
        showStatsComponent.UpdateRakanStats(rakan); // Update Xayah stats immediately when selected
    }

    public void ShowStats()
    {
        showStats.SetActive(true);
    }

    public void HideStats()
    {
        showStats.SetActive(false);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        blur.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        pausePanel.SetActive(false);
        blur.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    //Lost panel
    public GameObject panelDefeated;
    public void Defeated()
    {
        panelDefeated.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 0;
    }

    public void DefeatedAndBackToResult()
    {
        SceneManager.LoadScene("EndAdventure");
        Time.timeScale = 1;
    }

    public GameObject panelVictory;
    public void Victory()
    {
        panelVictory.SetActive(true);
        Time.timeScale = 0;
    }

    public void FinishCombat()
    {
        EnemySpawner checkDiff = FindObjectOfType<EnemySpawner>();
        if(checkDiff.diff == 18)
        {
            string currentTile = "Assets/Data/ingame_data/current_tile.txt";
            if (File.Exists(currentTile))
            {
                string[] lines = File.ReadAllLines(currentTile);
                if (lines.Length > 0)
                {
                    int firstLineValue = int.Parse(lines[0]);
                    lines[0] = (firstLineValue + 5).ToString();
                    File.WriteAllLines(currentTile, lines);
                }
                else
                {
                    Debug.LogWarning("File is empty.");
                }
            }
            else
            {
                Debug.LogError("File not found.");
            }

            SceneManager.LoadScene("EndAdventure");
            Time.timeScale = 1;

        }
        else
        {
            string directoryPath = "Assets/Data/ingame_data/reward";
            string filePath = Path.Combine(directoryPath, "curioselect.txt");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";
            if (File.Exists(electricChipPath))
            {
                string fileContent = File.ReadAllText(electricChipPath);
                int currentAmount;
                if (int.TryParse(fileContent, out currentAmount))
                {
                    currentAmount += 50;
                    File.WriteAllText(electricChipPath, currentAmount.ToString());
                }
                else
                {
                    Debug.LogError("The file does not contain a valid integer.");
                }
            }
            else
            {
                Debug.LogError("File not found: " + electricChipPath);
            }

            File.WriteAllText(filePath, "1");
            SceneManager.LoadScene("Adventure");
            Time.timeScale = 1;
        }
        
        
    }

  
}
