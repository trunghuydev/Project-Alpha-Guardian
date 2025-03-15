using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class amount_update : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI electric_amount_text;
    string path = "Assets/Data/ingame_data/electric_chip_amount.txt";
    public int electric_amount = 0;

    void Start()
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "50");

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (File.Exists(path))
        {
            File.ReadAllText(path);
            int.TryParse(File.ReadAllText(path), out electric_amount);
            electric_amount_text.text = electric_amount.ToString();
        }
    }
}
