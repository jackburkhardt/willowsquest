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

    public Canvas AttributesCanvas;
    public GameObject AttributesButtons;

    public  Canvas BattleCanvas;
    public  Image  BattleSprite;
    private Battle _battleInstance;
    private Action _postBattleAction;

    public Canvas GameOverCanvas;

    public PlayerControls PlayerControls;

    public Sprite HappyTag;
    public Sprite AngryTag;
    public Sprite SadTag;

    public GameObject EnemyMood;
    public GameObject WillowMood;

    
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

    public void StartBattle(Battle battle, Action postBattleAction)
    {
        BattleSprite.sprite = battle.EnemySprite;
        _battleInstance = battle;
        _postBattleAction = postBattleAction;

        battle.OnBattleStart();
        EnableBattleButtons();

        SetCanvas(BattleCanvas);
        ToggleCanvas(UICanvas, true);
        ToggleCanvas(AttributesCanvas, true);

        // Randomly decide who will act first in the battle
        if (Random.Range(0f,1f) < 0.5) {
            battle.OnBattleTurn("nop");
        }
    }

    public void OnBattleTurn(string turn) 
    {
        DisableBattleButtons();
        StartCoroutine(_battleInstance.OnBattleTurn(turn, EnableBattleButtons));

    }

    void DisableBattleButtons()
    {
        Button[] buttons = BattleCanvas.GetComponentsInChildren<Button>(true);
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    void EnableBattleButtons(Dictionary<string, int> cooldowns = null)
    {
        Button[] buttons = BattleCanvas.GetComponentsInChildren<Button>(true);
        //Text buttonText;
        if (cooldowns == null)
        {
            foreach (var button in buttons)
            {
                //buttonText = button.GetComponentInChildren<Text>();
                //buttonText.text = button.name;
                button.interactable = true;
            }
        }
        else
        {
            int cooldown = 0;
            foreach (var button in buttons)
            {
                //buttonText = button.GetComponentInChildren<Text>();
                if (cooldowns.TryGetValue(button.name.ToLower(), out cooldown))
                {
                    //buttonText.text = String.Format("{0} ({1})", button.name, cooldown);
                }
                else
                {
                    //buttonText.text = button.name;
                    button.interactable = true;
                }
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
        t.text = String.Format("ATK\t\t{0:N0}\nDEF\t\t{1:N0}\nSPD\t\t{2:N0}\nMD\t\t{3}\nEXP\t\t{4:N0}",
            a.ATK, a.DEF, a.SPD, a.MD.ToString(), a.EXP);
        
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

    public void UpdateMoodTag(string newmood, string party)
    {
        GameObject currTag = WillowMood;
        if (party == "enemy")
        {
            currTag = EnemyMood;
        }
        else
        {
            currTag = WillowMood;
        }

        if (newmood == "happy")
        {
            currTag.GetComponent<Image>().sprite = HappyTag;
        }
        else if (newmood == "angry")
        {
            currTag.GetComponent<Image>().sprite = AngryTag;
        }
        else
        {
            currTag.GetComponent<Image>().sprite = SadTag;
        }

    }
}
