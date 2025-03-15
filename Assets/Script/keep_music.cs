using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keep_music : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject game_audio;
    public static keep_music instance;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
