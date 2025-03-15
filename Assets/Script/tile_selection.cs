using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class tile_selection : MonoBehaviour
{
    public GameObject panel;
    public Animator animator;
    public GameObject button;
    public Animator buttonanim;
    public GameObject block;
    public GameObject transition;

    public Sprite spriteTile0;
    public Sprite spriteTile1;
    public Sprite spriteTile2;
    public Sprite spriteTile3;
    public Sprite spriteTile4;
    public Sprite spriteTile5;
    public Sprite spriteTile6;
    public Sprite spriteTile7;
    public Sprite spriteTile8;
    public Sprite spriteTile9;
    public Sprite spriteTile10;
    public Sprite spriteTile11;

    private string currentTileType = "";
    private int des_x = -100, des_y = -100;

    string curioDataPath = "Assets/Data/ingame_data/curio";
    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentTileType != "")
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void GetNextTileInfo(string tiletype)
    {
        animator.SetTrigger("appear");

        string path_nor = "Assets/Data/ingame_data/map";
        string path_const = "Assets/Data/ingame_data/map_const";
        string tilePath;

        if (tiletype.Contains("const"))
        {
            tilePath =  path_const + "/" + tiletype + ".txt" ;            
        }
        else
        {
            tilePath = path_nor + "/" + tiletype + ".txt";
        }

        string[] lines = File.ReadAllLines(tilePath);

        currentTileType = lines[2];

        int.TryParse(lines[0], out des_x);
        int.TryParse(lines[1], out des_y);

        panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Text").gameObject.GetComponent<TextMeshProUGUI>().text = currentTileType;

        switch (currentTileType)
        {
            case "Trống":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile0;
                break;

            case "Sự kiện":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile1;
                break;
            case "Chiến đấu":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile2;
                break;
            case "Cửa hàng":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile3;
                break;
            case "Nhận thức":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile4;
                break;
            case "Rèn":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile5;
                break;
            case "Bất thường":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile6;
                break;
            case "Thưởng":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile7;
                break;
            case "Game":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile8;
                break;
            case "Bắt đầu":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile9;
                break;
            case "Tinh Anh":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile10;
                break;
            case "Boss":
                panel.gameObject.transform.Find("BG").gameObject.transform.Find("Type Image").gameObject.GetComponent<Image>().sprite = spriteTile11;
                break;           
        }


            button.SetActive(true);
            buttonanim.SetTrigger("trigger");

    }



    public void Selected()
    {
        string path_current = "Assets/Data/ingame_data/current_tile.txt";
        string systemPointPath = "Assets/Data/ingame_data/system_point.txt";

        if (des_x != -100 && des_y != -100)
        {
            File.WriteAllText(path_current, des_x + "\n" + des_y);
            block.SetActive(true);

            // Đọc giá trị hệ thống điểm từ tệp
            if (int.TryParse(File.ReadAllText(systemPointPath), out int systemPoint))
            {
                // Thực hiện phép trừ mỗi khi hàm được gọi
                systemPoint -= 10;

                // Đảm bảo rằng giá trị không nhỏ hơn -100
                if (systemPoint < -100)
                {
                    systemPoint = -100;
                }

                // Ghi giá trị hệ thống điểm cập nhật trở lại tệp
                File.WriteAllText(systemPointPath, systemPoint.ToString());
            }

            transition.SetActive(true);
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        switch (currentTileType)
        {
            case "Trống":
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
                transition.SetActive(false);
                break;
            case "Sự kiện":
                LoadSceneWithDelay("Adventure_Event", 1f);
                break;
            case "Chiến đấu":
                LoadSceneWithDelay("Adventure_Combat", 1f);
                break;
            case "Cửa hàng":
                LoadSceneWithDelay("Adventure_Shop", 1f);
                break;
            case "Nhận thức":
                LoadSceneWithDelay("Adventure_Incognito", 1f);
                break;
            case "Rèn":
                LoadSceneWithDelay("Adventure_Forge", 1f);
                break;
            case "Bất thường":
                LoadSceneWithDelay("Adventure_Abnormal", 1f);
                break;
            case "Thưởng":
                LoadSceneWithDelay("Adventure_Reward", 1f);
                break;
            case "Game":
                LoadSceneWithDelay("Adventure_Game", 1f);
                break;
            case "Bắt đầu":
                currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
                transition.SetActive(false);
                break;
            case "Tinh Anh":
                LoadSceneWithDelay("Adventure_Combat", 1f);
                break;
            case "Boss":
                LoadSceneWithDelay("Adventure_Combat", 1f);
                break;

        } 
    }

    public void LoadSceneWithDelay(string sceneName, float delay)
    {
        string path = curioDataPath + "/curio2.txt";
        if(File.Exists(path))
        {
            int completion = int.Parse(File.ReadAllText(path));
            if (completion == 100)
            {
                int currentEChip = int.Parse(File.ReadAllText(electricChipPath));
                File.WriteAllText(electricChipPath, (currentEChip * 1.12f).ToString("0"));
            }
        }

        StartCoroutine(LoadSceneAfterDelay(sceneName, delay));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay) 
    { 
        yield return new WaitForSeconds(delay); 
        SceneManager.LoadScene(sceneName); 
    }
}

