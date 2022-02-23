using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameTracker : MonoBehaviour
{
    public static GameState currentGameState = 0;
    private Dictionary<GameState, string> objectiveDescriptions; // ⍟
    [SerializeField] private Text objectiveText;
    public static List<GameObject> enemiesKilledSinceLastCheckpoint = new List<GameObject>();
    public static List<Item> itemsUsedSinceLastCheckpoint = new List<Item>();

    // Start is called before the first frame update
    void Awake()
    {
        objectiveDescriptions = new Dictionary<GameState, string>()
        {
            {GameState.Start, "Talk to the statues around here to figure out where you are."},
            {GameState.FightSquirrels, "Fight off the squirrels. One of them probably has the key."},
            {GameState.Stump, "Grab the key from the stump and open the gate."},
            {GameState.MeetCobra, "Talk to the Cobra King."},
            {GameState.FightSkunks, "Go \"talk\" to the skunks."},
            {GameState.GiveCrown, "Return the crown to the Cobra King."},
            {GameState.FightFoxes, "AMBUSH! Fight the sneaky foxes off!"},
            {GameState.MeetFrog, "Talk to the esteemed frog blocking the path."},
            {GameState.FindWorms, "Find the worms. They should be in a small pot somewhere."},
            {GameState.GiveWorms, "Give the (very slimy) worms to Dr. Frog"},
            {GameState.FightBear, "OH NO! A BEAR!"},
            {GameState.Finish, "Reunite with your gelatin friend!"}
        };

        objectiveText.text = objectiveDescriptions[0];

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
            Battle battle = enemyObject.GetComponent<Battle>();
            battle.EnemyAttributes.HP = 100;
        }

        Inventory inventory = FindObjectOfType<Inventory>();
        foreach (var item in itemsUsedSinceLastCheckpoint)
        {
            inventory.Items.Add(item);
        }
    }

    private void AdvanceObjective()
    {
        currentGameState++;
        objectiveDescriptions.TryGetValue(currentGameState, out var s);
        objectiveText.text = s;

    }
}
