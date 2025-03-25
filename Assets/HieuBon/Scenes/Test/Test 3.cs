using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{
    Rigidbody rb;

    public GameObject a;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = new Vector3(3, 0, 3);

        transform.LookAt(transform.position + rb.velocity);
    }

    private Vector3 previousPosition;

    void Update()
    {
        previousPosition = transform.position;

    }

    int count;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            count++;

            Vector3 collisionDirection = transform.position - previousPosition;
            collisionDirection.Normalize();

            RaycastHit hit;
            if (Physics.Raycast(transform.position, collisionDirection, out hit, 5))
            {
                Vector3 normal = hit.normal;
                Vector3 reflectionDirection = Vector3.Reflect(collisionDirection, normal);

                Debug.Log("Hướng phản xạ (raycast): " + reflectionDirection);

                rb.velocity = reflectionDirection * 5;

                transform.LookAt(transform.position + rb.velocity);
            }
            else
            {
                Debug.Log("Raycast không chạm vào gì cả.");
            }
            if (count > 2) gameObject.SetActive(false);
        }
    }

    Vector3 CalculateReflection(Vector3 collisionDirection, Vector3 wallNormal)
    {
        return Vector3.Reflect(collisionDirection, wallNormal);
    }
}
