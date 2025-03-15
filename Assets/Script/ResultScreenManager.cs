using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScreenManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject tileTraveledBar;
    public TextMeshProUGUI tileTraveledNum;
    public TextMeshProUGUI currentChipNum;
    public TextMeshProUGUI currentSystemPointNum;
    public TextMeshProUGUI totalPotionNum;
    public TextMeshProUGUI towerHealthNum;
    public GameObject towerHealthBar;
    public TextMeshProUGUI curioNum;
    public TextMeshProUGUI errorNum;
    public GameObject hero1Image;
    public GameObject hero2Image;

    public GameObject curioContent;

    public GameObject errorSlot1;
    public GameObject errorSlot2;
    public TextMeshProUGUI errorDisplayText;

    public Sprite perfectedCorner;
    public Sprite perfectedBorder;

    string mapPath = "Assets/Data/ingame_data/map";
    string mapConstPath = "Assets/Data/ingame_data/map_const";

    string rewardCurioPath = "Assets/Data/ingame_data/reward";
    string consumeItemPath = "Assets/Data/ingame_data/consume_item";
    string currentTilePath = "Assets/Data/ingame_data/current_tile.txt";
    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";
    string systemPointPath = "Assets/Data/ingame_data/system_point.txt";
    string currentHealthPath = "Assets/Data/Hero_stat/ingame_stat/tower/TowerCurrentHealth.txt";
    string maxHealthPath = "Assets/Data/Hero_stat/ingame_stat/tower/TowerMaxHealth.txt";
    string curioPath = "Assets/Data/ingame_data/curio";
    string potionPath = "Assets/Data/ingame_data/potion";
    string curioImagePath = "Assets/Data/curio_sprite";
    string errorPath = "Assets/Data/ingame_data/error";
    string hero1Path = "Assets/Data/hero_select/hero1.txt";
    string hero2Path = "Assets/Data/hero_select/hero2.txt";
    string pathToPurchasedPotionCount = "Assets/Data/ingame_data/purchased_potion_count.txt";

    public class Curio
    {
        public string imagePath;
        public int completion;

        public Curio(string path, int comp)
        {
            imagePath = path;
            completion = comp;
        }
    }

    public List<Curio> curioList = new List<Curio>();

    void Start()
    {
        AssignDataToScreen();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AssignDataToScreen()
    {
        string[] line = File.ReadAllLines(currentTilePath);
        int tile = int.Parse(line[0]);
        if(tile >=90)
        {
            tile += -5;
        }
        tileTraveledNum.text = tile /5 + "/18";
        tileTraveledBar.GetComponent<Image>().fillAmount = tile / 90f;
        int.TryParse(File.ReadAllText(electricChipPath), out int currentChip);
        currentChipNum.text = currentChip.ToString();
        int.TryParse(File.ReadAllText(systemPointPath), out int systemPoint);
        currentSystemPointNum.text = systemPoint.ToString();
        string[] potionPaths = Directory.GetFiles(potionPath).Where(f => !f.EndsWith(".meta")).ToArray();
        totalPotionNum.text = potionPaths.Length.ToString();
        int.TryParse(File.ReadAllText(currentHealthPath), out int currentHealth);
        int.TryParse(File.ReadAllText(maxHealthPath), out int maxHealth);
        towerHealthBar.GetComponent<Image>().fillAmount = (float) currentHealth / maxHealth;
        towerHealthNum.text = currentHealth + "/" + maxHealth;


        string[] curioPaths = Directory.GetFiles(curioPath).Where(f => !f.EndsWith(".meta")).ToArray();        
        curioNum.text = curioPaths.Length.ToString();

        for (int i = 0; i < curioPaths.Length; i++)
        {
            GameObject curio = curioContent.transform.Find("ItemSlot" + (i + 1)).gameObject;
            curio.gameObject.SetActive(true);
            int.TryParse(File.ReadAllText(curioPaths[i]), out int completion);
            string curioid = Path.GetFileNameWithoutExtension(curioPaths[i]);
            string curioImage = curioImagePath + "/" + curioid + ".png";
            AssignInformationToCurioList(curioImage, completion);
        }

        for (int i = 0; i < curioPaths.Length; i++)
        {
            GameObject curio = curioContent.transform.Find("ItemSlot" + (i + 1)).gameObject;
            AssignImageToCurioSlot(curioList[i].imagePath, curio.transform.Find("Image").gameObject);
            curio.transform.Find("CompletionNumber").GetComponent<TextMeshProUGUI>().text = curioList[i].completion + "%";
            if(curioList[i].completion == 100)
            {
                curio.transform.Find("Completion").GetComponent<Image>().sprite = perfectedCorner;
                curio.GetComponent<Image>().sprite = perfectedBorder;
            }
        }



        string[] errorPaths = Directory.GetFiles(errorPath).Where(f => !f.EndsWith(".meta")).ToArray();

        if(errorPaths.Length > 0) 
        {
            errorSlot1.SetActive(true);
        }
        if (errorPaths.Length > 1)
        {
            errorSlot2.SetActive(true);
        }

        errorNum.text = errorPaths.Length.ToString();

        string hero1id = File.ReadAllText(hero1Path);
        string hero2id = File.ReadAllText(hero2Path);

        if(hero1id != "")
        {
            AssignImageToHeroSlot(hero1id, hero1Image);
        }
        if (hero2id != "")
        {
            AssignImageToHeroSlot(hero2id, hero2Image);
        }


    }


    public void ReturnMainMenu()
    {
        ClearAllData();
        LoadSceneWithDelay("Level Select", 1f);
    }

    void ClearAllData()
    {
        File.WriteAllText(currentTilePath,"0\n0");
        File.WriteAllText(electricChipPath, "0");
        File.WriteAllText(systemPointPath, "100");
        File.WriteAllText(currentHealthPath, "30000");
        File.WriteAllText(maxHealthPath, "30000");
        File.WriteAllText(pathToPurchasedPotionCount, "0");

        if (Directory.Exists(rewardCurioPath))
        {
            string[] files = Directory.GetFiles(rewardCurioPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Đã xoá: {file}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Lỗi khi xóa {file}: {e.Message}");
                }
            }
        }
        if (Directory.Exists(curioPath))
        {
            string[] files = Directory.GetFiles(curioPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Đã xoá: {file}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Lỗi khi xóa {file}: {e.Message}");
                }
            }
        }
        if (Directory.Exists(potionPath))
        {
            string[] files = Directory.GetFiles(potionPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Đã xoá: {file}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Lỗi khi xóa {file}: {e.Message}");
                }
            }
        }
        if (Directory.Exists(errorPath))
        {
            string[] files = Directory.GetFiles(errorPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Đã xoá: {file}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Lỗi khi xóa {file}: {e.Message}");
                }
            }
        }
        if (Directory.Exists(mapPath))
        {
            string[] files = Directory.GetFiles(mapPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Đã xoá: {file}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Lỗi khi xóa {file}: {e.Message}");
                }
            }
        }
        if (Directory.Exists(mapConstPath))
        {
            string[] files = Directory.GetFiles(mapConstPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Đã xoá: {file}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Lỗi khi xóa {file}: {e.Message}");
                }
            }
        }
        if (Directory.Exists(consumeItemPath))
        {
            string[] files = Directory.GetFiles(consumeItemPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Đã xoá: {file}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Lỗi khi xóa {file}: {e.Message}");
                }
            }
        }
        if (Directory.Exists("Assets/Data/hero_select"))
        {
            string[] files = Directory.GetFiles("Assets/Data/hero_select");

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Đã xoá: {file}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Lỗi khi xóa {file}: {e.Message}");
                }
            }
        }
    }


    public void DisplayErrorEffect(int index)
    {
        string[] errorPaths = Directory.GetFiles(errorPath).Where(f => !f.EndsWith(".meta")).ToArray();
        errorDisplayText.text = File.ReadAllText(errorPaths[index]);
    }


    void AssignInformationToCurioList(string imagePath, int completion)
    {
        AddCurioToList(new Curio(imagePath, completion));
        curioList.Sort((a, b) => b.completion.CompareTo(a.completion));
    }

    void AddCurioToList(Curio newCurio)
    {
        // Kiểm tra xem đối tượng Curio đã có trong danh sách chưa
        bool exists = curioList.Exists(curio => curio.imagePath == newCurio.imagePath
                                                && curio.completion == newCurio.completion);
        if (!exists)
        {
            curioList.Add(newCurio);
        }
    }


    void AssignImageToCurioSlot(string path, GameObject image)
    {
        string image_path = path;

        byte[] imageData = File.ReadAllBytes(image_path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        image.GetComponent<Image>().sprite = newSprite;
    }
    void AssignImageToHeroSlot(string heroid, GameObject hero)
    {
        string image_path = "Assets/Data/hero_sprite/hero" + heroid + ".jpg";

        byte[] imageData = File.ReadAllBytes(image_path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        hero.GetComponent<Image>().sprite = newSprite;
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
