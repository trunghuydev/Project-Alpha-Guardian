using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class ChangeSence : MonoBehaviour
{
    public Button gameButton;          //  chuyển đến scene Game
    public Button inventoryButton;     //  chuyển đến scene Inventory
    public Button exitButton;
    public Button heroInfoButton;
    public Button backToMenuButton;


    private void Start()
    {
       
        if(gameButton != null)
        {
            gameButton.onClick.AddListener(SwitchToGameScene);
        }
        if(inventoryButton != null)
        {
            inventoryButton.onClick.AddListener(SwitchToInventoryScene);
        }
        if(heroInfoButton != null)
        {
            heroInfoButton.onClick.AddListener(SwitchToheroInfoScene);
        }
        if(exitButton != null)
        {
            exitButton.onClick.AddListener(ExitApplication);
        }
        if(backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(SwitchToMenuScene);
        }
        
    }

    public void SwitchToMenuScene()
    {
        SceneManager.LoadScene("StartMenu");
    }

    private void SwitchToGameScene()
    {
        
        SceneManager.LoadScene("Level Select");
    }

    
    private void SwitchToInventoryScene()
    {
       
        SceneManager.LoadScene("Inventory");
    }

    private void SwitchToheroInfoScene()
    {
        SceneManager.LoadScene("HeroInfo");
    }


    private void ExitApplication()
    {
       
        Debug.Log("Thoát ứng dụng");

       
        Application.Quit();

       
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
