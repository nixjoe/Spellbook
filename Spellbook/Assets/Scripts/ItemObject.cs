using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ItemObject : IComparable<ItemObject>
{
    public string name = "New Item";
    // public Sprite icon;
    //public Image image;
    public int buyPrice;
    public int sellPrice;
    public string flavorDescription;
    public string mechanicsDescription;
    // public bool isDefaultItem = false;

    public ItemObject(string newName, /*Image newImage,*/ int newBuyPrice, int newSellPrice,
        string newFlavorDescription, string newMechanicsDescription)
    {
        name = newName;
        //image = newImage;
        buyPrice = newBuyPrice;
        sellPrice = newSellPrice;
        flavorDescription = newFlavorDescription;
        mechanicsDescription = newMechanicsDescription;
    }

    public int CompareTo(ItemObject other) 
    {
        if(other == null)
        {
            return 1;
        }

        return 0;
    }
}
