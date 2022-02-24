using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public bool Consumable;

    public virtual void Use()
    {
        if (!Consumable) return;
        Inventory i = FindObjectOfType<Player>().Inventory;
        GameTracker.itemsUsedSinceLastCheckpoint.Add(this);
        i.Remove(this);

        // Maybe we want to make this not a MonoBehaviour?
        Destroy(gameObject);
    }
}
