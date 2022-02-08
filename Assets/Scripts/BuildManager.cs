using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
#if UNITY_STANDALONE
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            Application.Quit(); 
        }
    }
#endif
}