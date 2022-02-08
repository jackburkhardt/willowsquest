using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite Icon;

    public virtual void Use()
    {
        Inventory i = FindObjectOfType<Player>().Inventory;
        i.Remove(this);

        // Maybe we want to make this not a MonoBehaviour?
        Destroy(gameObject);
    }
}
