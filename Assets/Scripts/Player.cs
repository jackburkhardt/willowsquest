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
    public Dictionary<string, int> Cooldowns;
    public Image HealthBar;
    
    [SerializeField] private float _lerpSpeed;

    void Update()
    {
        if (Attributes.HP < 1)
        {
            UIManager ui = FindObjectOfType<UIManager>();
            ui.GameOver();
        }
        UpdateHealth(Attributes.HP);
    }

    private void UpdateHealth(float health)
    {
        HealthBar.fillAmount = Mathf.Lerp(
            HealthBar.fillAmount,
            Mathf.Clamp(health/100f, 0, 1f),
            _lerpSpeed);
    }
}
