using Hunter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody rb;
    public Transform target;
    float timeGlider = 0.5f;
    float time;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float speed = 3f;
    float angularSpeed;

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        if (Math.Round(time, 2) / timeGlider != 0 && Math.Round(time, 2) % timeGlider == 0)
        {
            angularSpeed = UnityEngine.Random.Range(-1f, 1f);
            //Debug.Log(Math.Round(time, 2));
        }

        rb.angularVelocity = new Vector3(0, angularSpeed, 0);

        rb.velocity = transform.forward * speed;
    }
}
