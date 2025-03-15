using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
using Unity.VisualScripting;

public class ItemSlot : MonoBehaviour
{
    public Image RankBG; // Ảnh vật phẩm
    public Image ItemImage;
    public string id;
    public TextMeshProUGUI QuantityText; // Số lượng vật phẩm
    [HideInInspector]
    public int _quan = 0;
    [HideInInspector]
    public Sprite _itemSprite;
    [HideInInspector]
    public string _itemDes;
    [HideInInspector]
    public string _itemName;
    public Toggle toggle;
    public Image ItemSeletedImage;
    public Sprite SelectedSprite;
    public Sprite UnselectedSprite;

    public void Initialize(Sprite itemSprite, string idItem, int quantity, Sprite rankBG, string itemName, string itemDes)
    {
        id = idItem.ToString();
       
        if (itemSprite != null)
            ItemImage.sprite = itemSprite;
        _itemSprite = itemSprite;
       
        QuantityText.text = quantity.ToString();
        _quan = quantity;
        RankBG.sprite = rankBG;
        _itemName = itemName;
        _itemDes = itemDes;




    }

    public void UpdateQuantity(int newQuantity)
    {
       
        _quan = newQuantity;

        
        if (QuantityText != null)
        {
            QuantityText.text = newQuantity.ToString();
        }

        
        if (newQuantity <= 0)
        {
            gameObject.SetActive(false); 
        }

      
    }


    public void ShowDetail()
    {
        
        ItemSeletedImage.sprite = GetComponent<Toggle>().isOn ? SelectedSprite : UnselectedSprite;

        InventoryController.instance.idItemSelected = id;
        
        DetailPanelController.instance.ShowDetails(_itemSprite, _itemName, _itemDes, _quan);
    }


}


public class ItemInventory
{
    public string id;
    public string itemLink;
    public string name;
    public string description;
    public int quanity;
    public int rank;

    public ItemInventory(string id, string itemLink, string name, string description, int quanity, int rank)
    {
        this.id = id;
        this.itemLink = itemLink;
        this.name = name;
        this.description = description;
        this.quanity = quanity;
        this.rank = rank;
    }



}