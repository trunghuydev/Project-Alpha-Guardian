using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    // Start is called before the first frame update

    string curioRewardPath = "Assets/Data/ingame_data/reward/curioselect.txt";
    int curioRewardRemaining = 0;
    public GameObject curioSelectCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (File.Exists(curioRewardPath))
        {
            int.TryParse(File.ReadAllText(curioRewardPath), out curioRewardRemaining);
            if (curioRewardRemaining > 0)
            {
                DelayedFunction();
                curioSelectCanvas.SetActive(true);
            }
            else
            {
                curioSelectCanvas.SetActive(false);
            }
        }
        

    }

    IEnumerator DelayedFunction()
    {
        Debug.Log("Starting delay...");
        yield return new WaitForSeconds(1f);  // Đợi 1 giây
        Debug.Log("1 second has passed!");

        // Thực hiện các tác vụ sau khi hết thời gian đợi
        
    }
    
}
