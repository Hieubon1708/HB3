using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class ShotgunBot : BotSniper
    {
        public override IEnumerator Attack(GameObject target)
        {
            animator.SetTrigger("Aiming");
            yield return new WaitForSeconds(aiming * 3);
            animator.SetTrigger("Fire");
            weapon.Attack(target.transform);

            float visionAngle = 15f * Mathf.Deg2Rad;
            float Currentangle = -visionAngle / 2;
            float angleIcrement = visionAngle / (3 - 1);
            float Sine;
            float Cosine;

            AudioController.instance.PlaySoundNVibrate(AudioController.instance.shotGun, 0);
            for (int i = 0; i < 5; i++)
            {

                BulletChase bullet = bullets[indexBullet];

                Sine = Mathf.Sin(Currentangle);
                Cosine = Mathf.Cos(Currentangle);

                bullet.gameObject.SetActive(false);
                bullet.rb.velocity = Vector3.zero;

                Vector3 dir = (transform.forward * Cosine) + (transform.right * Sine);
                bullet.transform.position = weapon.startBullet.position;
                bullet.transform.LookAt(bullet.transform.position + dir);
                bullet.transform.rotation = Quaternion.Euler(bullet.transform.localEulerAngles.x, bullet.transform.localEulerAngles.y, 90);
                bullet.trailRenderer.Clear();

                DOVirtual.DelayedCall(Random.Range(0f, 0.15f), delegate
                {
                    bullet.rb.velocity = dir * bulletSpeed;
                    bullet.gameObject.SetActive(true);
                });

                indexBullet++;
                if (indexBullet == bullets.Length) indexBullet = 0;

                Currentangle += angleIcrement;
            }

            yield return new WaitForSeconds(rateOfFire);
            StopAttack();
        }
    }
}
