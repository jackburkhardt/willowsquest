using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Make sure collider acts as trigger
[RequireComponent(typeof(Collider2D))]
public class NPCInteractor : MonoBehaviour
{

    // Interactor for the player, assigned automatically on scene start.
    private Interactor _playerInteractor;
    
    // The type of interaction this NPC provides
    [SerializeField] private InteractionType _interactionType;
    
    // (Optional) list of dialogue lines
    [SerializeField] private List<string> _dialogue;

    // (Optional) enemy information if this is an enemy
    [SerializeField] private Battle _battle;
    
    // (Optional) item information if this NPC needs to hold/recieve an item
    [SerializeField] private Item _item;

    // Is player in range of this npc?
    private bool _inRange;
    
    // (Optional) Icon to appear above their head when in range to signal intractability.
    private SpriteRenderer _iconRenderer;
    private Transform _iconHolder;
    [SerializeField] private Sprite _iconSprite;
    [SerializeField] private float  _iconOffset;

    [Header("Tasks (Optional)")] 
    [SerializeField] private Task task;
    //[SerializeField] private List<string> _taskIncompleteDialogue;
    [SerializeField] private List<string> _taskCompleteDialogue;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInteractor = FindObjectOfType<Interactor>();
        _battle = GetComponent<Battle>();
        _iconHolder = GameObject.Find("Icon Holder").transform;

        _iconRenderer = ((GameObject)Instantiate(Resources.Load("Icons/IconPrefab"), new Vector3(transform.position.x, transform.position.y + _iconOffset), Quaternion.identity, _iconHolder)).GetComponent<SpriteRenderer>();
        if (_iconSprite != null) _iconRenderer.sprite = _iconSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_inRange || _playerInteractor.IsInteracting) return;
        
        if (Input.GetKeyDown(KeyCode.E) && task != null && (task.completed || task.AboutToComplete(this)))
        {
            _playerInteractor.StartInteraction(this, _interactionType,
                _taskCompleteDialogue, _battle, _item);
        }
        else if (_interactionType is InteractionType.Battle) 
        {
            _playerInteractor.StartInteraction(this, _interactionType, _dialogue, _battle, _item);
            _inRange = false;
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            _playerInteractor.StartInteraction(this, _interactionType, _dialogue, _battle, _item);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        
        _inRange = true;
        if (_iconSprite != null) _iconRenderer.enabled = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        
        _inRange = false;
        if (_iconSprite != null) _iconRenderer.enabled = false;
    }
}
