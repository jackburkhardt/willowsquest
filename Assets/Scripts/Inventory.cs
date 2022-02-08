using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Capacity = 10;
    public List<Item> Items = new List<Item>();
    public bool Full = false;
    
    private bool _isOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager ui = FindObjectOfType<UIManager>();
            if (_isOpen) 
            {
                ui.CloseInventory();
                _isOpen = false;
            } 
            else 
            {
                ui.OpenInventory(ref Items);
                _isOpen = true;
            }
        }
    }

    public bool Store(Item item)
    {
        if (Full) return false;

        Items.Add(item);
        if (_isOpen) UpdateUI();

        if (Items.Count == Capacity) Full = true;

        return true;
    }

    public bool Remove(Item item)
    {
        if (Items.Count == 0 || !Items.Contains(item)) return false;
        if (Full) Full = false;

        Items.Remove(item);
        if (_isOpen) UpdateUI();

        return true;
    }

    void UpdateUI()
    {
        UIManager ui = FindObjectOfType<UIManager>();
        ui.RenderInventory(ref Items);
    }
}
