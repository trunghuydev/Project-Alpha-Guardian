using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SystemPointManager : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI systemPointText;
    public GameObject CurrentSystemPoint;
    public Sprite normalSystemPoint;
    public Sprite abnormalSystemPoint;
    int systemPoint;
    string systemPointPath = "Assets/Data/ingame_data/system_point.txt";
    public float duration = 2f;
    int maxSystemPoint;
    int minSystemPoint;
    void Start()
    {
        if(!File.Exists(systemPointPath))
        {
            File.WriteAllText(systemPointPath, "100");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        int.TryParse(File.ReadAllText(systemPointPath), out systemPoint);
        StartCoroutine(CountTo(systemPoint));
        int.TryParse(systemPointText.text, out int systemPointTMP);
        if (systemPointTMP > 0)
        {
            maxSystemPoint = 100;
            minSystemPoint = 0;
            CurrentSystemPoint.GetComponent<Image>().sprite = normalSystemPoint;
        }
        if (-50 < systemPointTMP && systemPointTMP <= 0)
        {
            maxSystemPoint = 0;
            minSystemPoint = -50;
            CurrentSystemPoint.GetComponent<Image>().sprite = abnormalSystemPoint;
        }
        if (-100 <= systemPointTMP && systemPointTMP <= -50)
        {
            maxSystemPoint = -50;
            minSystemPoint = -100;
            CurrentSystemPoint.GetComponent<Image>().sprite = abnormalSystemPoint;
        }

        CurrentSystemPoint.GetComponent<Image>().fillAmount = (float) (systemPointTMP - minSystemPoint) / (maxSystemPoint - minSystemPoint);
    }

    IEnumerator CountTo(int targetNumber)
        {
            int.TryParse(systemPointText.text, out int startNumber);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                int currentNumber = Mathf.RoundToInt(Mathf.Lerp(startNumber, targetNumber, t));
                systemPointText.text = currentNumber.ToString();
                yield return null;
            }

            // Đảm bảo giá trị cuối cùng là số mong muốn
            systemPointText.text = targetNumber.ToString();
        }
}
