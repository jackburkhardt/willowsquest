using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas StartCanvas;

    public Canvas UICanvas;

    public Canvas InventoryCanvas;
    public GameObject InventorySlots;
    private Action[] _inventoryUseActions;

    public Canvas StatsCanvas;

    public  Canvas BattleCanvas;
    public  Image  BattleSprite;
    private Action<string> _battleTurnAction;
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
        _battleTurnAction = enemy.OnBattleTurn;
        _postBattleAction = postBattleAction;

        SetCanvas(BattleCanvas);
        ToggleCanvas(UICanvas, true);
    }

    public void OnBattleTurn(string turn) {
        _battleTurnAction.Invoke(turn);
    }

    public void QuitBattle()
    {
        _postBattleAction.Invoke();

        SetCanvas(UICanvas);
    }

    public void OpenInventory(ref List<Item> inventoryItems)
    {
        PlayerControls.Disabled = true;

        RenderInventory(ref inventoryItems);
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

        PlayerControls.Disabled = false;
    }

    public void OpenStats(ref Attributes attributes)
    {
        PlayerControls.Disabled = true;

        RenderStats(ref attributes);
        ToggleCanvas(StatsCanvas, true);
    }

    void RenderStats(ref Attributes a)
    {
        Text text = StatsCanvas.GetComponentInChildren<Text>(true);
        text.text = String.Format("HP\t\t{0:N0}\nATK\t\t{1:N0}\nDEF\t\t{2:N0}\nSPD\t\t{3:N0}\nMD\t\t{4}",
            a.HP, a.ATK, a.DEF, a.SPD, a.MD.ToString());
    }

    public void CloseStats()
    {
        ToggleCanvas(StatsCanvas, false);

        PlayerControls.Disabled = false;
    }

    void SetCanvas(Canvas canvas)
    {
        foreach (var c in new Canvas[6] {StartCanvas, UICanvas, InventoryCanvas, StatsCanvas, BattleCanvas, GameOverCanvas})
        {
            if (c.name.Equals(canvas.name))
            {
                c.enabled = true;
            }
            else
            {
                c.enabled = false;
            }
        }
    }

    void ToggleCanvas(Canvas canvas, bool enable)
    {
        canvas.enabled = enable;
    }
}
