using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroInfo : MonoBehaviour
{
    private string RakanFolderPath = "Assets/Data/Hero_stat/base_stat/rakan";
    private string XayahFolderPath = "Assets/Data/Hero_stat/base_stat/xayah";
    private string folderPathQuan = "Assets/Data/InventoryData";

    // Đường dẫn file cấp độ
    private string Rakanlevel = "Assets/Data/Hero_stat/hero_lv/rakan/Lv.txt";
    private string Xayahlevel = "Assets/Data/Hero_stat/hero_lv/xayah/Lv.txt";

    public TMP_Text[] textUIList; //danh sách thuộc tính
    public TMP_Text[] previewList; //số lượng item
    public TMP_Text[] clickCountTexts; // đếm số lượng click
    public Button levelUpButton; // nút tăng cáp
    public Button resetButton; // nút reset chỉ số
    public Button showRakanFilesButton; 
    public Button showXayahFilesButton; 

    public Button[] itemButtons; // Danh sách các nút cho item

    private string[] specificFiles = { "item10000.txt", "item10001.txt", "item10002.txt" }; 
    private int[] experienceValues = { 10, 100, 1000 }; 
    private int[] clickCounts; 

    
    private bool isRakanSelected = false;
    private bool isXayahSelected = false;

  
    public TMP_Text levelText;
    public TMP_Text expText;

  
    private int attackPerLevel = 10;  
    private int healthPerLevel = 20;  
    private float healthRegenPerLevel = 0.2f;  

    // Tên các file cần cập nhật
    private string attackFile = "Attack.txt";
    private string healthFile = "Health.txt";
    private string healthRegenFile = "HealthRegen.txt";

    private void Start()
    {
        ShowSpecificQuanFiles();

      
        clickCounts = new int[itemButtons.Length];

       
        for (int i = 0; i < itemButtons.Length; i++)
        {
            int index = i; 
            itemButtons[i].onClick.AddListener(() => OnItemClicked(index));
        }

        
        for (int i = 0; i < clickCountTexts.Length; i++)
        {
            clickCountTexts[i].text = " ";
        }
       
        if (levelUpButton != null)
        {
            levelUpButton.onClick.AddListener(LevelUp);
        }
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetHeroStats);  
        }

  
    }

    
    public void ShowRakanFiles()
    {
        isRakanSelected = true;
        isXayahSelected = false; 
        DisplayFilesInFolder(RakanFolderPath, textUIList);

        
        CheckSelectedHero();

        UpdateLevelUpButtonState();
        OpenLevelFile(Rakanlevel);
    }

   
    public void ShowXayahFiles()
    {
        isXayahSelected = true;
        isRakanSelected = false; 
        DisplayFilesInFolder(XayahFolderPath, textUIList);

        UpdateLevelUpButtonState();
        CheckSelectedHero();

       
        OpenLevelFile(Xayahlevel);
    }

    private void DisplayFilesInFolder(string folderPath, TMP_Text[] uiList)
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Thư mục không tồn tại: " + folderPath);
            return;
        }

        Dictionary<string, string> fileNameMapping = new Dictionary<string, string>
        {
            { "Armor", "Phòng Thủ" },
            { "Attack", "Tấn công" },
            { "AttackSpd", "Tốc độ tấn công" },
            { "CritDmg", "Sát thương bạo kích" },
            { "CritRate", "Tỉ lệ bạo kích" },
            { "DmgBoost", "khuếch đại săt thương" },
            { "Health", "Máu" },
            { "HealthRegen", "Hồi máu mỗi giây " },
            { "IgnoreRes", "xuyên kháng thuộc tính" },
            { "Mana", "Năng lượng" },
            { "ManaRegen", "Hiệu suất hồi năng lượng" },
            { "Res", "Kháng thuộc tính " }
        };

        string[] files = Directory.GetFiles(folderPath, "*.txt");
        int count = Mathf.Min(files.Length, uiList.Length);

        for (int i = 0; i < count; i++)
        {
            string content = File.ReadAllText(files[i]);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(files[i]);

          
            if (fileNameMapping.ContainsKey(fileNameWithoutExtension))
            {
                fileNameWithoutExtension = fileNameMapping[fileNameWithoutExtension];
            }

            uiList[i].text = $"{fileNameWithoutExtension}: {content}"; 
        }

        
        for (int i = count; i < uiList.Length; i++)
        {
            uiList[i].text = "";
        }
    }

    public void ShowSpecificQuanFiles()
    {
        DisplaySpecificFiles(folderPathQuan, specificFiles, previewList);
    }

    private void DisplaySpecificFiles(string folderPath, string[] fileNames, TMP_Text[] uiList)
    {
        for (int i = 0; i < fileNames.Length; i++)
        {
            string fullPath = Path.Combine(folderPath, fileNames[i]).Replace("\\", "/");

            if (File.Exists(fullPath))
            {
                string content = File.ReadAllText(fullPath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNames[i]);
                uiList[i].text = content;
            }
            else
            {
                Debug.LogWarning($"File không tồn tại: {fullPath}");
                uiList[i].text = "Không tìm thấy file!";
            }
        }

        for (int i = fileNames.Length; i < uiList.Length; i++)
        {
            uiList[i].text = "";
        }
    }

    private void OnItemClicked(int index)
    {
        clickCounts[index]++;
        clickCountTexts[index].text = $"{clickCounts[index]} ";
    }

    // Hàm kiểm tra trạng thái của Rakan hoặc Xayah
    public void CheckSelectedHero()
    {
        if (isRakanSelected)
        {
            Debug.Log("Rakan đã được chọn.");
        }
        else if (isXayahSelected)
        {
            Debug.Log("Xayah đã được chọn.");
        }
        else
        {
            Debug.Log("Chưa chọn hero.");
        }
    }

    // Hàm mở file cấp độ và hiển thị cấp độ và điểm kinh nghiệm
    private void OpenLevelFile(string levelFilePath)
    {
        if (File.Exists(levelFilePath))
        {
            string[] lines = File.ReadAllLines(levelFilePath);
            if (lines.Length >= 2)
            {
               
                string level = lines[0];
                string exp = lines[1];

                
                levelText.text = "Cấp độ: " + level;
                expText.text = "Điểm kinh nghiệm: " + exp +"/500";
            }
        }
        else
        {
            Debug.LogWarning("File cấp độ không tồn tại: " + levelFilePath);
            levelText.text = "Cấp độ: Không có dữ liệu";
            expText.text = "Điểm kinh nghiệm: Không có dữ liệu";
        }
    }

    // hàm xử lý tăng cấp 
    public void LevelUp()
    {
      
        string heroLevelPath = isRakanSelected ? Rakanlevel : isXayahSelected ? Xayahlevel : null;

        if (string.IsNullOrEmpty(heroLevelPath))
        {
            Debug.LogError("Không có nhân vật nào được chọn!");
            return;
        }

      
        string[] heroData = File.ReadAllLines(heroLevelPath);

        if (heroData.Length < 2)
        {
            Debug.LogError("File cấp độ không đúng định dạng!");
            return;
        }

        int currentLevel = 0;
        int currentExp = 0;

        // Kiểm tra xem giá trị cấp độ và điểm kinh nghiệm có hợp lệ không
        if (!int.TryParse(heroData[0].Trim(), out currentLevel))
        {
            Debug.LogError("Cấp độ không hợp lệ trong file: " + heroData[0]);
            return;
        }

        if (!int.TryParse(heroData[1].Trim(), out currentExp))
        {
            Debug.LogError("Điểm kinh nghiệm không hợp lệ trong file: " + heroData[1]);
            return;
        }

        
        int totalExpGain = 0;

  
        for (int i = 0; i < specificFiles.Length; i++)
        {
            totalExpGain += ProcessItem(specificFiles[i], experienceValues[i], i);
        }

       
        currentExp += totalExpGain;


        // Giới hạn cấp độ tối đa là 36
        while (currentExp >= 500 && currentLevel < 36)
        {
            currentExp -= 500;
            currentLevel++;
        }

        // Nếu đạt cấp độ tối đa, không tăng kinh nghiệm vượt giới hạn
        if (currentLevel >= 36)
        {
            currentLevel = 36;
            currentExp = 0; 
            Debug.Log("Đã đạt cấp độ tối đa!");
        }
        UpdateLevelUpButtonState();


        string[] updatedHeroData = {
        $"{currentLevel}",
        $"{currentExp}"
    };
        File.WriteAllLines(heroLevelPath, updatedHeroData);
     
        IncreaseHeroStats(currentLevel);  
        
        for (int i = 0; i < clickCounts.Length; i++)
        {
            clickCounts[i] = 0;
        }

        
        levelText.text = $"Cấp độ: {currentLevel} ";
        expText.text = $"Kinh nghiệm: {currentExp} / 500";

       
        for (int i = 0; i < clickCountTexts.Length; i++)
        {
            clickCountTexts[i].text = "0"; 
        }

        DisplayFilesInFolder(RakanFolderPath, textUIList);
        ReloadPreviewList();  

    }

    private void IncreaseHeroStats(int level)
    {
        // Xác định thư mục và file cần sửa đổi
        string folderPath = isRakanSelected ? RakanFolderPath : isXayahSelected ? XayahFolderPath : null;

        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("Không có nhân vật nào được chọn!");
            return;
        }

        // Các chỉ số cần sửa đổi và giá trị tăng theo cấp
        string[] statsFiles = { "Attack.txt", "Health.txt", "HealthRegen.txt" };

        
        float[] baseStatsRakan = { 200, 1000, 5 };  
        float[] statIncrementsRakan = { 7, 28, 0.28f };  

        float[] baseStatsXayah = { 200, 1000, 5 }; 
        float[] statIncrementsXayah = { 10, 20, 0.2f }; 

        
        float[] baseStats = isRakanSelected ? baseStatsRakan : baseStatsXayah;
        float[] statIncrements = isRakanSelected ? statIncrementsRakan : statIncrementsXayah;

        
        for (int i = 0; i < statsFiles.Length; i++)
        {
            string filePath = Path.Combine(folderPath, statsFiles[i]); 

            if (File.Exists(filePath))
            {
                
                string fileContent = File.ReadAllText(filePath).Trim();
                float currentStat = 0f;

                if (float.TryParse(fileContent, out currentStat)) 
                {
                   
                    currentStat = baseStats[i] + statIncrements[i] * (level - 1); 

                    
                    File.WriteAllText(filePath, currentStat.ToString());

                   
                    Debug.Log($"Cập nhật {statsFiles[i]}: {currentStat}");
                }
                else
                {
                    Debug.LogError($"Không thể chuyển đổi giá trị trong file: {filePath}");
                }
            }
            else
            {
                Debug.LogWarning($"File không tồn tại: {filePath}");
            }
        }
    }


    private int ProcessItem(string fileName, int experienceValue, int index)
    {
        string itemPath = Path.Combine(folderPathQuan, fileName);

        if (File.Exists(itemPath))
        {
           
            int itemCount = int.Parse(File.ReadAllText(itemPath));

            if (clickCounts[index] > 0)
            {
                if (clickCounts[index] <= itemCount)
                {
                    
                    itemCount -= clickCounts[index];
                    int expGain = clickCounts[index] * experienceValue;

                    
                    File.WriteAllText(itemPath, itemCount.ToString());

                    Debug.Log($"Item {fileName} đã sử dụng {clickCounts[index]} lần. Số lượng còn lại: {itemCount}. Kinh nghiệm cộng: {expGain}");
                    return expGain;
                }
                else
                {
                    Debug.LogWarning($"Không đủ số lượng {fileName} để sử dụng!");
                }
            }
        }
        else
        {
            Debug.LogWarning($"File item không tồn tại: {itemPath}");
        }

        return 0;
    }

    // Hàm reset các chỉ số nhân vật
    public void ResetHeroStats()
    {
        
        string folderPath = isRakanSelected ? RakanFolderPath : isXayahSelected ? XayahFolderPath : null;

        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("Không có nhân vật nào được chọn!");
            return;
        }

        
        if (isRakanSelected)
        {
            
            ResetStat("Attack.txt", 250, folderPath);         
            ResetStat("Health.txt", 270, folderPath);          
            ResetStat("HealthRegen.txt", 7.4f, folderPath);   
        }
        else if (isXayahSelected)
        {
            
            ResetStat("Attack.txt", 340, folderPath);         
            ResetStat("Health.txt", 580, folderPath);         
            ResetStat("HealthRegen.txt", 5.8f, folderPath);   
        }
        DisplayFilesInFolder(RakanFolderPath, textUIList);


        string levelFilePath = isRakanSelected ? Rakanlevel : isXayahSelected ? Xayahlevel : null;
        if (!string.IsNullOrEmpty(levelFilePath) && File.Exists(levelFilePath))
        {
            
            string[] resetLevelData = { "1", "0" };
            File.WriteAllLines(levelFilePath, resetLevelData);
            Debug.Log($"Đã reset cấp độ và kinh nghiệm của {Path.GetFileNameWithoutExtension(levelFilePath)}.");

            
            levelText.text = "Cấp độ: 1";
            expText.text = "Diểm kinh nghiệm: 0 / 500";
        }
        else
        {
            Debug.LogError("Không thể reset cấp độ vì file Lv.txt không tồn tại.");
        }

        UpdateLevelUpButtonState();
        Debug.Log("Đã reset chỉ số nhân vật.");
    }
    // reset chỉ số
    private void ResetStat(string fileName, float newValue, string folderPath)
    {
      
        string filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            
            File.WriteAllText(filePath, newValue.ToString());

            
            Debug.Log($"Đã reset {fileName} với giá trị: {newValue}");
        }
        else
        {
            Debug.LogWarning($"File không tồn tại: {filePath}");
        }
    }

    // Hàm để reload lại previewList sau khi tăng cấp
    private void ReloadPreviewList()
    {
        
        DisplaySpecificFiles(folderPathQuan, specificFiles, previewList);
    }

    private void UpdateLevelUpButtonState()
    {
        string heroLevelPath = isRakanSelected ? Rakanlevel : isXayahSelected ? Xayahlevel : null;

        if (string.IsNullOrEmpty(heroLevelPath))
        {
            Debug.LogError("Không có nhân vật nào được chọn!");
            return;
        }

        if (File.Exists(heroLevelPath))
        {
            string[] heroData = File.ReadAllLines(heroLevelPath);

            if (heroData.Length >= 1 && int.TryParse(heroData[0].Trim(), out int currentLevel))
            {
                levelUpButton.interactable = currentLevel < 36; // Hoạt động nếu cấp < 36
            }
            else
            {
                Debug.LogError("Dữ liệu cấp độ không hợp lệ!");
                levelUpButton.interactable = false;
            }
        }
        else
        {
            Debug.LogError("File cấp độ không tồn tại!");
            levelUpButton.interactable = false;
        }
    }




}
