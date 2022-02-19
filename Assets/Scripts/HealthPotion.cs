using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    public float Amount;

    public override void Use()
    {
        Attributes a = FindObjectOfType<Player>().Attributes;
        a.UpdateHealth(Amount);

        base.Use();
    }
}
