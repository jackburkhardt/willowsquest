using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Health : MonoBehaviour
{
    private Image _hb;

    // Start is called before the first frame update
    void Start()
    {
        _hb = GetComponent<Image>();
    }

    public void UpdateHealth(float health)
    {
        _hb.fillAmount = Mathf.Clamp(health, 0, 1f);
    }
}
