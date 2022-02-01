using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerControls _pc;

    void OnCollisionEnter2D(Collision2D collision)
    {
        _pc = collision.gameObject.GetComponent<PlayerControls>();
        if (_pc) 
        {
            _pc.Disabled = true;
            UIManager ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            Action<string> battleAction = m => OnBattleResult(m);
            ui.StartBattle(sr.sprite, battleAction);
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