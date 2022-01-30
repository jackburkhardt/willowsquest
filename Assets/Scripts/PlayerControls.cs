using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float Speed;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            temp += Vector3.up * Speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            temp += Vector3.left * Speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            temp += Vector3.down * Speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            temp += Vector3.right * Speed;
        }
        if (!temp.Equals(Vector3.zero)) 
        {
            _rb.MovePosition(_rb.transform.position + temp);
        }
    }
}
