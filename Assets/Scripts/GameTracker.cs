using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameTracker : MonoBehaviour
{
    public static GameState currentGameState = 0;
    [SerializeField] private Tilemap wallMap;
    private Dictionary<GameState, string> objectiveDescriptions; // ⍟
    [SerializeField] private Text objectiveText;
    [SerializeField] private Text objectiveLabelText;
    public static List<GameObject> enemiesKilledSinceLastCheckpoint = new List<GameObject>();
    public static List<Item> itemsUsedSinceLastCheckpoint = new List<Item>();

    // Start is called before the first frame update
    void Awake()
    {
        objectiveDescriptions = new Dictionary<GameState, string>()
        {
            {GameState.Start, "Talk to the statues around here to figure out where you are."},
            {GameState.FightSquirrels, "Fight off the squirrels. One of them probably has the key."},
            {GameState.Stump, "Grab the key and bring it to the gate guard."},
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
            battle.InitializeEnemy();
        }

        Inventory inventory = FindObjectOfType<Inventory>();
        foreach (var item in itemsUsedSinceLastCheckpoint)
        {
            inventory.Items.Add(item);
        }
    }

    public IEnumerator AdvanceObjective(GameState state)
    {
        // should prevent players from accidentally undoing progress if they
        // complete tasks in the wrong order
        if (state <= currentGameState) yield break;
        
        currentGameState = state;

        objectiveLabelText.text = "Objective Complete!";
        objectiveText.color = Color.green;
        objectiveLabelText.color = Color.green;
        
        yield return new WaitForSeconds(5);

        objectiveLabelText.text = "Current Objective:";
        objectiveDescriptions.TryGetValue(currentGameState, out var s);
        objectiveText.text = s;
        objectiveLabelText.color = Color.white;
        objectiveText.color = Color.white;

    }

    public void UnlockGate(int gate)
    {
        switch (gate)
        {
            case 1:
                wallMap.SetTiles(new []
                {
                    new Vector3Int(4, -22, 0),
                    new Vector3Int(4, -23, 0),
                    new Vector3Int(4, -24, 0)
                }, new TileBase[] { null, null, null } );
                Debug.Log("gate 1 unlocked");
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                return;
        }
    }
    
}
