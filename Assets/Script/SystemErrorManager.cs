using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class SystemErrorManager : MonoBehaviour
{
    public TextMeshProUGUI systemPointText;
    public GameObject errorSlot1;
    public GameObject errorSlot2;

    public TextMeshProUGUI errorSelection1;
    public TextMeshProUGUI errorSelection2;

    public GameObject deletedErrorSelection1;
    public GameObject deletedErrorSelection2;

    public TextMeshProUGUI errorEffectPanel;

    public Sprite errorEmpty;
    public Sprite errorAppear;

    public GameObject addErrorPanel;
    public GameObject removeErrorPanel;

    string errorPath = "Assets/Data/ingame_data/error";
    string errorLibPath = "Assets/Data/Library_data/Error_lib";
    int currentErrorQuantity;

    int targetErrorQuantity;

    bool hasErrorAdded = false;
    bool hasErrorRemoved = false;

    // Danh sách để lưu các lỗi ngẫu nhiên được chọn
    private List<string> randomErrors;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string[] errorFiles = Directory.GetFiles(errorPath).Where(f => !f.EndsWith(".meta")).ToArray();
        currentErrorQuantity = errorFiles.Length;

        int.TryParse(File.ReadAllText("Assets/Data/ingame_data/system_point.txt"), out int systemPoint);

        if (systemPoint > 0)
        {
            targetErrorQuantity = 0;
        }
        if (systemPoint <= 0 && systemPoint > -50)
        {
            targetErrorQuantity = 1;
        }
        if (systemPoint <= -50 && systemPoint >= -100)
        {
            targetErrorQuantity = 2;
        }

        if (currentErrorQuantity < targetErrorQuantity && !hasErrorAdded)
        {
            AddNewError();
            hasErrorAdded = true;
            hasErrorRemoved = false; // Reset flag
        }

        if (currentErrorQuantity > targetErrorQuantity && !hasErrorRemoved)
        {
            RemoveError();
            hasErrorRemoved = true;
            hasErrorAdded = false; // Reset flag
        }

        ShowErrorEffect(currentErrorQuantity);

    }

    void ShowErrorEffect(int currentErrorQuantity)
    {
        string[] errorFiles = Directory.GetFiles(errorPath).Where(f => !f.EndsWith(".meta")).ToArray();

        if (currentErrorQuantity > 0 && errorFiles.Length > 0)
        {
            errorSlot1.GetComponent<Button>().onClick.RemoveAllListeners();
            errorSlot1.GetComponent<Button>().onClick.AddListener(() => DisplayErrorEffect(0, errorFiles));

            if (currentErrorQuantity > 1 && errorFiles.Length > 1)
            {
                errorSlot1.SetActive(true);
                errorSlot2.SetActive(true);
                errorSlot2.GetComponent<Button>().onClick.RemoveAllListeners();
                errorSlot2.GetComponent<Button>().onClick.AddListener(() => DisplayErrorEffect(1, errorFiles));
            }
            else
            {
                errorSlot1.SetActive(true);
                errorSlot2.SetActive(false);
            }
        }
        else
        {
            errorSlot1.SetActive(false);
            errorSlot2.SetActive(false);
        }
    }

    void DisplayErrorEffect(int slotIndex, string[] errorFiles)
    {
        if (slotIndex < errorFiles.Length)
        {
            string errorContent = File.ReadAllText(errorFiles[slotIndex]);
            errorEffectPanel.text = "Hiệu ứng lỗi: " + errorContent;
        }
    }

    void AddNewError()
    {
        addErrorPanel.SetActive(true);
        currentErrorQuantity++;
        List<string> allErrors = ReadErrorsFromDirectory(errorLibPath);
        List<string> existingErrors = ReadErrorsFromDirectory(errorPath);

        // Chọn ngẫu nhiên 2 lỗi chưa có
        randomErrors = GetRandomErrors(allErrors, existingErrors);

        // Hiển thị kết quả trên UI
        if (randomErrors.Count >= 2)
        {
            string errorContent1 = File.ReadAllText(randomErrors[0]);
            string errorContent2 = File.ReadAllText(randomErrors[1]);
            errorSelection1.text = errorContent1;
            errorSelection2.text = errorContent2;
        }
    }

    List<string> ReadErrorsFromDirectory(string path)
    {
        List<string> errors = new List<string>();
        var files = Directory.GetFiles(path, "*.txt"); // Chỉ đọc các file .txt

        foreach (var file in files)
        {
            if (!file.EndsWith(".meta")) // Loại bỏ các file .meta
            {
                errors.Add(file);
            }
        }
        return errors;
    }

    // Chọn hai lỗi ngẫu nhiên chưa có
    List<string> GetRandomErrors(List<string> errors, List<string> existingErrors, int numErrors = 2)
    {
        List<string> availableErrors = errors.Where(e => !existingErrors.Contains(Path.GetFileNameWithoutExtension(e))).ToList();
        System.Random rnd = new System.Random();
        return availableErrors.OrderBy(x => rnd.Next()).Take(numErrors).ToList();
    }

    List<string> errorsOwn = new List<string>();
    void RemoveError()
    {
        currentErrorQuantity--;
        removeErrorPanel.SetActive(true);

        var files = Directory.GetFiles(errorPath, "*.txt"); // Chỉ đọc các file .txt

        foreach (var file in files)
        {
            if (!file.EndsWith(".meta")) // Loại bỏ các file .meta
            {
                errorsOwn.Add(file);
            }
        }

        if(errorsOwn.Count >= 1)
        {
            deletedErrorSelection1.SetActive(true);
            deletedErrorSelection1.transform.Find("Error_effect").GetComponent<TextMeshProUGUI>().text = File.ReadAllText(errorsOwn[0]);
        }
        if (errorsOwn.Count >= 2)
        {
            deletedErrorSelection2.SetActive(true);
            deletedErrorSelection2.transform.Find("Error_effect").GetComponent<TextMeshProUGUI>().text = File.ReadAllText(errorsOwn[1]);
        }
    }

    public void ErrorRemoveFromFile(int selection)
    {
        File.Delete(errorsOwn[selection - 1]);
    }
    

    // Hàm để ghi lỗi vào thư mục lỗi đã có
    public void ConfirmErrorSelection(int selectionNumber)
    {
        string selectedErrorPath = randomErrors[selectionNumber - 1]; // Lấy lỗi dựa trên số lựa chọn
        string selectedErrorId = Path.GetFileNameWithoutExtension(selectedErrorPath); // Lấy id của lỗi
        string sourceFilePath = selectedErrorPath; // Đường dẫn file gốc trong thư viện
        string destFilePath = Path.Combine(errorPath, selectedErrorId + ".txt"); // Đường dẫn file đích

        if (File.Exists(sourceFilePath))
        {
            string errorContent = File.ReadAllText(sourceFilePath);
            if (!File.Exists(destFilePath))
            {
                File.WriteAllText(destFilePath, errorContent);
                Debug.Log("Lỗi đã được ghi vào thư mục: " + destFilePath);
                Debug.Log("Lựa chọn số: " + selectionNumber); // In ra số lựa chọn
            }
        }
        else
        {
            Debug.LogWarning("File lỗi gốc không tồn tại: " + sourceFilePath);
        }
    }
}
