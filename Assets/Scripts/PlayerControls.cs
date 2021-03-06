using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    public float Speed = 1f;
    public bool Disabled = false;

    private Rigidbody2D _rb;

    private UIManager _ui;
    private bool _attributesOpen = false;
    private bool _inventoryOpen = false;

    private Animator _anim;

    private Attributes _attr;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ui = FindObjectOfType<UIManager>();
        _anim = GetComponent<Animator>();
        _attr = FindObjectOfType<Player>().Attributes;
    }

    void FixedUpdate()
    {
        if (Disabled)
        {
            _rb.velocity = Vector2.zero;
            _anim.Play("willowstaticf");
            return;
        }

        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x = -1;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir.x = 1;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir.y = -1;
        }

        PlayAnimation(dir);

        dir.Normalize();
        _rb.velocity = Speed * dir;
    }

    void Update()
    {
        if (Disabled) return;

        if (_attr.HP < 1)
        {
            _ui.GameOver();
        }

        CheckMenu(KeyCode.Tab, ref _inventoryOpen, _ui.OpenInventory, () => _ui.CloseInventory(false));
        CheckMenu(KeyCode.Q, ref _attributesOpen, _ui.OpenAttributes, _ui.CloseAttributes);
    }

    void CheckMenu(KeyCode key, ref bool open, Action openAction, Action closeAction)
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

    void PlayAnimation(Vector2 dir)
    {
        switch (dir.y)
        {
            case -1:
                _anim.Play("willowforward");
                break;
            case 1:
                _anim.Play("willowbackwards");
                break;
            default:
                switch (dir.x)
                {
                    case -1:
                        _anim.Play("willowleft");
                        break;
                    case 1:
                        _anim.Play("willowright");
                        break;
                    default:
                        _anim.Play("willowstaticf");
                        break;
                }
                break;
        }
    }
}
