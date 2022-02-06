using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Attributes))]
[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour
{
    public Attributes Attributes;
    public Inventory  Inventory;

    public Image HealthBar;

    public float EXP = 0f;

    private bool _statsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        Attributes = GetComponent<Attributes>();
        Inventory  = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UIManager ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            if (_statsOpen)
            {
                ui.CloseStats();
                _statsOpen = false;
            }
            else
            {
                ui.OpenStats(ref Attributes);
                _statsOpen = true;
            }
        }

        if (Attributes.HP < 0)
        {
            UIManager ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            ui.GameOver();
        }
        UpdateHealth(Attributes.HP);
    }

    private void UpdateHealth(float health)
    {
        HealthBar.fillAmount = Mathf.Clamp(health/100f, 0, 1f);
    }
}
