using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas UICanvas;

    public  Canvas BattleCanvas;
    public  Image  BattleSprite;
    private Action<string> _battleTurnAction;
    private Action<string> _battleResultAction;

    // Start is called before the first frame update
    void Start()
    {
        UICanvas.enabled = true;
        BattleCanvas.enabled = false;
    }

    public void StartBattle(Sprite sprite, Action<string> turnAction, Action<string> resultAction)
    {
        UICanvas.enabled = false;
        BattleSprite.sprite = sprite;
        _battleTurnAction = turnAction;
        _battleResultAction = resultAction;
        BattleCanvas.enabled = true;
    }

    public void OnBattleTurn(string turn) {
        _battleTurnAction.Invoke(turn);
    }

    public void QuitBattle(string result)
    {
        BattleCanvas.enabled = false;
        _battleResultAction.Invoke(result);
        UICanvas.enabled = true;
    }
}
