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
    private bool isInteracting;
    
    // the delay between characters in the dialogue text scrolling (smaller = faster)
    [SerializeField] private float textScrollDelay;
    
    // Canvas text for dialogue
    [SerializeField] private Text displayText;
    
    // image to signal that player can move to next dialogue line
    [SerializeField] private Image nextPageImage;
    [SerializeField] private Image dialogueBackgroundImage;
    private PlayerControls PlayerControls;

    // Start is called before the first frame update
    void Start()
    {
        PlayerControls = FindObjectOfType<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartInteraction(GameObject go, InteractionType type, List<string> dialogue = null)
    {
        isInteracting = true;
        PlayerControls.Disabled = true;
        switch (type)
        {
            case InteractionType.Speak:
                StartCoroutine(DisplayDialogue(dialogue));
                break;
            case InteractionType.Rest:
                // do resting func (go.startrest(), etc)
            case InteractionType.Battle:
                // start battle
            case InteractionType.Shop:
                // open shop
                break;
        }
    }

    private IEnumerator DisplayDialogue(List<string> dialogue)
    {
        // this coroutine chops each line up into a char array and then
        // displays each char in order to simulate scroll effect
        dialogueBackgroundImage.enabled = true;
        displayText.text = "";
        displayText.enabled = true;
        foreach (var line in dialogue)
        {
            var characters = line.ToCharArray();
            foreach (var c in characters)
            {
                displayText.text += c;
                yield return new WaitForSeconds(textScrollDelay);
            }
            while (!Input.GetKeyDown(KeyCode.E))
            {
                nextPageImage.enabled = true;
                yield return null;
            }
            nextPageImage.enabled = false;
            displayText.text = "";
        }
        dialogueBackgroundImage.enabled = false;
        displayText.enabled = false;
        EndInteraction(InteractionType.Speak);
    }

    public void EndInteraction(InteractionType type)
    {
        isInteracting = false;
        PlayerControls.Disabled = false;
    }
    
    public bool IsInteracting => isInteracting;
    
    
}
