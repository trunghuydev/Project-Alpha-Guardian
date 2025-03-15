using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ItemFilterController : MonoBehaviour
{   
    public int idRes;
    string folderPathLib = "Assets/Data/LibraryData";
    string folderPathQuan = "Assets/Data/InventoryData";
    public void LoadItems() {
        if(!GetComponent<Toggle>().isOn) {
            return;
        }
        var Items = InventoryController.instance.itemInventories.OrderByDescending(item => item.rank).ToList();
        foreach(var i in Items) {
            Debug.Log(i.id);
        }
        SorterInventoryController.instance.dropdown.value = 0;
        switch(idRes) {
            case 0: InventoryController.instance.LoadView(Items); InventoryController.instance.inventoryFilterLists = Items; break;
            case 1: InventoryController.instance.LoadView(Items.Where(f => f.id.StartsWith("0")).ToList()); InventoryController.instance.inventoryFilterLists = Items.Where(f => f.id.StartsWith("0")).ToList(); break;
            case 2: InventoryController.instance.LoadView(Items.Where(f => f.id.StartsWith("1")).ToList()); InventoryController.instance.inventoryFilterLists = Items.Where(f => f.id.StartsWith("1")).ToList(); break;
        }
    }

}
