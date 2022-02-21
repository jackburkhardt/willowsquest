using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component for interactions to be used on the player character.
/// </summary>
public class Interactor : MonoBehaviour
{
    // is player currently in an interaction?
    private bool _isInteracting;
    
    // the delay between characters in the dialogue text scrolling (smaller = faster)
    // 0.01 fast, 0.05 medium, 0.1 slow
    [SerializeField] private float _textScrollDelay;
    
    // Canvas text for dialogue
    [SerializeField] private Text _displayText;
    
    // image to signal that player can move to next dialogue line
    [SerializeField] private Image _nextPageImage;
    [SerializeField] private Image _dialogueBackgroundImage;
    private PlayerControls _playerControls;

    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameTracker _tracker;

    // Start is called before the first frame update
    void Start()
    {
        _playerControls = FindObjectOfType<PlayerControls>();
    }

    public void StartInteraction(GameObject go, InteractionType type, List<string> dialogue = null, Enemy enemy = null, Item item = null)
    {
        if (_isInteracting) return;
        _isInteracting = true;
        _playerControls.Disabled = true;
        switch (type)
        {
            case InteractionType.Speak:
                StartCoroutine(DisplayDialogue(dialogue, EndInteraction));
                break;
            case InteractionType.Rest:
                // do resting func (go.startrest(), etc)
            case InteractionType.Battle:
                StartCoroutine(DisplayDialogue(dialogue, () => StartBattle(enemy)));
                break;
            case InteractionType.PickUp:
                StartCoroutine(DisplayDialogue(new List<string>
                {
                    "You picked up a " + item.Name + "!",
                    "The description reads: \"" + item.Description + "\"",
                    "You put the " + item.Name + " in your inventory [I]."
                }, () => PickUp(item)));
                break;
            case InteractionType.Give:
                StartCoroutine(DisplayDialogue(new List<string>
                {
                    "The " + item.Name + " was removed from your inventory."
                }, () => GiveItem(item)));
                break;
        }
    }

    public IEnumerator DisplayDialogue(List<string> dialogue, Action postDialogueAction = null)
    {
        // this coroutine chops each line up into a char array and then
        // displays each char in order to simulate scroll effect
        _dialogueBackgroundImage.enabled = true;
        _displayText.text = "";
        _displayText.enabled = true;
        foreach (var line in dialogue)
        {
            var characters = line.ToCharArray();
            foreach (var c in characters)
            {
                _displayText.text += c;
                yield return new WaitForSeconds(_textScrollDelay);
            }
            while (!Input.GetKeyDown(KeyCode.E))
            {
                _nextPageImage.enabled = true;
                yield return null;
            }
            _nextPageImage.enabled = false;
            _displayText.text = "";
        }
        _dialogueBackgroundImage.enabled = false;
        _displayText.enabled = false;
        postDialogueAction?.Invoke();
    }

    public void PickUp(Item item)
    {
        _inventory.Store(item);
        EndInteraction();
    }

    public void GiveItem(Item item)
    {
        _inventory.Remove(item);
        EndInteraction();
    }

    public void StartBattle(Enemy enemy) 
    {
        UIManager ui = FindObjectOfType<UIManager>();
        ui.StartBattle(enemy, EndInteraction);
    }

    public void EndInteraction()
    {
        _isInteracting = false;
        _playerControls.Disabled = false;
    }
    
    public bool IsInteracting => _isInteracting;
}
