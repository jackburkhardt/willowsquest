using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerControls pc = collision.gameObject.GetComponent<PlayerControls>();
        if (pc) 
        {
            pc.Disabled = true;
            GameObject ui = GameObject.Find("UIManager");
            ui.GetComponent<UIManager>().StartBattle();
        }
    }
}
