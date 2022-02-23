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

    // To display attributes
    public Image HealthBar;
    [SerializeField] private float _lerpSpeed = 0.01f;
    public Text RenderText;

    // Player-only fields and logic
    public float EXP = 0f;

    // What type/are we rendering our health
    public bool PlayerAttributes = false;
    public bool ActiveEnemy = false;

    void Start()
    {
        UpdateText();
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealth(float value)
    {
        HP = Mathf.Clamp(HP+value, 0, 100f);
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
        UpdateText();
    }

    public void UpdateText()
    {
        if (RenderText)
        {
            UIManager ui = FindObjectOfType<UIManager>();
            ui.RenderAttributes(this, RenderText);
        }
    }

    void UpdateHealthBar()
    {
        if (HealthBar && (PlayerAttributes || (!PlayerAttributes && ActiveEnemy)))
        {
            HealthBar.fillAmount = Mathf.Lerp(
                HealthBar.fillAmount,
                Mathf.Clamp(HP/100f, 0, 1f),
                _lerpSpeed);
        }
    }
}
