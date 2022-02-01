using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    public float Speed = 1.0f;
    public bool  Disabled = false;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Disabled) return;

        var temp = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            temp += Vector3.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            temp += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            temp += Vector3.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            temp += Vector3.right;
        }
        if (!temp.Equals(Vector3.zero)) 
        {
            _rb.MovePosition(_rb.transform.position + (temp * Speed));
        }
    }
}
