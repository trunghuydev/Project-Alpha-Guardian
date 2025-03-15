using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class EnemySpawner : MonoBehaviour
{
    GameController gameController;
    public GameObject[] enemyPrefabs;
    public Vector3 spawnPosition;
    public float offScreenX = 1;

    //difficulty
    public int diff;

    //wave
    public float waveDelay = 40;
    public float spawnDelay = 3;

    private List<GameObject> activeEnemy = new List<GameObject>();
    private List<GameObject> enemySpawned = new List<GameObject>();
    private int totalEnemy;
    private int totalEnemyDestroy;
    private int totalWave;

    private Dictionary<string, (int count, int level)> minionCounts = new Dictionary<string, (int count, int level)>();
    public TextMeshProUGUI minionCountText;
    public TextMeshProUGUI diffText;

    private Dictionary<string, int> waveMinionCounts = new Dictionary<string, int>();
    public TextMeshProUGUI minionsInWave;
    public TextMeshProUGUI waveText;

    
    private void GetDifficulty()
    {
        string path = "Assets/Data/ingame_data/current_tile.txt";
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string firstLine = reader.ReadLine();
                diff = Convert.ToInt32(firstLine) / 5;
                Debug.Log("First Line: " + firstLine);
            }
        }
        else
        {
            Debug.Log("File does not exist.");
        }
    }

    private void DisplayMinionInfo()
    {
        string info = "";
        foreach (var entry in minionCounts)
        {
            info += $"{entry.Value.count} {entry.Key}\n";
        }
        minionCountText.text = info;
        diffText.text = "Độ khó: "+Convert.ToString(diff);
    }

    private void DisplayMinionCountEachWave()
    {
        string waveInfo = "";
        foreach (var entry in waveMinionCounts)
        {
            waveInfo += $"{entry.Value} {entry.Key}\n";
        }
        minionsInWave.text = waveInfo;
    }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        GetDifficulty();
        Debug.Log("Difficulty: "+diff);
        Difficulty();
    }

    private void Update()
    {
        CheckForVictory();
        waveText.text = "Đợt: "+Convert.ToString(totalWave);
        
    }

    private void SpawnMeleeMinion(float level)
    {
        SpawnMinion(0, "Lính cận chiến", level);
    }

    private void SpawnRangedMinion(float level)
    {
        SpawnMinion(1, "Lính đánh xa", level);
    }

    private void SpawnSiegeMinion(float level)
    {
        SpawnMinion(2, "Lính xe pháo", level);
    }

    private void SpawnSuperMinion(float level)
    {
        SpawnMinion(3, "Lính siêu cấp", level);
    }

    private void SpawnMinion(int prefabIndex, string type, float level)
    {
        float currentMultiplier = Mathf.Pow(1.01f, level - 1);
        float screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        
        
        if(prefabIndex == 0)
        {
            spawnPosition = new Vector3(screenWidth / 2f + 8, -2f, 0);
        }
        else
        {
            spawnPosition = new Vector3(screenWidth / 2f + 8, -1.3f, 0);
        }
        GameObject minion = Instantiate(enemyPrefabs[prefabIndex], spawnPosition, Quaternion.identity);
        activeEnemy.Add(minion);
        enemySpawned.Add(minion);
        totalEnemy++;

        SuperMinionStats minionStats = minion.GetComponent<SuperMinionStats>();
        if (minionStats != null)
        {
            minionStats.hp *= currentMultiplier;
            minionStats.atk *= currentMultiplier;
            minionStats.res += level / 100;
        }

        SiegeMinionStats minionStats2 = minion.GetComponent<SiegeMinionStats>();
        if (minionStats2 != null)
        {
            minionStats2.hp *= currentMultiplier;
            minionStats2.atk *= currentMultiplier;
            minionStats2.res += level / 100;
        }

        RangedMinionStats minionStats3 = minion.GetComponent<RangedMinionStats>();
        if (minionStats3 != null)
        {
            minionStats3.hp *= currentMultiplier;
            minionStats3.atk *= currentMultiplier;
            minionStats3.res += level / 100;
        }

        MeleeMinionStats minionStats4 = minion.GetComponent<MeleeMinionStats>();
        if (minionStats4 != null)
        {
            minionStats4.hp *= currentMultiplier;
            minionStats4.atk *= currentMultiplier;
            minionStats4.res += level / 100;
        }

        // Update minion counts
        if (minionCounts.ContainsKey(type))
        {
            minionCounts[type] = (minionCounts[type].count + 1, (int)level);
        }
        else
        {
            minionCounts[type] = (1, (int)level);
        }

        if (waveMinionCounts.ContainsKey(type))
        {
            waveMinionCounts[type]++;
        }
        else
        {
            waveMinionCounts[type] = 1;
        }
        DisplayMinionCountEachWave();

        //Debug.Log($"Total {type}s: {minionCounts[type].count}");
    }


    public void RemoveEnemyFromList(GameObject enemy)
    {
        if (activeEnemy.Contains(enemy))
        {
            activeEnemy.Remove(enemy);
            totalEnemyDestroy++;
            //Debug.Log("Enemy Destroyed: " + totalEnemyDestroy);
        }
        
    }

    public void Difficulty()
    {
        waveDelay = 8+ diff * 1.5f;
        switch (diff)
        {
            case 1:
                
                StartCoroutine(HandleWaves(new System.Action[] { Wave1, Wave2, Wave3 }));
                break;
            case 2:
                StartCoroutine(HandleWaves(new System.Action[] { Wave2, Wave3, Wave4 }));
                break;
            case 3:
                StartCoroutine(HandleWaves(new System.Action[] { Wave3, Wave4, Wave5 }));
                break;
            case 4:
                StartCoroutine(HandleWaves(new System.Action[] { Wave4, Wave5, Wave6 }));
                break;
            case 5:
                StartCoroutine(HandleWaves(new System.Action[] { Wave5, Wave6, Wave7 }));
                break;
            case 6:
                StartCoroutine(HandleWaves(new System.Action[] { Wave6, Wave7, Wave8 }));
                break;
            case 7:
                StartCoroutine(HandleWaves(new System.Action[] { Wave7, Wave8, Wave9 }));
                break;
            case 8:
                StartCoroutine(HandleWaves(new System.Action[] { Wave8, Wave9, Wave10 }));
                break;
            case 9:
                StartCoroutine(HandleWaves(new System.Action[] { Wave9, Wave10, Wave11 }));
                break;
            case 10:
                StartCoroutine(HandleWaves(new System.Action[] { Wave10, Wave11, Wave12 }));
                break;
            case 11:
                StartCoroutine(HandleWaves(new System.Action[] { Wave11, Wave12, Wave13 }));
                break;
            case 12:
                StartCoroutine(HandleWaves(new System.Action[] { Wave12, Wave13, Wave14 }));
                break;
            case 13:
                StartCoroutine(HandleWaves(new System.Action[] { Wave13, Wave14, Wave15 }));
                break;
            case 14:
                StartCoroutine(HandleWaves(new System.Action[] { Wave14, Wave15, Wave16 }));
                break;
            case 15:
                StartCoroutine(HandleWaves(new System.Action[] { Wave15, Wave16, Wave17 }));
                break;
            case 16:
                StartCoroutine(HandleWaves(new System.Action[] { Wave16, Wave17, Wave18 }));
                break;
            case 18:
                StartCoroutine(HandleWaves(new System.Action[] { Wave18, Wave19, Wave20 }));
                break;
            
            default:
                break;
        }
    }

    

    private void Wave1()
    {
        
        
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(1), () => SpawnMeleeMinion(1) }));
        totalWave++;
    }

    private void Wave2()
    {
        
        
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(2), () => SpawnRangedMinion(2) }));
        totalWave++;
    }

    private void Wave3()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(4), () => SpawnMeleeMinion(4) }));
        totalWave++;
    }

    private void Wave4()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(1), () => SpawnMeleeMinion(1), () => SpawnRangedMinion(1) }));
        totalWave++;
    }

    private void Wave5()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(9), () => SpawnMeleeMinion(9) }));
        totalWave++;
    }

    private void Wave6()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(10), () => SpawnRangedMinion(9) }));
        totalWave++;
    }

    private void Wave7()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(36) }));
        totalWave++;
    }

    private void Wave8()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(26), () => SpawnRangedMinion(6) }));
        totalWave++;
    }

    private void Wave9()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(16), () => SpawnSiegeMinion(1), () => SpawnRangedMinion(8) }));
        totalWave++;
    }

    private void Wave10()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(36), () => SpawnRangedMinion(36) }));
        totalWave++;
    }

    private void Wave11()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnSuperMinion(1), () => SpawnRangedMinion(16), () => SpawnRangedMinion(16) }));
        totalWave++;
    }

    private void Wave12()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] {() => SpawnSuperMinion(1), () => SpawnMeleeMinion(26), () => SpawnSiegeMinion(11) , () => SpawnRangedMinion(16) }));
        totalWave++;
    }

    private void Wave13()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnSuperMinion(11), () => SpawnRangedMinion(36), () => SpawnRangedMinion(36) }));
        totalWave++;
    }

    private void Wave14()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnSuperMinion(6), () => SpawnMeleeMinion(36),() => SpawnSiegeMinion(26), () => SpawnRangedMinion(36) }));
        totalWave++;
    }

    private void Wave15()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnMeleeMinion(36), () => SpawnMeleeMinion(36), () => SpawnMeleeMinion(36), () => SpawnSiegeMinion(36) }));
        totalWave++;
    }

    private void Wave16()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnSuperMinion(11), () => SpawnSiegeMinion(36), () => SpawnRangedMinion(36), () => SpawnRangedMinion(36) }));
        totalWave++;
    }

    private void Wave17()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnSuperMinion(36), () => SpawnSiegeMinion(36), () => SpawnSiegeMinion(36) }));
        totalWave++;
    }

    private void Wave18()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnSuperMinion(36),() => SpawnMeleeMinion(36), () => SpawnSiegeMinion(36), () => SpawnRangedMinion(36) }));
        totalWave++;
    }

    private void Wave19()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnSuperMinion(36), () => SpawnSuperMinion(36), () => SpawnSuperMinion(36) }));
        totalWave++;
    }

    private void Wave20()
    {
        StartCoroutine(SpawnWaveWithDelay(new System.Action[] { () => SpawnSuperMinion(36), () => SpawnSuperMinion(36), () => SpawnMeleeMinion(36), () => SpawnMeleeMinion(36), () => SpawnSiegeMinion(36), () => SpawnSiegeMinion(36), () => SpawnRangedMinion(36), () => SpawnRangedMinion(36) }));
        totalWave++;
    }
    private IEnumerator HandleWaves(System.Action[] waves)
    {
        foreach(var wave in waves)
        {
            waveMinionCounts.Clear();
            wave();
            yield return new WaitForSeconds(waveDelay);
        }
        
    }

    private IEnumerator SpawnWaveWithDelay(System.Action[] spawns)
    {
        foreach (var spawn in spawns)
        {
            spawn(); 
            yield return new WaitForSeconds(spawnDelay); 
        }
    }

    private void CheckForVictory()
    {
        if (activeEnemy.Count == 0 && totalWave == 3)
        {
            TowerStats tower = FindObjectOfType<TowerStats>();
            tower.SaveCurrentHp();
            StartCoroutine(WaitBeforeVictory());
        }
    }

    private IEnumerator WaitBeforeVictory()
    {
        yield return new WaitForSeconds(1); 
        gameController.Victory();
        DisplayMinionInfo();
    }
}


