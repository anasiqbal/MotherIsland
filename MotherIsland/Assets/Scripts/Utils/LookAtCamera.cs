using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    private GameObject camera;
    
    private void Awake()
    {
        camera = Camera.main.gameObject;
    }

    private void Update()
    {
        transform.LookAt(camera.transform.position);
    }
}
