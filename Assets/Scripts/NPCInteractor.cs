using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make sure collider acts as trigger
[RequireComponent(typeof(CircleCollider2D))]
public class NPCInteractor : MonoBehaviour
{

    // Interactor for the player, assigned automatically on scene start.
    private Interactor playerInteractor;
    
    // The type of interaction this NPC provides
    [SerializeField] private InteractionType interactionType;
    
    // (Optional) list of dialogue lines
    [SerializeField] private List<string> dialogue;
    
    // Is player in range of this npc?
    private bool inRange;
    
    // Icon to appear above their head (if we want) when in range to signal intractability.
    private SpriteRenderer icon;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInteractor = FindObjectOfType<Interactor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && !playerInteractor.IsInteracting && Input.GetKeyDown(KeyCode.E))
        {
            playerInteractor.StartInteraction(gameObject, interactionType, dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        inRange = true;
        //icon.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inRange = false;
       // icon.enabled = false;
    }
}
