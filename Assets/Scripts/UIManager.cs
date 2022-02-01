using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas UICanvas;

    public Image  BattleBackground;
    public Image  BattleSprite;
    public Text   BattleText;
    public Button BattleAttackButton;
    public Button BattleFleeButton;
    private Action<string> _battleAction;

    // Start is called before the first frame update
    void Start()
    {
        BattleBackground.enabled = false;
        BattleSprite.enabled = false;
        BattleText.enabled = false;
        ToggleButton(BattleAttackButton, false);
        ToggleButton(BattleFleeButton, false);
    }

    public void StartBattle(Sprite sprite, Action<string> action)
    {
        _battleAction = action;
        BattleBackground.enabled = true;
        BattleSprite.sprite = sprite;
        BattleSprite.enabled = true;
        BattleText.enabled = true;
        ToggleButton(BattleAttackButton, true);
        ToggleButton(BattleFleeButton, true);
    }

    public void QuitBattle(string result)
    {
        BattleBackground.enabled = false;
        BattleSprite.enabled = false;
        BattleText.enabled = false;
        ToggleButton(BattleAttackButton, false);
        ToggleButton(BattleFleeButton, false);
        _battleAction.Invoke(result);
    }

    private void ToggleButton(Button button, bool active)
    {
        button.enabled = active;
        button.image.enabled = active;
        button.GetComponentInChildren<Text>().enabled = active;
    }
}
