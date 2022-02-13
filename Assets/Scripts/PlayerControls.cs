using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    //public float Speed = 1.0f;
    public bool  Disabled = false;

    private Rigidbody2D _rb;
    public float MovementScale = 1;

    private bool _attributesOpen = false;
    private bool _inventoryOpen = false;

    private UIManager _ui;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;

        _ui = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Disabled) return;

        /*var temp = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            temp += Vector3.up;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            temp += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            temp += Vector3.down;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            temp += Vector3.right;
        }
        if (!temp.Equals(Vector3.zero)) 
        {
            _rb.MovePosition(_rb.transform.position + (temp * Speed));
        }*/
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _rb.AddRelativeForce(Vector2.left * MovementScale, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _rb.AddRelativeForce(Vector2.right * MovementScale, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _rb.AddRelativeForce(Vector2.up * MovementScale, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _rb.AddRelativeForce(Vector2.down * MovementScale, ForceMode2D.Impulse);
        }

        CheckMenu(KeyCode.I, ref _inventoryOpen, _ui.OpenInventory, _ui.CloseInventory);
        CheckMenu(KeyCode.Q, ref _attributesOpen, _ui.OpenAttributes, _ui.CloseAttributes);
   }

    private void CheckMenu(KeyCode key, ref bool open, Action openAction, Action closeAction)
    {
        if (Input.GetKeyDown(key))
        {
            if (open)
            {
                closeAction.Invoke();
                open = false;
            }
            else
            {
                openAction.Invoke();
                open = true;
            }
        }
    }
}
