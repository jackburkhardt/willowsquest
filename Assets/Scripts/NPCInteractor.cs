using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make sure collider acts as trigger
[RequireComponent(typeof(CircleCollider2D))]
public class NPCInteractor : MonoBehaviour
{

    // Interactor for the player, assigned automatically on scene start.
    private Interactor _playerInteractor;
    
    // The type of interaction this NPC provides
    [SerializeField] private InteractionType _interactionType;
    
    // (Optional) list of dialogue lines
    [SerializeField] private List<string> _dialogue;

    // (Optional) enemy information if this is an enemy
    [SerializeField] private Enemy _enemy;
    
    // Is player in range of this npc?
    private bool _inRange;
    
    // Icon to appear above their head (if we want) when in range to signal intractability.
    private SpriteRenderer _icon;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInteractor = FindObjectOfType<Interactor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inRange && !_playerInteractor.IsInteracting && 
            (_interactionType is InteractionType.Battle || Input.GetKeyDown(KeyCode.E)))
        {
            _playerInteractor.StartInteraction(gameObject, _interactionType, _dialogue, _enemy);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _inRange = true;
        // icon.enabled = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _inRange = false;
        // icon.enabled = false;
    }
}
