using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class BulletGlider : Bullet
    {
        float timeGlider = 0.25f;
        float time;

        public override void Init(int damage, string tag, float timeHide, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt)
        {
            base.Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
        }

        void FixedUpdate()
        {
            if (!col.enabled) return;

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
}
