using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AbnormalManager : MonoBehaviour
{
    string pathToMap = "Assets/Data/ingame_data/map";
    string pathToCurrentTile = "Assets/Data/ingame_data/current_tile.txt";
    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";
    string curioRewardPath = "Assets/Data/ingame_data/reward/curioselect.txt";

    public TextMeshProUGUI currentEChipText;

    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public Button option4Button;

    public GameObject Line1;
    public GameObject Line2;
    public GameObject buttonLine2;
    public GameObject Line3;

    public class Tile
    {
        public int x;
        public int y;
        public string name;
        public string filename;

        public Tile(int x, int y, string name, string filename)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            this.filename = filename;
        }
    }

    List<Tile> changableTiles = new List<Tile>();

    int adjustOption = 0;
    void Start()
    {
        GetTileWhichCanBeChanged();
    }

    void Update()
    {
        UpdateEChipAmount();
        CheckOptionForAvailable();
    }

    void UpdateEChipAmount()
    {
        currentEChipText.text = "Chip Điện tử hiện tại:" + int.Parse(File.ReadAllText(electricChipPath)).ToString();
    }

    void CheckOptionForAvailable()
    {
        int currentEChip = int.Parse(File.ReadAllText(electricChipPath));
        if (currentEChip < 200)
        {
            option1Button.interactable = false;
        }
        if (currentEChip < 100)
        {
            option2Button.interactable = false;
        }
    }

    public void AbnormalSelection(int selection)
    {
        int currentEChip = int.Parse(File.ReadAllText(electricChipPath));

        switch (selection)
        {
            case 1:
                adjustOption = -4;
                currentEChip += -200;
                break;
            case 2:
                adjustOption = -3;
                currentEChip += -100;
                break;
            case 3:
                adjustOption = -2;
                break;
            case 4:
                adjustOption = -1;
                File.WriteAllText(curioRewardPath, "1");
                break;
        }

        Line1.SetActive(false);
        Line2.SetActive(true);
        File.WriteAllText(electricChipPath, currentEChip.ToString());
    }

    public void ShowResult()
    {
        int randomResult = Random.Range(0, 6);

        Debug.Log(randomResult + adjustOption);

        string effect = "";

        switch (adjustOption + randomResult)
        {
            case -4:
                AssignRewardTilesNextToCurrentTile();
                effect = "Jackpot !!! Các ô xung quanh chuyển thành ô Thưởng!";
                break;
            case -3:
                AssignRewardTileNextToCurrentTile();
                effect = "Một ô ngẫu nhiên xung quanh chuyển thành ô Thưởng!";
                break;
            case -2:
                AssignRewardTileToRandomTile();
                effect = "Một ô ngẫu nhiên trên bản đồ chuyển thành ô Thưởng!";
                break;
            case -1:
                AssignShopTileToTheLastTile();
                effect = "Một ô cuối cùng của bản đồ chuyển thành ô Cửa hàng!";
                break;
            case 0:
                SwitchTwoRandomTiles();
                effect = "Hai ô ngẫu nhiên trên bản đồ đổi vị trí cho nhau!";
                break;
            case 1:
                AssignRandomTileToRandomTile();
                effect = "Một ô ngẫu nhiên trên bản đồ chuyên thành một ô ngẫu nhiên khác!";
                break;
            case 2:
                AssignEventTileToTheRandomTiles();
                effect = "Các ô đều có 30% biến thành ô Sự kiện!";
                break;
            case 3:
                AssignCombatTileToTheRandomTiles();
                effect = "Các ô đều có 30% biến thành ô Chiến đấu!";
                break;
            case 4:
                AssignBlankTileToTheRandomTiles();
                effect = "Siêu xui xẻo! Các ô đều có 30% biến thành ô Trống!";
                break;
        }

        buttonLine2.SetActive(false);
        Line3.SetActive(true);

        Line3.GetComponent<TextMeshProUGUI>().text += effect;
    }

    public void ExitAbnormal()
    {
        LoadSceneWithDelay("Adventure", 1f);
    }

    void GetTileWhichCanBeChanged()
    {
        string[] tileFiles = Directory.GetFiles(pathToMap).Where(f => !f.EndsWith(".meta")).ToArray();
        string[] currentTileLines = File.ReadAllLines(pathToCurrentTile);
        int currentPosX = int.Parse(currentTileLines[0]);
        int currentPosY = int.Parse(currentTileLines[1]);

        foreach (string file in tileFiles)
        {
            string[] lines = File.ReadAllLines(file);
            int tilePosX = int.Parse(lines[0]);
            int tilePosY = int.Parse(lines[1]);
            string tileName = lines[2];

            if (tilePosX > currentPosX)
            {
                changableTiles.Add(new Tile(tilePosX, tilePosY, tileName, Path.GetFileName(file)));
            }
        }

        // In ra các ô có thể thay đổi để kiểm tra
        foreach (var tile in changableTiles)
        {
            Debug.Log($"Tile Name: {tile.name}, X: {tile.x}, Y: {tile.y}, Filename: {tile.filename}");
        }
    }

    void SwitchTwoRandomTiles()
    {
        if (changableTiles.Count < 2)
        {
            Debug.LogWarning("Không có đủ ô để chọn.");
            return;
        }

        // Lấy hai chỉ số ngẫu nhiên khác nhau
        int firstIndex = Random.Range(0, changableTiles.Count);
        int secondIndex;
        do
        {
            secondIndex = Random.Range(0, changableTiles.Count);
        } while (secondIndex == firstIndex);

        // Lấy hai ô ngẫu nhiên từ danh sách
        Tile firstTile = changableTiles[firstIndex];
        Tile secondTile = changableTiles[secondIndex];

        Debug.Log($"First Random Tile: {firstTile.name}, X: {firstTile.x}, Y: {firstTile.y}, Filename: {firstTile.filename}");
        Debug.Log($"Second Random Tile: {secondTile.name}, X: {secondTile.x}, Y: {secondTile.y}, Filename: {secondTile.filename}");

        string path1 = pathToMap + "/" + firstTile.filename;
        string path2 = pathToMap + "/" + secondTile.filename;

        string[] lines1 = File.ReadAllLines(path1);
        string[] lines2 = File.ReadAllLines(path2);

        string temp = lines1[2];
        lines1[2] = lines2[2];
        lines2[2] = temp;

        

        File.WriteAllLines(path1, lines1);
        File.WriteAllLines(path2, lines2);

        lines1 = File.ReadAllLines(path1);
        lines2 = File.ReadAllLines(path2);

        Debug.Log($"First Random Tile: {lines1[2]}, X: {firstTile.x}, Y: {firstTile.y}, Filename: {firstTile.filename}");
        Debug.Log($"Second Random Tile: {lines2[2]}, X: {secondTile.x}, Y: {secondTile.y}, Filename: {secondTile.filename}");
    }

    void AssignRandomTileToRandomTile()
    {
        if (changableTiles.Count < 1)
        {
            Debug.LogWarning("Không có đủ ô để chọn.");
            return;
        }

        // Lấy hai chỉ số ngẫu nhiên khác nhau
        int firstIndex = Random.Range(0, changableTiles.Count);

        // Lấy hai ô ngẫu nhiên từ danh sách
        Tile firstTile = changableTiles[firstIndex];

        Debug.Log($"First Random Tile: {firstTile.name}, X: {firstTile.x}, Y: {firstTile.y}, Filename: {firstTile.filename}");

        string path = pathToMap + "/" + firstTile.filename;

        string[] lines = File.ReadAllLines(path);

        int randomResult = Random.Range(1, 10);

        switch (randomResult)
        {
            case 1:
                lines[2] = "Trống";
                break;
            case 2:
                lines[2] = "Sự kiện";
                break;
            case 3:
                lines[2] = "Chiến đấu";
                break;
            case 4:
                lines[2] = "Cửa hàng";
                break;
            case 5:
                lines[2] = "Nhận thức";
                break;
            case 6:
                lines[2] = "Rèn";
                break;
            case 7:
                lines[2] = "Bất thường";
                break;
            case 8:
                lines[2] = "Thưởng";
                break;
            case 9:
                lines[2] = "Game";
                break;
        }

        File.WriteAllLines(path, lines);


        Debug.Log($"Changed Tile: {lines[2]}, X: {firstTile.x}, Y: {firstTile.y}, Filename: {firstTile.filename}");
    }

    void AssignRewardTileToRandomTile()
    {
        if (changableTiles.Count < 1)
        {
            Debug.LogWarning("Không có đủ ô để chọn.");
            return;
        }

        // Lấy hai chỉ số ngẫu nhiên khác nhau
        int firstIndex = Random.Range(0, changableTiles.Count);

        // Lấy hai ô ngẫu nhiên từ danh sách
        Tile firstTile = changableTiles[firstIndex];

        Debug.Log($"First Random Tile: {firstTile.name}, X: {firstTile.x}, Y: {firstTile.y}, Filename: {firstTile.filename}");

        string path = pathToMap + "/" + firstTile.filename;

        string[] lines = File.ReadAllLines(path);

        lines[2] = "Thưởng";


        File.WriteAllLines(path, lines);


        Debug.Log($"Changed Tile: {lines[2]}, X: {firstTile.x}, Y: {firstTile.y}, Filename: {firstTile.filename}");
    }

    void AssignGameTileNextToCurrentTile()
    {
        Tile[] selectTile = new Tile[3];
        int index = 0;
        string[] currentTileLines = File.ReadAllLines(pathToCurrentTile);
        int currentPosX = int.Parse(currentTileLines[0]);
        int currentPosY = int.Parse(currentTileLines[1]);

        foreach (var tile in changableTiles)
        {
            if (tile.x == currentPosX + 5 || (tile.x == currentPosX + 10 && tile.y == currentPosY)){
                selectTile[index] = tile;
                index++;
            }           
        }

        if (index == 0)
        {
            return;
        }

        int randomResult = Random.Range(0, index);

        Tile selectedTile = changableTiles[randomResult];

        Debug.Log($"Changed Tile: {selectedTile.name}, X: {selectedTile.x}, Y: {selectedTile.y}, Filename: {selectedTile.filename}");

        string path = pathToMap + "/" + selectedTile.filename;

        string[] lines = File.ReadAllLines(path);

        lines[2] = "Game";

        File.WriteAllLines(path,lines);

        Debug.Log($"Changed Tile: {lines[2]}, X: {selectedTile.x}, Y: {selectedTile.y}, Filename: {selectedTile.filename}");
    }

    void AssignRewardTileNextToCurrentTile()
    {
        Tile[] selectedTiles = new Tile[3];
        int index = 0;
        string[] currentTileLines = File.ReadAllLines(pathToCurrentTile);
        int currentPosX = int.Parse(currentTileLines[0]);
        int currentPosY = int.Parse(currentTileLines[1]);

        foreach (var tile in changableTiles)
        {
            if (tile.x == currentPosX + 5 || (tile.x == currentPosX + 10 && tile.y == currentPosY))
            {
                selectedTiles[index] = tile;
                index++;
            }
        }

        if (index == 0)
        {
            return;
        }

        int randomResult = Random.Range(0, index);

        Tile selectedTile = selectedTiles[randomResult];

        Debug.Log($"Changed Tile: {selectedTile.name}, X: {selectedTile.x}, Y: {selectedTile.y}, Filename: {selectedTile.filename}");

        string path = pathToMap + "/" + selectedTile.filename;

        string[] lines = File.ReadAllLines(path);

        lines[2] = "Thưởng";

        File.WriteAllLines(path, lines);

        Debug.Log($"Changed Tile: {lines[2]}, X: {selectedTile.x}, Y: {selectedTile.y}, Filename: {selectedTile.filename}");
    }

    void AssignRewardTilesNextToCurrentTile()
    {
        Tile[] selectedTiles = new Tile[3];
        int index = 0;
        string[] currentTileLines = File.ReadAllLines(pathToCurrentTile);
        int currentPosX = int.Parse(currentTileLines[0]);
        int currentPosY = int.Parse(currentTileLines[1]);

        foreach (var tile in changableTiles)
        {
            if (tile.x == currentPosX + 5 || (tile.x == currentPosX + 10 && tile.y == currentPosY))
            {
                Debug.Log(tile.filename);
                selectedTiles[index] = tile;
                index++;
            }
        }

        if (index == 0)
        {
            return;
        }

        for(int i = 0; i < index; i++)
        {
            Tile selectedTile = selectedTiles[i];

            Debug.Log($"Changed Tile: {selectedTile.name}, X: {selectedTile.x}, Y: {selectedTile.y}, Filename: {selectedTile.filename}");

            string path = pathToMap + "/" + selectedTile.filename;

            string[] lines = File.ReadAllLines(path);

            lines[2] = "Thưởng";

            File.WriteAllLines(path, lines);

            Debug.Log($"Changed Tile: {lines[2]}, X: {selectedTile.x}, Y: {selectedTile.y}, Filename: {selectedTile.filename}");
        }       
    }

    void AssignShopTileToTheLastTile()
    {      
        int maxPosX = 0;
        Tile[] selectedTile = new Tile[3];

        foreach (var tile in changableTiles)
        {
            if (tile.x >= maxPosX)
            {
                selectedTile[0] = tile;
                maxPosX = tile.x;
            }
        }

        foreach (var tile in changableTiles)
        {
            if (tile.x == maxPosX && tile != selectedTile[0])
            {
                selectedTile[1] = tile;
                maxPosX = tile.x;
            }
        }

        if (selectedTile[0] == null)
        {
            return;
        }

        int randomResult = Random.Range(0, 2);

        Tile selected = selectedTile[randomResult];

        Debug.Log($"Changed Tile: {selected.name}, X: {selected.x}, Y: {selected.y}, Filename: {selected.filename}");

        string path = pathToMap + "/" + selected.filename;

        string[] lines = File.ReadAllLines(path);

        lines[2] = "Cửa hàng";

        File.WriteAllLines(path, lines);

        Debug.Log($"Changed Tile: {lines[2]}, X: {selected.x}, Y: {selected.y}, Filename: {selected.filename}");
    }

    void AssignBlankTileToTheRandomTiles()
    {
        int numberOfTileChanged = 0;

        foreach (var tile in changableTiles)
        {
            int randomResult = Random.Range(0, 10);

            if (randomResult < 3)
            {

                Debug.Log($"Changed Tile: {tile.name}, X: {tile.x}, Y: {tile.y}, Filename: {tile.filename}");

                string path = pathToMap + "/" + tile.filename;

                string[] lines = File.ReadAllLines(path);

                lines[2] = "Trống";

                File.WriteAllLines(path, lines);

                Debug.Log($"Changed Tile: {lines[2]}, X: {tile.x}, Y: {tile.y}, Filename: {tile.filename}");

                numberOfTileChanged++;
            }
        }
        
        Debug.Log("Number of Tile Changed: " +  numberOfTileChanged);
    }

    void AssignCombatTileToTheRandomTiles()
    {
        int numberOfTileChanged = 0;

        foreach (var tile in changableTiles)
        {
            int randomResult = Random.Range(0, 10);

            if (randomResult < 3)
            {

                Debug.Log($"Changed Tile: {tile.name}, X: {tile.x}, Y: {tile.y}, Filename: {tile.filename}");

                string path = pathToMap + "/" + tile.filename;

                string[] lines = File.ReadAllLines(path);

                lines[2] = "Chiến đấu";

                File.WriteAllLines(path, lines);

                Debug.Log($"Changed Tile: {lines[2]}, X: {tile.x}, Y: {tile.y}, Filename: {tile.filename}");

                numberOfTileChanged++;
            }
        }

        Debug.Log("Number of Tile Changed: " + numberOfTileChanged);
    }

    void AssignEventTileToTheRandomTiles()
    {
        int numberOfTileChanged = 0;

        foreach (var tile in changableTiles)
        {
            int randomResult = Random.Range(0, 10);

            if (randomResult < 3)
            {

                Debug.Log($"Changed Tile: {tile.name}, X: {tile.x}, Y: {tile.y}, Filename: {tile.filename}");

                string path = pathToMap + "/" + tile.filename;

                string[] lines = File.ReadAllLines(path);

                lines[2] = "Sự kiện";

                File.WriteAllLines(path, lines);

                Debug.Log($"Changed Tile: {lines[2]}, X: {tile.x}, Y: {tile.y}, Filename: {tile.filename}");

                numberOfTileChanged++;
            }
        }

        Debug.Log("Number of Tile Changed: " + numberOfTileChanged);
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
