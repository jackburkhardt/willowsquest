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
        ToggleActive(false);
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
        ToggleActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleActive(false);
    }

    virtual public void ToggleActive(bool active)
    {
        if (HintObject != null)
        {
            IsActive = active;
            HintObject.SetActive(active);
        }
    }
}
