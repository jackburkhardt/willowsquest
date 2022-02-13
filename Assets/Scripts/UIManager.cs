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

    public Canvas InventoryCanvas;
    public GameObject InventorySlots;
    private Action[] _inventoryUseActions;

    public Canvas StatsCanvas;
    public GameObject StatsButtons;

    public  Canvas BattleCanvas;
    public  Image  BattleSprite;
    private Enemy  _battleEnemy;
    private Action _postBattleAction;

    public Canvas GameOverCanvas;

    public PlayerControls PlayerControls;


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
        ToggleCanvas(StatsCanvas, true);

        // Randomly decide who will act first in the battle
        if (Random.Range(0f,1f) < 0.5) {
            OnBattleTurn("nop");
        }
    }

    public void OnBattleTurn(string turn) {
        StartCoroutine(_battleEnemy.OnBattleTurn(turn));
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
        ToggleCanvas(StatsCanvas, true);
    }

    public void RenderAttributes(Attributes a, Text t)
    {
        t.text = String.Format("HP\t\t{0:N0}\nATK\t\t{1:N0}\nDEF\t\t{2:N0}\nSPD\t\t{3:N0}\nMD\t\t{4}\nEXP\t\t{5:N0}",
            a.HP, a.ATK, a.DEF, a.SPD, a.MD.ToString(), a.EXP);
        
        if (a.PlayerAttributes)
        {
            Image[] buttons = StatsButtons.GetComponentsInChildren<Image>(true);
            bool enabled  = a.EXP >= 1f;
            foreach (var button in buttons)
            {
                button.enabled = enabled;
            }
        }
    }

    public void CloseAttributes()
    {
        ToggleCanvas(StatsCanvas, false);
    }

    void SetCanvas(Canvas canvas)
    {
        foreach (var c in new Canvas[6] {StartCanvas, UICanvas, InventoryCanvas, StatsCanvas, BattleCanvas, GameOverCanvas})
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
