using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject HintObject;
    public bool IsActive;

    // Start is called before the first frame update
    void Start()
    {
        _toggleActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            HintObject.transform.position = position;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _toggleActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _toggleActive(false);
    }

    private void _toggleActive(bool active)
    {
        if (HintObject != null)
        {
            IsActive = active;
            HintObject.SetActive(active);
        }
    }
}
