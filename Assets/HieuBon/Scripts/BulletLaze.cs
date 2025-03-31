using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Hunter
{
    public class BulletLaze : MonoBehaviour
    {
        Transform parent;
        LayerMask mask;

        public void Awake()
        {
            mask = LayerMask.GetMask("Player", "Wall");
        }

        public void Init(Vector3 lookAt, Transform parent)
        {
            this.parent = parent;

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.zero;

            transform.LookAt(transform.position + lookAt);

            gameObject.SetActive(true);
        }

        public void Update()
        {
            RaycastHit hit;

            Physics.Raycast(parent.position, transform.forward, out hit, 10, mask);

            if (hit.collider != null)
            {
                float distance = Vector3.Distance(parent.position, hit.point);
                transform.localScale = new Vector3(1, 1, distance);
                transform.localPosition = new Vector3(0, 0, distance / 2);
            }
            else
            {
                Debug.LogError("!");
            }
        }
    }
}
