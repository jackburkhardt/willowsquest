using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas UICanvas;

    public Canvas InventoryCanvas;
    public GameObject InventorySlots;
    private Action[] _inventoryUseActions;

    public Canvas StatsCanvas;

    public  Canvas BattleCanvas;
    public  Image  BattleSprite;
    private Action<string> _battleTurnAction;
    private Action<string> _battleResultAction;

    public PlayerControls PlayerControls;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, true);

        PlayerControls.Disabled = false;

        EnableCanvas(UICanvas);
    }

    public void StartBattle(Sprite sprite, Action<string> turnAction, Action<string> resultAction)
    {
        PlayerControls.Disabled = true;

        BattleSprite.sprite = sprite;
        _battleTurnAction = turnAction;
        _battleResultAction = resultAction;
        EnableCanvas(BattleCanvas);
    }

    public void OnBattleTurn(string turn) {
        _battleTurnAction.Invoke(turn);
    }

    public void QuitBattle(string result)
    {
        _battleResultAction.Invoke(result);

        PlayerControls.Disabled = false;
        EnableCanvas(UICanvas);
    }

    public void OpenInventory(ref List<Item> inventoryItems)
    {
        PlayerControls.Disabled = true;

        RenderInventory(ref inventoryItems);
        InventoryCanvas.enabled = true;
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
        InventoryCanvas.enabled = false;

        PlayerControls.Disabled = false;
    }

    public void OpenStats(ref Attributes attributes)
    {
        PlayerControls.Disabled = true;

        StatsCanvas.enabled = true;
    }

    public void CloseStats()
    {
        StatsCanvas.enabled = false;

        PlayerControls.Disabled = false;
    }

    public void GameOver()
    {
        PlayerControls.Disabled = true;
    }

    public void EnableCanvas(Canvas canvas)
    {
        foreach (var c in new Canvas[4] {UICanvas, InventoryCanvas, StatsCanvas, BattleCanvas})
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
}
