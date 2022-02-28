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
            Debug.Log("speak event triggered");
            if (type == InteractionType.Speak) RegisterSpeakEvent(interactor);
        };

    }


    private void RegisterSpeakEvent(NPCInteractor callerNPC)
    {
        if (npcs.Contains(callerNPC)) npcs.Remove(callerNPC);
    }


    private IEnumerator CheckForProgress()
    {
        while (true)
        {
            foreach (var enemy in enemies.Where(enemy => enemy.EnemyAttributes.HP <= 0))
            {
                enemies.Remove(enemy);
            }

            foreach (var item in items.Where(item => _inventory.Items.Contains(item)))
            {
                items.Remove(item);
            }

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
        _tracker.AdvanceObjective();
    }
}

public enum TaskType
{
    Fight,
    Collect,
    Talk
}

public delegate void InteractionEventDelegate(InteractionType type, NPCInteractor interactor);
