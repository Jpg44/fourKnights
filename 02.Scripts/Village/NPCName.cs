using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCName : MonoBehaviour
{
    public GameObject cam;

    void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
    private void LateUpdate()
    {
        if(cam != null)
        {
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}