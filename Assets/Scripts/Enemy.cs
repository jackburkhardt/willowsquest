using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attributes))]
public class Enemy : MonoBehaviour
{
    public Attributes Attributes;

    // Start is called before the first frame update
    void Start()
    {
        Attributes = GetComponent<Attributes>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            UIManager ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            Sprite s = gameObject.GetComponent<SpriteRenderer>().sprite;
            ui.StartBattle(s, Debug.Log, OnBattleResult);
        }
    }

    void OnBattleResult(string message)
    {
        Debug.Log(message);
        if (message.Equals("victory"))
        {
            Destroy(gameObject);
        }
    }
}
