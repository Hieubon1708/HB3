using UnityEngine;

namespace Hunter
{
    public class PlayerShotGunBullet : PlayerBullet
    {
        float time;

        public override void ResetBullet()
        {
            time = 0;
            base.ResetBullet();
        }

        void FixedUpdate()
        {
            if (!col.enabled) return;
            time += Time.fixedDeltaTime;
            if(time >= 0.3f)
            {
                rb.isKinematic = true;
                mesh.SetActive(false);
                col.enabled = false;
            }
        }
    }
}
