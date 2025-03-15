using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reward1Data : MonoBehaviour
{
    public TextMeshProUGUI firstNumber;
    public TextMeshProUGUI secondNumber;
    public TextMeshProUGUI thirdNumber;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI remainingAttemptText;

    public TextMeshProUGUI currentEChipText;

    bool isFirstActivate = false;
    bool isSecondActivate = false;
    bool isThirdActivate = false;

    public Button StartMachine;
    public Button StopMachine;
    public GameObject ExitButton;

    int number1 = 1;
    int number2 = 1;
    int number3 = 1;

    int resultChip;

    int remainingAttempt = 3;

    string electricChipPath = "Assets/Data/ingame_data/electric_chip_amount.txt";

    void Start()
    {

    }

    void Update()
    {
        remainingAttemptText.text = "Số lần quay còn lại: " + remainingAttempt;
        if (isFirstActivate)
        {
            number1 = Random.Range(1, 4);
        }
        if (isSecondActivate)
        {
            number2 = Random.Range(1, 4);
        }
        if (isThirdActivate)
        {
            number3 = Random.Range(1, 4);
        }

        firstNumber.text = number1.ToString();
        secondNumber.text = number2.ToString();
        thirdNumber.text = number3.ToString();

        UpdateEChipAmount();
    }

    void UpdateEChipAmount()
    {
        currentEChipText.text = "Chip Điện tử hiện tại: " + int.Parse(File.ReadAllText(electricChipPath)).ToString();
    }

    public void StartTheMachine()
    {
        StartCoroutine(StartTheMachineCoroutine());
    }

    public void StopTheMachine()
    {
        StartCoroutine(StopTheMachineCoroutine());
    }

    IEnumerator StartTheMachineCoroutine()
    {
        resultText.text = "";
        remainingAttempt--;
        StopMachine.interactable = false;
        yield return new WaitForSeconds(0.2f);
        isFirstActivate = true;
        Debug.Log("First activated");

        yield return new WaitForSeconds(0.2f);
        isSecondActivate = true;
        Debug.Log("Second activated");

        yield return new WaitForSeconds(0.2f);
        isThirdActivate = true;
        Debug.Log("Third activated");
        StopMachine.interactable = true;
    }

    IEnumerator StopTheMachineCoroutine()
    {
        StartMachine.interactable = false;
        yield return new WaitForSeconds(1f);
        isThirdActivate = false;
        Debug.Log("Third deactivated");

        yield return new WaitForSeconds(1f);
        isSecondActivate = false;
        Debug.Log("Second deactivated");

        yield return new WaitForSeconds(1.5f);
        isFirstActivate = false;
        Debug.Log("First deactivated");

        if (remainingAttempt > 0)
        {
            StartMachine.interactable = true;
        }
        else
        {
            ExitButton.SetActive(true);
            StartMachine.gameObject.SetActive(false);
        }   
        
        resultChip = number1 * 100 + number2 * 10 + number3;
        if(resultChip % 111 == 0)
        {
            resultChip = 400;
            resultText.text = "Jackpot!!! ";
        }


        resultText.text += "+" + resultChip.ToString() + " Chip Điện tử";
        int.TryParse(File.ReadAllText(electricChipPath), out int currentChip);
        currentChip += resultChip;
        File.WriteAllText(electricChipPath, currentChip.ToString());

    }

    public void ExitReward()
    {
        LoadSceneWithDelay("Adventure", 1f);
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

