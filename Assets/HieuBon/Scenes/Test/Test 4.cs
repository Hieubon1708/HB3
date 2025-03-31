using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test4 : MonoBehaviour
{
    public Transform target;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            float distance = Vector3.Distance(transform.position, target.position);
            transform.localScale = new Vector3(1, 1, distance);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, distance / 2);
            Debug.Log(distance);
        }
    }
}
