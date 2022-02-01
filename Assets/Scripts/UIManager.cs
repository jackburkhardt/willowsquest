using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas UICanvas;
    public Image BattleBackground;

    // Start is called before the first frame update
    void Start()
    {
        BattleBackground.enabled = false;
    }

    public void StartBattle()
    {
        BattleBackground.enabled = true;
    }

    public void QuitBattle()
    {
        BattleBackground.enabled = false;
    }
}
