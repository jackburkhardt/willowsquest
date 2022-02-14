using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public static List<GameObject> enemiesKilledSinceLastCheckpoint = new List<GameObject>();
    public static List<Item> itemsUsedSinceLastCheckpoint = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LoadLastGameState()
    {
        foreach (var enemyObject in enemiesKilledSinceLastCheckpoint)
        {
            Instantiate(enemyObject);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.Attributes.HP = 100;
        }

        Inventory inventory = FindObjectOfType<Inventory>();
        foreach (var item in itemsUsedSinceLastCheckpoint)
        {
            inventory.Items.Add(item);
        }
    }
}
