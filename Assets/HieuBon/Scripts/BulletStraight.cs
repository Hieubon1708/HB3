using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class BulletStraight : Bullet
    {
        void FixedUpdate()
        {
            if (!col.enabled) return;

            rb.velocity = transform.forward * speed;
        }
    }
}
