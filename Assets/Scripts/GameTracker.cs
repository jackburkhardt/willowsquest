using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] private SpriteRenderer gate2Door;
    [SerializeField] private SpriteRenderer gate3Door;
    [SerializeField] private Sprite openDoor;
    [SerializeField] private GameObject crown;

    // Start is called before the first frame update
    void Awake()
    {
        objectiveDescriptions = new Dictionary<GameState, string>()
        {
            {GameState.Start, "Talk to the statues around here to figure out where you are."},
            {GameState.CheckFirstGate, "Begin following the path to the gate."},
            {GameState.FightSquirrels, "Fight off the squirrels. One of them may know where the key is."},
            {GameState.Stump, "Grab the key and bring it to the gate guard."},
            {GameState.MeetCobra, "Talk to the Cobra King."},
            {GameState.FightWolves, "Go \"talk\" to the wolves."},
            {GameState.GiveCrown, "Return the crown to the Cobra King."},
            {GameState.CheckSecondGate, "Keep following the path to the next gate!"},
            //{GameState.FightFoxes, "AMBUSH! Fight the sneaky foxes off!"},
            {GameState.MeetFrog, "The gatekeeper is asleep. Maybe find someone else to ask."},
            {GameState.FindWorms, "Find the worms. They may be hidden under some debris somewhere."},
            {GameState.GiveWorms, "Give the (very slimy) worms to Dr. Frog"},
            {GameState.FightBear, "Keep going. The gelatin has to be close!"},
            {GameState.Finish, "Reunite with your gelatin friend."}
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

    public void RestartGame()
    {
        // TODO: may not be our intended behavior
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
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
                gate2Door.sprite = openDoor;
                gate2Door.gameObject.GetComponent<Collider2D>().enabled = false;
                break;
            case 3:
                gate3Door.sprite = openDoor;
                gate3Door.gameObject.GetComponent<Collider2D>().enabled = false;
                break;
            default:
                return;
        }
    }

    public void SpawnCrown()
    {
        crown.transform.position = FindObjectOfType<Player>().transform.position;
        crown.SetActive(true);
    }

}
