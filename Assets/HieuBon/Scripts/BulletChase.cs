using UnityEngine;

namespace Hunter
{
    public class BulletChase : Bullet
    {    
        [HideInInspector]
        public float timeHide;
        float time;

        public override void Init(int damage, string tag, float timeHide, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt)
        {
            base.Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
            this.timeHide = timeHide;
        }

        void FixedUpdate()
        {
            if (!col.enabled) return;
            if (time >= timeHide)
            {
                if(mesh.enabled)
                {
                    Disable();
                }
                return;
            }

            time += Time.fixedDeltaTime;

            Vector3 target = PlayerController.instance.player.transform.position;
            Vector3 dir = target - transform.position;
            dir.Normalize();

            float angle = Vector3.Cross(transform.forward, dir).y;

            rb.angularVelocity = new Vector3(0, angle * angularSpeed, 0);

            rb.velocity = transform.forward * speed;
        }
    }
}
