using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemHint : Hint
{
    [SerializeField] private int _index;

    override public void ToggleActive(bool active)
    {
        if (HintObject != null)
        {
            if (active) _updateItemHint();
            IsActive = active;
            HintObject.SetActive(active);
        }
    }

    private void _updateItemHint()
    {
        var items = FindObjectOfType<Player>().Inventory.Items;
        if (_index < items.Count)
        {
            Item item = items[_index];
            HintObject.GetComponentInChildren<Text>().text = String.Format("{0}\n{1}", item.Name, item.Description);
        }
    }
}
