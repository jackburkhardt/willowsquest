using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attributes))]
[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour
{
    public Attributes Attributes;
    public Inventory  Inventory;
}
