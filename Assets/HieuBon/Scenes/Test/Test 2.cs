using Hunter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public Transform[] cubes;

    float radius = 2;

    public void Awake()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].position = new Vector3(
                   transform.position.x + radius * Mathf.Cos(2 * Mathf.PI * i / cubes.Length),
                   transform.position.y,
                   transform.position.z + radius * Mathf.Sin(2 * Mathf.PI * i / cubes.Length));
        }
    }
}
