using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrounClickHandler : MonoBehaviour
{
    GameController gameController;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    void OnMouseDown()
    {
        gameController.HideStats();
        
    }

    
}
