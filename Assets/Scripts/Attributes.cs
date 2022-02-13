using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attributes : MonoBehaviour
{
    public float HP  = 100f;
    public float ATK = 1f;
    public float DEF = 1f;
    public float SPD = 1f;
    public Mood  MD  = Mood.Happy;

    // Player-only fields and logic
    public float EXP = 0f;

    public bool PlayerAttributes = false;
    public Text RenderText;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateHealth(float value)
    {
        HP = Mathf.Clamp(HP+value, 0, 100f);
        UpdateUI();
    }

    public void AddAttribute(string attribute)
    {
        if (!PlayerAttributes || EXP < 1f) return;

        switch (attribute.ToUpper())
        {
            case "ATK":
                ATK += 1f;
                break;
            case "DEF":
                DEF += 1f;
                break;
            case "SPD":
                SPD += 1f;
                break;
            default:
                Debug.Log("Invalid attribute name.");
                return;
        }

        EXP -= 1f;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (RenderText)
        {
            UIManager ui = FindObjectOfType<UIManager>();
            ui.RenderAttributes(this, RenderText);
        }
    }
}
