using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attributes))]
public class Enemy : MonoBehaviour
{
    public Sprite Sprite;
    public Attributes Attributes;

    private Attributes _playerAttributes;

    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>().sprite;
        Attributes = GetComponent<Attributes>();

        _playerAttributes = FindObjectOfType<Player>().Attributes;
    }

    public void OnBattleTurn(string message)
    {
        // An example of applying damage to the player
        _playerAttributes.HP -= 33;

        // In our sample scene, we use the command "attack,100" to instantly defeat the enemy
        string[] command = message.Split(',');
        for (int index = 0; index < command.Length; index++)
        {
            if (command[index].Equals("attack"))
            {
                int damage = int.Parse(command[++index]);
                Attributes.HP -= damage;
            }
        }

        // If the enemy runs out of health, trigger destory and call back to UIManager to quit the battle
        if (Attributes.HP <= 0)
        {
            UIManager ui = FindObjectOfType<UIManager>();
            ui.QuitBattle();

            Destroy(gameObject);
        }
    }
}
