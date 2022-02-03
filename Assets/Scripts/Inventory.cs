using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Size = 0;
    public int Capacity = 10;
    public List<InventoryItem> InventoryItems = new List<InventoryItem>();
    public bool Full = false;

    // Update is called once per frame
    void Update()
    {
        
    }
}
