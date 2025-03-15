using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventData3 : MonoBehaviour
{
    
    string event3Path = "Assets/Data/Library_data/Event_lib/event3.txt";

    int chanceSuccessFox = 0;
    int chanceSuccessUnicorn = 0;

    bool isFoxSuccess = false;
    bool isUnicornSuccess = false;

    public GameObject Line3Fox;
    public GameObject Line3Unicorn;
    public GameObject Line3FoxSuccess;
    public GameObject Line3UnicornSuccess;
    public GameObject Line3FoxFailure;
    public GameObject Line3UnicornFailure;

    public TextMeshProUGUI ChanceFox;
    public TextMeshProUGUI ChanceUnicorn;

    string curioPath = "Assets/Data/ingame_data/curio";
    string event3CurioPath = "Assets/Data/ingame_data/reward/curioselect.txt";
    string event3ElectricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";

    // Start is called before the first frame update

    void Start()
    {
        GetSuccessChance();
        GetEventResult();
        UpdateStat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetSuccessChance()
    {
        string[] lines = File.ReadAllLines(event3Path);

        chanceSuccessFox = int.Parse(lines[3]);
        chanceSuccessUnicorn = int.Parse(lines[5]);
    }

    void UpdateStat()
    {
        ChanceFox.text = "Tỉ lệ xuất hiện: " + chanceSuccessFox.ToString() + "%";
        ChanceUnicorn.text = "Tỉ lệ xuất hiện: " + chanceSuccessUnicorn.ToString() + "%";
    }

    void GetEventResult()
    {
        if (Random.Range(1, 100) <= chanceSuccessFox)
        {
            isFoxSuccess = true;
        }
        if (Random.Range(1, 100) <= chanceSuccessUnicorn)
        {
            isUnicornSuccess = true;
        }
    }

    public void ShowDialog(int selection)
    {
        string[] lines = File.ReadAllLines(event3Path);
        int.TryParse(File.ReadAllText(event3ElectricChipPath), out int electricChip);
        switch (selection)
        {
            case 1:
                Line3Fox.SetActive(true);
                if (isFoxSuccess)
                {
                    Line3FoxSuccess.SetActive(true);
                    Line3FoxFailure.SetActive(false);
                }
                else
                {
                    Line3FoxSuccess.SetActive(false);
                    Line3FoxFailure.SetActive(true);
                }
                break;
            case 2:
                Line3Unicorn.SetActive(true);
                if (isUnicornSuccess)
                {
                    Line3UnicornSuccess.SetActive(true);
                    Line3UnicornFailure.SetActive(false);
                }
                else
                {
                    Line3UnicornSuccess.SetActive(false);
                    Line3UnicornFailure.SetActive(true);
                }
                break;
            case 3:
                chanceSuccessFox += 25;
                chanceSuccessUnicorn += 5;
                if (chanceSuccessFox >= 100)
                {
                    chanceSuccessFox = 100;
                }
                if (chanceSuccessUnicorn >= 100)
                {
                    chanceSuccessUnicorn = 100;
                }
                lines[3] = chanceSuccessFox.ToString();
                lines[5] = chanceSuccessUnicorn.ToString();
                File.WriteAllLines(event3Path, lines);
                LoadSceneWithDelay("Adventure", 1f);
                break;
            case 4:
                LoadSceneWithDelay("Adventure", 1f);
                break;
            case 5:
                File.WriteAllText(event3CurioPath, "1");
                electricChip += 200;
                File.WriteAllText(event3ElectricChipPath, electricChip.ToString());
                chanceSuccessFox = 50;
                lines[3] = chanceSuccessFox.ToString();
                File.WriteAllLines(event3Path, lines);
                LoadSceneWithDelay("Adventure", 1f);
                break;
            case 6:
                File.WriteAllText(event3CurioPath, "1");
                electricChip += 1000;
                File.WriteAllText(event3ElectricChipPath, electricChip.ToString());
                UpdateCurioFiles();
                chanceSuccessUnicorn = 10;
                lines[5] = chanceSuccessUnicorn.ToString();
                File.WriteAllLines(event3Path, lines);
                LoadSceneWithDelay("Adventure", 1f);
                break;
        }
    }

    void UpdateCurioFiles()
    {
        // Lấy tất cả các file trừ file .meta
        string[] dataFiles = Directory.GetFiles(curioPath).Where(f => !f.EndsWith(".meta")).ToArray();

        // Kiểm tra nếu không có file nào
        if (dataFiles.Length == 0)
        {
            Debug.LogWarning("Không có file nào trong thư mục.");
            return;
        }

        // Lọc các file có số khác 100
        var filteredFiles = dataFiles.Where(file =>
        {
            int number = int.Parse(File.ReadAllText(file));
            return number != 100;
        }).ToList();

        // Kiểm tra nếu không có file nào thỏa mãn điều kiện
        if (filteredFiles.Count == 0)
        {
            Debug.LogWarning("Không có file nào có số khác 100.");
            return;
        }

        // Chọn ngẫu nhiên các file từ danh sách đã lọc và nâng số bên trong lên 100
        int filesToProcess = Mathf.Min(3, filteredFiles.Count);
        for (int i = 0; i < filesToProcess; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, filteredFiles.Count);
            string selectedFile = filteredFiles[randomIndex];
            File.WriteAllText(selectedFile, "100");
            Debug.Log($"{selectedFile} đã được nâng lên 100");
            filteredFiles.RemoveAt(randomIndex);
        }

        // Thông báo nếu không đủ 3 file để nâng cấp
        if (filteredFiles.Count < 3)
        {
            Debug.LogWarning("Không đủ 3 file có số khác 100. Đã nâng cấp các file hiện có.");
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
