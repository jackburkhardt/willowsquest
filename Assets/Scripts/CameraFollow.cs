
using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform FollowTransform;
    //public Transform background;
    public bool gameActive;
        
    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (!gameActive) return;

        // if (FollowTransform == null)
        // {
        //     FollowTransform = GameObject.Find("CenterTransform").transform;
        // }
        
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10), 
            new Vector3(FollowTransform.position.x, FollowTransform.position.y, -10), 0.08f);
        //background.position = Vector3.Lerp(new Vector3(background.position.x, background.position.y, -10), 
        //    new Vector3(transform.position.x, transform.position.y, -10), 0.05f);
    }
}

