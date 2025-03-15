using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct Tile
    {
        public int x_pos;
        public int y_pos;
        public string type;
    }

    public struct TileRandomLibrary
    {
        public string type;
        public int weight;
        public int quantity;
    }

    public GameObject firstReward;
    private const int MapMaxValue = 32;
    private const int MapConstMaxValue = 6;
    public List<TileRandomLibrary> tilesLib = new List<TileRandomLibrary>();
    public List<Tile> tilesMap = new List<Tile>();

    private readonly int[] xValues = new int[]
    {
        5, 5, 10, 10, 10, 15, 15, 20, 20, 20, 25, 25, 30, 30, 30, 35, 35, 50, 50, 50, 55, 55, 60, 60, 60, 65, 65, 70, 70, 70, 75, 75
    };

    private readonly int[] yValues = new int[]
    {
        5, -5, 10, 0, -10, 5, -5, 10, 0, -10, 5, -5, 10, 0, -10, 5, -5, 10, 0, -10, 5, -5, 10, 0, -10, 5, -5, 10, 0, -10, 5, -5
    };

    void Start()
    {
        LoadTileLibrary();
        InitializeMap();
        AddingConstTile();
    }

    private void LoadTileLibrary()
    {
        string path = "Assets/Data/Library_data/Tile_lib";
        var filePaths = Directory.GetFiles(path).Where(file => !file.EndsWith(".meta")).ToArray();
        int total = 0;

        foreach (string filePath in filePaths)
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length >= 3 &&
                int.TryParse(lines[1], out int quantity) &&
                int.TryParse(lines[2], out int weight))
            {
                TileRandomLibrary tile = new TileRandomLibrary { type = lines[0], quantity = quantity, weight = weight };
                tilesLib.Add(tile);
                total += tile.quantity;

                // Thêm các ô với số lượng ban đầu vào danh sách tilesMap
                for (int i = 0; i < quantity; i++)
                {
                    tilesMap.Add(new Tile { x_pos = 0, y_pos = 0, type = tile.type });
                }
            }
            else
            {
                Debug.LogError($"File format error: {filePath}");
            }
        }

        FillRemainingTiles(total);
    }

    private void FillRemainingTiles(int total)
    {
        while (total < MapMaxValue)
        {
            string tileToAdd = GetRandomTile(tilesLib);
            if (tileToAdd != null)
            {
                tilesMap.Add(new Tile { x_pos = xValues[total], y_pos = yValues[total], type = tileToAdd });
                total++;
            }
            else
            {
                Debug.LogError("Unable to find a valid tile to add.");
                break;
            }
        }
    }

    private void InitializeMap()
    {
        string pathToTile = "Assets/Data/ingame_data/map";
        var existingTiles = Directory.GetFiles(pathToTile).Where(file => !file.EndsWith(".meta")).ToArray();

        if (existingTiles.Length < MapMaxValue)
        {
            ClearDirectory(pathToTile);
            CreateNewFiles(pathToTile);

            firstReward.SetActive(true);
        }
    }

    private void ClearDirectory(string path)
    {
        var files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            try
            {
                File.Delete(file);
                Debug.Log($"Deleted file: {file}");
            }
            catch (IOException ex)
            {
                Debug.LogError($"Error deleting file: {file}\nException: {ex.Message}");
            }
        }
    }

    private void CreateNewFiles(string pathToTile)
    {
        for (int i = 0; i < MapMaxValue; i++)
        {
            string tilePath = Path.Combine(pathToTile, $"tile{i}.txt");
            InitializeTileFile(tilePath, i);
        }
    }

    private void InitializeTileFile(string tilePath, int index)
    {
        if (tilesLib == null || tilesLib.Count == 0)
        {
            Debug.LogError("Tile library is empty or null.");
            return;
        }

        if (tilesMap == null || tilesMap.Count == 0)
        {
            Debug.LogError("Tile map is empty or null.");
            return;
        }

        Tile selectedTile = tilesMap[index];
        selectedTile.x_pos = xValues[index];
        selectedTile.y_pos = yValues[index];
        File.WriteAllText(tilePath, $"{selectedTile.x_pos}\n{selectedTile.y_pos}\n{selectedTile.type}");
        Debug.Log($"Initialized tile file: {tilePath} with x: {selectedTile.x_pos}, y: {selectedTile.y_pos}, type: {selectedTile.type}");
    }

    private string GetRandomTile(List<TileRandomLibrary> tileList)
    {
        int totalWeight = tileList.Sum(tile => tile.weight);
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var tile in tileList)
        {
            cumulativeWeight += tile.weight;
            if (randomValue < cumulativeWeight)
            {
                UpdateTileQuantity(tileList.IndexOf(tile), -1);
                return tile.type;
            }
        }

        return null;
    }

    private void UpdateTileQuantity(int index, int increment)
    {
        if (index >= 0 && index < tilesLib.Count)
        {
            TileRandomLibrary tile = tilesLib[index];
            tile.quantity += increment;
            tilesLib[index] = tile;
        }
        else
        {
            Debug.LogError("Index out of range.");
        }
    }

    private void AddingConstTile()
    {
        string pathToConstTile = "Assets/Data/ingame_data/map_const";
        var existingConstTiles = Directory.GetFiles(pathToConstTile).Where(file => !file.EndsWith(".meta")).ToArray();
        if (existingConstTiles.Length < MapConstMaxValue)
        {
            ClearDirectory(pathToConstTile);
            CreateNewFilesConst(pathToConstTile);
        }
    }

    private void CreateNewFilesConst(string pathToConstTile)
    {
        for (int i = 0; i < MapConstMaxValue; i++)
        {
            string tilePath = Path.Combine(pathToConstTile, $"consttile{i}.txt");
            InitializeConstTile(tilePath, i);
        }
    }

    private void InitializeConstTile(string tilePath, int index)
    {
        switch (index)
        {
            case 0:
                File.WriteAllText(tilePath, "0\n0\nBắt đầu");
                Debug.Log($"Initialized tile file: {tilePath} with x: 0, y: 0, type: Bắt đầu");
                break;
            case 1:
                File.WriteAllText(tilePath, "40\n0\nTinh Anh");
                Debug.Log($"Initialized tile file: {tilePath} with x: 40, y: 0, type: Tinh Anh");
                break;
            case 2:
                File.WriteAllText(tilePath, "45\n5\nTinh Anh");
                Debug.Log($"Initialized tile file: {tilePath} with x: 45, y: 5, type: Tinh Anh");
                break;
            case 3:
                File.WriteAllText(tilePath, "45\n-5\nTinh Anh");
                Debug.Log($"Initialized tile file: {tilePath} with x: 45, y: -5, type: Tinh Anh");
                break;
            case 4:
                File.WriteAllText(tilePath, "80\n0\nCửa hàng");
                Debug.Log($"Initialized tile file: {tilePath} with x: 80, y: 0, type: Cửa hàng");
                break;
            case 5:
                File.WriteAllText(tilePath, "90\n0\nBoss");
                Debug.Log($"Initialized tile file: {tilePath} with x: 90, y: 0, type: Boss");
                break;
        }
    }
}
