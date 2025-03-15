using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class selection_manager : MonoBehaviour
{
    // Start is called before the first frame update

    string curioRewardPath = "Assets/Data/ingame_data/reward/curioselect.txt";

    int selection=0;
    public GameObject button_anim;
    public GameObject button;

    public GameObject rewardPanel;
    public GameObject rewardappear;

    public Animator animator;

    public Image item_image;
    public TextMeshProUGUI itemquantity;

    public Sprite electric_chip;
    public Sprite contingency_upgrade;

    public GameObject curioCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Selection(int selectIndex)
    {
        if (selectIndex!=0)
        {
            selection = selectIndex;
            button_anim.SetActive(true);
            animator.SetTrigger("trigger");
            button.GetComponent<Button>().interactable = true;
        }
        else
        {
            button_anim.SetActive(false);
        }
    }

    public void ConfirmSelection() 
    {
        switch (selection)
        {
            case 1:
                rewardPanel.SetActive(false);
                RewardScreen("electric_chip",150);
                break;

            case 2:
                rewardPanel.SetActive(false);
                if (File.Exists(curioRewardPath))
                {
                    int.TryParse(File.ReadAllText(curioRewardPath), out int remaining);
                    remaining++;
                    File.WriteAllText(curioRewardPath, remaining.ToString());
                }
                else
                {
                    File.WriteAllText(curioRewardPath, "1");
                }

                break;
                

            case 3:
                rewardPanel.SetActive(false);
                RewardScreen("Contingency_upgrade", 1);
                break;
        }
    }

    void RewardScreen(string itemname, int quantity)
    {
        rewardappear.SetActive(true);
        switch (itemname)
        {
            case "electric_chip":
                item_image.sprite = electric_chip;
                itemquantity.text = quantity.ToString();
                int.TryParse(File.ReadAllText("Assets/Data/ingame_data/electric_chip_amount.txt"), out int chipquantity);
                File.WriteAllText("Assets/Data/ingame_data/electric_chip_amount.txt", (chipquantity + 150).ToString());
                File.WriteAllText("Assets/Data/receive/itemget.txt", "90000");
                break;

            case "Contingency_upgrade":
                item_image.sprite = contingency_upgrade;
                itemquantity.text = quantity.ToString();
                if (!File.Exists("Assets/Data/ingame_data/consume_item/item91000.txt")) 
                {
                    File.WriteAllText("Assets/Data/ingame_data/consume_item/item91000.txt", quantity.ToString());
                }
                else
                {
                    int.TryParse(File.ReadAllText("Assets/Data/ingame_data/consume_item/item91000.txt"),out int current);
                    File.WriteAllText("Assets/Data/ingame_data/consume_item/item91000.txt", (current + quantity).ToString());                 
                }
                File.WriteAllText("Assets/Data/receive/itemget.txt", "91000");
                break;
        }
    }
}
