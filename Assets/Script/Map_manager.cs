using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class MapLoader : MonoBehaviour
{
    public struct Tile
    {
        public int x;
        public int y;
        public string type;
    }

    public List<Tile> tiles = new List<Tile>(); // Khởi tạo danh sách tiles

    public GameObject tile0;
    public GameObject tile1;
    public GameObject tile2;
    public GameObject tile3;
    public GameObject tile4;
    public GameObject tile5;
    public GameObject tile6;
    public GameObject tile7;
    public GameObject tile8;
    public GameObject tile9;        
    public GameObject tile10;        
    public GameObject tile11;
    public GameObject tile12;
    public GameObject tile13;
    public GameObject tile14;
    public GameObject tile15;
    public GameObject tile16;
    public GameObject tile17;
    public GameObject tile18;
    public GameObject tile19;
    public GameObject tile20;
    public GameObject tile21;
    public GameObject tile22;
    public GameObject tile23;
    public GameObject tile24;
    public GameObject tile25;
    public GameObject tile26;    
    public GameObject tile27;
    public GameObject tile28;
    public GameObject tile29;
    public GameObject tile30;
    public GameObject tile31;

    public GameObject constTile0;
    public GameObject constTile1;
    public GameObject constTile2;
    public GameObject constTile3;
    public GameObject constTile4;
    public GameObject constTile5;


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

    void Start()
    {
        LoadMapData();
        AssignTileImages();
    }

    void LoadMapData()
    {
        string folderPath =  "Assets/Data/ingame_data/map";
        string path;
        int x, y;

        if (Directory.Exists(folderPath))
        {
            for (int i = 0; i < 32; i++)
            {
                path = folderPath+ "/tile" + i + ".txt";

                if (File.Exists(path))
                {
                    string[] lines = File.ReadAllLines(path);
                    if (lines.Length >= 3 &&
                            int.TryParse(lines[0], out x) &&
                            int.TryParse(lines[1], out y))
                    {
                        Tile tile = new Tile { x = x, y = y, type = lines[2] };
                        tiles.Add(tile);
                    }
                    else
                    {
                        Debug.LogError("File format error in: " + path);
                    }
                }
                else
                {
                    Debug.LogError("File not found: " + path);
                }
            }
        }
        else
        {
            Debug.LogError("Folder not found: " + folderPath);
        }
    }

    void AssignTileImages()
    {
        AssignImageToTile(tile0, 0);
        AssignImageToTile(tile1, 1);
        AssignImageToTile(tile2, 2);
        AssignImageToTile(tile3, 3);
        AssignImageToTile(tile4, 4);
        AssignImageToTile(tile5, 5);
        AssignImageToTile(tile6, 6);
        AssignImageToTile(tile7, 7);
        AssignImageToTile(tile8, 8);
        AssignImageToTile(tile9, 9);
        AssignImageToTile(tile10, 10);
        AssignImageToTile(tile11, 11);
        AssignImageToTile(tile12, 12);
        AssignImageToTile(tile13, 13);
        AssignImageToTile(tile14, 14);
        AssignImageToTile(tile15, 15);
        AssignImageToTile(tile16, 16);
        AssignImageToTile(tile17, 17);
        AssignImageToTile(tile18, 18);
        AssignImageToTile(tile19, 19);
        AssignImageToTile(tile20, 20);
        AssignImageToTile(tile21, 21);
        AssignImageToTile(tile22, 22);
        AssignImageToTile(tile23, 23);
        AssignImageToTile(tile24, 24);
        AssignImageToTile(tile25, 25);
        AssignImageToTile(tile26, 26);
        AssignImageToTile(tile27, 27);
        AssignImageToTile(tile28, 28);
        AssignImageToTile(tile29, 29);
        AssignImageToTile(tile30, 30);
        AssignImageToTile(tile31, 31);
        AssignImageToConstTile(constTile0, 0);
        AssignImageToConstTile(constTile1, 1);
        AssignImageToConstTile(constTile2, 1);
        AssignImageToConstTile(constTile3, 1);
        AssignImageToConstTile(constTile4, 2);
        AssignImageToConstTile(constTile5, 3);
    }

    void AssignImageToTile(GameObject tile, int fileOrder)
    {
        string path = "Assets/Data/ingame_data/map/tile" + fileOrder + ".txt";
        string[] lines = File.ReadAllLines(path);
        Sprite sprite = spriteTile0;
        if (tile != null)
        {
            Image imageComponent = tile.GetComponent<Image>();
            if (imageComponent != null)
            {
                switch (lines[2])
                {
                    case "Trống":
                        sprite = spriteTile0;
                        break;

                    case "Sự kiện":
                        sprite = spriteTile1;
                        break;
                    case "Chiến đấu":
                        sprite = spriteTile2;
                        break;
                    case "Cửa hàng":
                        sprite = spriteTile3;
                        break;
                    case "Nhận thức":
                        sprite = spriteTile4;
                        break;
                    case "Rèn":
                        sprite = spriteTile5;
                        break;
                    case "Bất thường":
                        sprite = spriteTile6;
                        break;
                    case "Thưởng":
                        sprite = spriteTile7;
                        break;
                    case "Game":
                        sprite = spriteTile8;
                        break;

                }

                imageComponent.sprite = sprite;

                Color newColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 20f / 255f);

                imageComponent.color = newColor;
            }
            else
            {
                Debug.LogError("Image component not found on tile.");
            }
        }
        else
        {
            Debug.LogError("Tile is null.");
        }
    }

    void AssignImageToConstTile(GameObject tile, int type)
    {
        Sprite sprite = spriteTile0;
        if (tile != null)
        {
            Image imageComponent = tile.GetComponent<Image>();
            if (imageComponent != null)
            {
                switch (type)
                {
                    case 0:
                        sprite = spriteTile9;
                        break;

                    case 1:
                        sprite = spriteTile10;
                        break;
                    case 2:
                        sprite = spriteTile3;
                        break;
                    case 3:
                        sprite = spriteTile11;
                        break;                   

                }

                imageComponent.sprite = sprite;

                Color newColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 20f / 255f);

                imageComponent.color = newColor;
            }
            else
            {
                Debug.LogError("Image component not found on tile.");
            }
        }
        else
        {
            Debug.LogError("Tile is null.");
        }
    }

}
