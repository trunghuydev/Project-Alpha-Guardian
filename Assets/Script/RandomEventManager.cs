using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Event1;
    public GameObject Event2;
    public GameObject Event3;

    void Start()
    {
        CreateRandomPool();


    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateRandomPool()
    {
        int randomResult = Random.Range(1, 4);
        switch (randomResult)
        {
            case 1:
                Event1.SetActive(true);
                break;
            case 2:
                Event2.SetActive(true);
                break;
            case 3:
                Event3.SetActive(true);
                break;
        }
    }
}
