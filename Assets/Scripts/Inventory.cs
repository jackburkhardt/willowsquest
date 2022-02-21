using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Capacity = 10;
    public List<Item> Items;
    public bool Full = false;

    void Start()
    {
        Items = new List<Item>();
        UpdateUI();
    }
    
    public bool Store(Item item)
    {
        if (Full) return false;

        Items.Add(item);
        UpdateUI();
        
        // TODO: see if this causes problems. if so, disable renderers and colliders manually
        item.gameObject.SetActive(false);

        if (Items.Count == Capacity) Full = true;

        return true;
    }

    public bool Remove(Item item)
    {
        if (Items.Count == 0 || !Items.Contains(item)) return false;
        if (Full) Full = false;

        Items.Remove(item);
        UpdateUI();

        return true;
    }

    void UpdateUI()
    {
        UIManager ui = FindObjectOfType<UIManager>();
        ui.RenderInventory(ref Items);
    }
}
