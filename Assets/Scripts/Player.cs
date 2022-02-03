using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attributes))]
[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour
{
    public Attributes MyAttributes;
    public Inventory  MyInventory;

    public float EXP = 0f;

    // Start is called before the first frame update
    void Start()
    {
        MyAttributes = GetComponent<Attributes>();
        MyInventory  = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
