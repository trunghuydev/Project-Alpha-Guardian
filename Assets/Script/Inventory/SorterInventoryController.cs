using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SorterInventoryController : MonoBehaviour
{   
    public static SorterInventoryController instance;
    public TMP_Dropdown dropdown;

    void Awake() {
        instance = this;
    }
    
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void SortItem() {
        if(dropdown.value == 0) {
            InventoryController.instance.LoadView(InventoryController.instance.inventoryFilterLists.OrderByDescending(f=> f.rank).ToList()); 
            return;
        }
        InventoryController.instance.LoadView(InventoryController.instance.inventoryFilterLists.Where(i => i.rank == dropdown.value).ToList());
    }
    
    void Update()
    {
        
    }
}
