using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keep_ambient : MonoBehaviour
{

    public GameObject game_audio;
    public static keep_ambient instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(game_audio);
        }
        else
        {
            Destroy(game_audio);
        }

    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
