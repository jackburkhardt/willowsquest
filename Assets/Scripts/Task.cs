using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class Task : MonoBehaviour
{
    /* A task is basically some code that lets developers set quest objectives
     for the player 
     */

    [SerializeField] private string name;
    
    // the type of task this is (arbitrary?)
    [SerializeField] private List<TaskType> taskType;
    [SerializeField] public bool completed;

    private bool allEnemiesDead;
    private bool allItemsCollected;
    private bool allNPCsTalked;

    private Inventory _inventory;
    private GameTracker _tracker;
    private Interactor _interactor;

    [SerializeField] public UnityEvent postTaskAction;
    
    // gamestate that should be changed to when task completes
    [SerializeField] private GameState nextState;
    
    // Enemies that need to be defeated
    [SerializeField] private List<Battle> enemies;
    
    // Items that need to be picked up
    [SerializeField] private List<Item> items;
    
    // NPC that needs to be spoken to
    [SerializeField] private List<NPCInteractor> npcs;

    private void Awake()
    {
        _tracker = FindObjectOfType<GameTracker>();
        _inventory = FindObjectOfType<Inventory>();
        _interactor = FindObjectOfType<Interactor>();
        StartCoroutine(CheckForProgress());

        _interactor.InteractionEvent += (type, interactor) =>
        {
            //Debug.Log("speak event triggered");
            if (type == InteractionType.Speak && npcs.Contains(interactor)) npcs.Remove(interactor);
        };

        _interactor.EnemyDeathEvent += (battle) =>
        {
            if (enemies.Contains(battle)) enemies.Remove(battle);
        };

        _inventory.InventoryStoreEvent += (item) =>
        {
            if (items.Contains(item)) items.Remove(item);
        };
    }
    

    private IEnumerator CheckForProgress()
    {
        while (true)
        {

            if (npcs.Count == 0) allNPCsTalked = true;
            if (items.Count == 0) allItemsCollected = true;
            if (enemies.Count == 0) allEnemiesDead = true;

            if (allEnemiesDead && allItemsCollected && allNPCsTalked)
            {
                CompleteTask();
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void CompleteTask()
    {
        completed = true;
        postTaskAction.Invoke();
        StartCoroutine(_tracker.AdvanceObjective(nextState));
    }
}

public enum TaskType
{
    Fight,
    Collect,
    Talk
}

public delegate void InteractionEventDelegate(InteractionType type, NPCInteractor interactor);

public delegate void EnemyDeathDelegate(Battle enemy);
