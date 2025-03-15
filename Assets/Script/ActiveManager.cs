using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;


[DefaultExecutionOrder(200)]
public class ActiveManager : MonoBehaviour
{
    public Sprite currentImage;
    public GameObject current;

    int tile_x_pos = 0, tile_y_pos = 0;

    string current_tile = "";
    
    string next_tile = "";
    string next_tile_1 = "";
    string next_tile_2 = "";

    // Start is called before the first frame update


    void Start()
    {
        string current_tile_path = "Assets/Data/ingame_data/current_tile.txt";

        if (File.Exists(current_tile_path))
        {
            string[] lines = File.ReadAllLines(current_tile_path);
            int.TryParse(lines[0], out tile_x_pos);
            int.TryParse(lines[1], out tile_y_pos);
        }
        else
        {
            File.WriteAllText(current_tile_path, "0");
            File.AppendAllText(current_tile_path, "\n0");
        }
        FindNextTile();
        SetTileActive();
    }

    // Update is called once per frame
    void Update()
    {
            string current_tile_path = "Assets/Data/ingame_data/current_tile.txt";



        if (File.Exists(current_tile_path))
        {
            string[] lines = File.ReadAllLines(current_tile_path);
            int.TryParse(lines[0], out tile_x_pos);
            int.TryParse(lines[1], out tile_y_pos);

            current.GetComponent<RectTransform>().position = new Vector3(460 + tile_x_pos * 12, 640 + tile_y_pos * 20, 0);
        }



    }



    void FindNextTile()
    {
        string folderPath = "Assets/Data/ingame_data/map";
        string folderPath2 = "Assets/Data/ingame_data/map_const";
        var filePaths = Directory.GetFiles(folderPath).Where(file => !file.EndsWith(".meta")).ToArray();
        var filePaths2 = Directory.GetFiles(folderPath2).Where(file => !file.EndsWith(".meta")).ToArray();

        next_tile = "";
        next_tile_1 = "";
        next_tile_2 = "";
        if (Directory.Exists(folderPath))
        {
            foreach (string filePath in filePaths)
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length >= 3 &&
                    int.TryParse(lines[0], out int x_pos) &&
                    int.TryParse(lines[1], out int y_pos))
                {
                    if (x_pos == tile_x_pos && y_pos == tile_y_pos)
                    {
                        current_tile = Path.GetFileNameWithoutExtension(filePath);
                        Debug.Log("Ô hiện tại: " + current_tile);
                    }

                    if ((x_pos == tile_x_pos + 5 && y_pos == tile_y_pos + 5) || (x_pos == tile_x_pos + 5 && y_pos == tile_y_pos - 5) || (x_pos == tile_x_pos + 10 && y_pos == tile_y_pos))
                    {
                        if (next_tile == "")
                        {
                            next_tile = Path.GetFileNameWithoutExtension(filePath);
                            Debug.Log("Tìm thấy ô tiếp theo: " + next_tile);
                        }
                        else
                        {
                            if (next_tile_1 == "")
                            {
                                next_tile_1 = Path.GetFileNameWithoutExtension(filePath);
                                Debug.Log("Tìm thấy ô tiếp theo: " + next_tile_1);
                            }
                            else
                            {
                                if (next_tile_2 == "")
                                {
                                    next_tile_2 = Path.GetFileNameWithoutExtension(filePath);
                                    Debug.Log("Tìm thấy ô tiếp theo: " + next_tile_2);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError($"File format error: {filePath}");
                }
            }
        }

        if (Directory.Exists(folderPath2))
        {
            foreach (string filePath in filePaths2)
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length >= 3 &&
                    int.TryParse(lines[0], out int x_pos) &&
                    int.TryParse(lines[1], out int y_pos))
                {
                    if (x_pos == tile_x_pos && y_pos == tile_y_pos)
                    {
                        current_tile = Path.GetFileNameWithoutExtension(filePath);
                        Debug.Log("Ô hiện tại: " +  current_tile);
                    }



                    if ((x_pos == tile_x_pos + 5 && y_pos == tile_y_pos + 5) || (x_pos == tile_x_pos + 5 && y_pos == tile_y_pos - 5) || (x_pos == tile_x_pos + 10 && y_pos == tile_y_pos))
                    {
                        if (next_tile == "")
                        {
                            next_tile = Path.GetFileNameWithoutExtension(filePath);
                            Debug.Log("Tìm thấy ô tiếp theo: " + next_tile);
                        }
                        else
                        {
                            if (next_tile_1 == "")
                            {
                                next_tile_1 = Path.GetFileNameWithoutExtension(filePath);
                                Debug.Log("Tìm thấy ô tiếp theo: " + next_tile_1);
                            }
                            else
                            {
                                if (next_tile_2 == "")
                                {
                                    next_tile_2 = Path.GetFileNameWithoutExtension(filePath);
                                    Debug.Log("Tìm thấy ô tiếp theo: " + next_tile_2);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError($"File format error: {filePath}");
                }
            }
        }


    }

    string GetTileName(string tile_name)
    {
        string obj_name;
        if (string.IsNullOrEmpty(tile_name))
        {
            return "none";
        }
        if (GetTileType(tile_name))
        {
            obj_name = "Tile_const (" + ExtractNumberFromString(tile_name) + ")";
        }
        else
        {
            obj_name = "Tile (" + ExtractNumberFromString(tile_name) + ")";
        }
        return obj_name;
    }

    public static int ExtractNumberFromString(string input)
    { 
        string numberString = Regex.Match(input, @"\d+").Value; 
        return int.Parse(numberString);
    }

    public static bool GetTileType (string input) 
    { 
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Chuỗi không được rỗng.");
        }

        char targetChar = 'c';
        // Lấy ký tự đầu tiên của chuỗi
        char firstChar = input[0];

        // So sánh ký tự đầu tiên với ký tự mục tiêu

        return firstChar == targetChar;
    }

    void SetTileActive()
    {
        string tileCur = GetTileName(current_tile);
        string tileNext1 = GetTileName(next_tile);
        string tileNext2 = GetTileName(next_tile_1);
        string tileNext3 = GetTileName(next_tile_2);

        if (tileCur != "none")
        {
            SetButtonCurrent(tileCur);
        }
        if (tileNext1 != "none")
        {
            SetButtonNext(tileNext1);
        }
        if (tileNext2 != "none")
        {
            SetButtonNext(tileNext2);
        }
        if (tileNext3 != "none")
        {
            SetButtonNext(tileNext3);
        }
    }

    void SetButtonNext(string parentName)
    {
        Debug.Log(parentName);

        GameObject parentObject = GameObject.Find(parentName);

        if (parentObject != null)
        {
            parentObject.GetComponent<Button>().interactable = true;

            Color newColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

            Image parentImage = parentObject.GetComponent<Image>();
            if (parentImage != null)
            {
                parentImage.color = newColor;
                Image childImage = parentObject.gameObject.transform.Find("type").gameObject.GetComponent<Image>();
                childImage.color = newColor;
            }
            else
            {
                Debug.LogError("Image component not found on parent object.");
            }
        }
        else
        {
            Debug.LogError("Parent GameObject not found.");
        }
    }


    void SetButtonCurrent(string parentName)
    {
        GameObject parentObject = GameObject.Find(parentName);

        Color newColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

        parentObject.GetComponent<Image>().color = newColor;

        parentObject.GetComponent<Button>().interactable = false;

        if (parentObject != null)
        {
            // Tìm GameObject con cụ thể theo tên
            Transform childTransformtype = parentObject.transform.Find("type");

            childTransformtype.gameObject.GetComponent<Image>().color = newColor;
        }
        else
        {
            Debug.LogError("Parent GameObject not found.");
        }           
    }



}
