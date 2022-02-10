using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sections that warp players from one area of the map to another
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class WarpZone : MonoBehaviour
{
    // is this teleporter unlocked?
    public bool unlocked;

    private Transform player;
    
    // Signifies if the player just teleported, and if true prevents them from being teleported again
    // once sent to the other zone (basically, prevents players from being stuck in teleporting loop)
    public static bool preventTeleportAgain;
    
    // the teleporter that the player should be sent to when they enter this one
    [SerializeField] private WarpZone connectedZone;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("collide!");
        if (unlocked && !preventTeleportAgain)
        {
            preventTeleportAgain = true;
            Warp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        preventTeleportAgain = false;
    }

    private void Warp()
    {
        player.position = connectedZone.transform.position;

        //TODO: play sound?
    }
}
