using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public Canvas StartCanvas;

    public Canvas UICanvas;
    public Dictionary<GameState, string> objectiveDescriptions;

    public Canvas InventoryCanvas;
    public GameObject InventorySlots;
    private Action[] _inventoryUseActions;

    public Canvas AttributesCanvas;
    public GameObject AttributesButtons;

    public  Canvas BattleCanvas;
    public  Image  BattleSprite;
    private Enemy  _battleEnemy;
    private Action _postBattleAction;

    public Canvas GameOverCanvas;

    public PlayerControls PlayerControls;


    private void Awake()
    {
        objectiveDescriptions = new Dictionary<GameState, string>()
        {
            {GameState.Start, "Talk to the people around here to figure out where you are."},
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
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, true);

        PlayerControls.Disabled = true;
        SetCanvas(StartCanvas);
    }

    public void StartGame()
    {
        PlayerControls.Disabled = false;
        SetCanvas(UICanvas);
    }

    public void GameOver()
    {
        PlayerControls.Disabled = true;
        SetCanvas(GameOverCanvas);
    }

    public void StartBattle(Enemy enemy, Action postBattleAction)
    {
        BattleSprite.sprite = enemy.Sprite;
        _battleEnemy = enemy;
        _postBattleAction = postBattleAction;

        SetCanvas(BattleCanvas);
        ToggleCanvas(UICanvas, true);
        ToggleCanvas(AttributesCanvas, true);

        // Randomly decide who will act first in the battle
        if (Random.Range(0f,1f) < 0.5) {
            OnBattleTurn("nop");
        }
    }

    public void OnBattleTurn(string turn) 
    {
        DisableBattleButtons();
        StartCoroutine(_battleEnemy.OnBattleTurn(turn, EnableBattleButtons));
    }

    void DisableBattleButtons()
    {
        Button[] buttons = BattleCanvas.GetComponentsInChildren<Button>(true);
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    void EnableBattleButtons(List<string> cooldowns)
    {
        Button[] buttons = BattleCanvas.GetComponentsInChildren<Button>(true);
        foreach (var button in buttons)
        {
            if (!cooldowns.Contains(button.name.ToLower())) 
            {
                button.interactable = true;
            }
        }
    }

    public void QuitBattle()
    {
        _postBattleAction.Invoke();

        SetCanvas(UICanvas);
    }

    public void OpenInventory()
    {
        ToggleCanvas(InventoryCanvas, true);
    }
    
    public void RenderInventory(ref List<Item> inventoryItems)
    {
        _inventoryUseActions = new Action[inventoryItems.Count];

        Image[] slots = InventorySlots.GetComponentsInChildren<Image>(true);
        int count = 0;
        foreach (var item in inventoryItems)
        {
            slots[count].sprite = item.Icon;
            slots[count].enabled = true;
            _inventoryUseActions[count] = item.Use;
            count++;
        }
        while (count < slots.Length)
        {
            slots[count].enabled = false;
            count++;
        }
    }

    public void OnInventoryUse(int slot)
    {
        _inventoryUseActions[slot].Invoke();
    }

    public void CloseInventory()
    {
        ToggleCanvas(InventoryCanvas, false);
    }

    public void OpenAttributes()
    {
        ToggleCanvas(AttributesCanvas, true);
    }

    public void RenderAttributes(Attributes a, Text t)
    {
        t.text = String.Format("HP\t\t{0:N0}\nATK\t\t{1:N0}\nDEF\t\t{2:N0}\nSPD\t\t{3:N0}\nMD\t\t{4}\nEXP\t\t{5:N0}",
            a.HP, a.ATK, a.DEF, a.SPD, a.MD.ToString(), a.EXP);
        
        if (a.PlayerAttributes)
        {
            Image[] buttons = AttributesButtons.GetComponentsInChildren<Image>(true);
            bool enabled  = a.EXP >= 1f;
            foreach (var button in buttons)
            {
                button.enabled = enabled;
            }
        }
    }

    public void CloseAttributes()
    {
        ToggleCanvas(AttributesCanvas, false);
    }

    void SetCanvas(Canvas canvas)
    {
        foreach (var c in new Canvas[6] {StartCanvas, UICanvas, InventoryCanvas, AttributesCanvas, BattleCanvas, GameOverCanvas})
        {
            if (c.name.Equals(canvas.name)) c.enabled = true;
            else c.enabled = false;
        }
    }

    void ToggleCanvas(Canvas canvas, bool enable)
    {
        canvas.enabled = enable;
    }
}
