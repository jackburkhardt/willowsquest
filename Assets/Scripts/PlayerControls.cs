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

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _rb.AddRelativeForce(Vector2.right * MovementScale, ForceMode2D.Impulse);
        }

        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _rb.AddRelativeForce(Vector2.up * MovementScale, ForceMode2D.Impulse);
        }

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _rb.AddRelativeForce(Vector2.down * MovementScale, ForceMode2D.Impulse);
        }
    }
}
