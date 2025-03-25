using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class BulletSpawn : Bullet
    {
        Bullet[] bulletSpawns;
        float timeSpawn;
        float time;
        int indexBulletSpawn;

        public override void Init(int damage, string tag, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt, float timeSpawn, Bullet[] bulletSpawns)
        {
            base.Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
            this.timeSpawn = timeSpawn;
            this.bulletSpawns = bulletSpawns;

            time = 0;
        }

        void FixedUpdate()
        {
            if (!col.enabled) return;

            time += Time.fixedDeltaTime;

            if (time >= timeSpawn)
            {
                Disable();
                Spawn();
            }

            rb.velocity = transform.forward * speed;
        }

        void Spawn()
        {
            int random = Random.Range(3, 6);

            float VisionAngle = 360f * Mathf.Deg2Rad;
            float Currentangle = -VisionAngle / 2;
            float angleIcrement = VisionAngle / random;
            float Sine;
            float Cosine;

            for (int i = 0; i < random; i++)
            {
                Sine = Mathf.Sin(Currentangle);
                Cosine = Mathf.Cos(Currentangle);

                Vector3 dir = (transform.forward * -Cosine) + (transform.right * -Sine);

                bulletSpawns[indexBulletSpawn].Init(damage, "Player", 2, 5, 0, transform.position, transform.position + dir * 5);

                indexBulletSpawn++;
                if (indexBulletSpawn == bulletSpawns.Length) indexBulletSpawn = 0;

                Currentangle += angleIcrement;
            }
        }
    }
}
