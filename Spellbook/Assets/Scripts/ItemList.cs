using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList
{
    void Start()
    {
        List<ItemObject> listOfItems = new List<ItemObject>();
        // Name, Buy Price, Sell Price, Flavor, Mechanics
        // Image commented out but it's between name and buy. Refer to ItemObject Class.
        listOfItems.Add(new ItemObject("Name", 2, 1, "Flavor", "Mechanics"));
    }
}

