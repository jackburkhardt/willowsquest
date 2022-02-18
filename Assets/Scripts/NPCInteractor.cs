using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    // (Optional) Icon to appear above their head when in range to signal intractability.
    private SpriteRenderer _iconRenderer;
    [SerializeField] private Sprite _iconSprite;
    [SerializeField] private float  _iconOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInteractor = FindObjectOfType<Interactor>();

        _iconRenderer = ((GameObject)Instantiate(Resources.Load("Icons/IconPrefab"), new Vector3(transform.position.x, transform.position.y + _iconOffset), Quaternion.identity, transform)).GetComponent<SpriteRenderer>();
        if (_iconSprite != null) _iconRenderer.sprite = _iconSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inRange && !_playerInteractor.IsInteracting)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _playerInteractor.StartInteraction(gameObject, _interactionType, _dialogue, _enemy);
            }
            else if (_interactionType is InteractionType.Battle) 
            {
                _playerInteractor.StartInteraction(gameObject, _interactionType, _dialogue, _enemy);
                _inRange = false;
            }
        } 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _inRange = true;
        if (_iconSprite != null) _iconRenderer.enabled = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _inRange = false;
        if (_iconSprite != null) _iconRenderer.enabled = false;
    }
}
