using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attributes))]
public class Enemy : MonoBehaviour
{
    public Attributes MyAttributes;

    private PlayerControls _pc;

    // Start is called before the first frame update
    void Start()
    {
        MyAttributes = GetComponent<Attributes>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        _pc = collision.gameObject.GetComponent<PlayerControls>();
        if (_pc) 
        {
            _pc.Disabled = true;
            UIManager ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            Sprite s = gameObject.GetComponent<SpriteRenderer>().sprite;
            Action<string> battleTurnAction   = m => Debug.Log(m);
            Action<string> battleResultAction = m => OnBattleResult(m);
            ui.StartBattle(s, battleTurnAction, battleResultAction);
        }
    }

    void OnBattleResult(string message)
    {
        Debug.Log(message);
        _pc.Disabled = false;
        if (message.Equals("victory"))
        {
            Destroy(gameObject);
        }
    }
}
