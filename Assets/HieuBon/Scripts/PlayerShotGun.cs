using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class PlayerShotGun : PlayerWeaponRangeBullet
    {
        public override void Init(Player player)
        {
            base.Init(player);
            player.animator.SetBool("Two Handed", true);
            player.animator.SetTrigger("Run");
        }

        public override IEnumerator Attack(Transform target)
        {
            player.animator.SetTrigger("Shot");
            DOVirtual.Float(0f, 1f, 0.467f, (v) =>
            {
                aim.position = target.position;
                aimConstraint.weight = v;
            }).SetUpdate(true).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.467f);

            AudioController.instance.PlaySoundNVibrate(AudioController.instance.shotGun, 0);
            parWeapon.Play();
            player.ChangeLookAt();

            float visionAngle = 30f * Mathf.Deg2Rad;
            float Currentangle = -visionAngle / 2;
            float angleIcrement = visionAngle / (5 - 1);
            float Sine;
            float Cosine;
            for (int i = 0; i < 5; i++)
            {
                PlayerBullet bullet = bullets[indexBullet];

                Sine = Mathf.Sin(Currentangle);
                Cosine = Mathf.Cos(Currentangle);
                bullet.gameObject.SetActive(false);
                bullet.ResetBullet();
                bullet.rb.velocity = Vector3.zero;
                Vector3 dir = (transform.forward * Cosine) + (transform.right * Sine);
                bullet.transform.position = startBullet.position;
                bullet.transform.LookAt(bullet.transform.position + dir);
                bullet.trailRenderer.Clear();

                DOVirtual.DelayedCall(Random.Range(0f, 0.15f), delegate
                {
                    bullet.rb.velocity = dir * speedBullet;
                    bullet.gameObject.SetActive(true);
                });

                indexBullet++;
                if (indexBullet == bullets.Length) indexBullet = 0;

                Currentangle += angleIcrement;
            }

            player.animator.SetTrigger("No Shot");
            DOVirtual.Float(1f, 0f, 0.467f, (v) =>
            {
                aimConstraint.weight = v;
            }).SetUpdate(true).SetEase(Ease.Linear);
            PlayerController.instance.player.navMeshAgent.speed *= 2;
            yield return new WaitForSeconds(0.467f);
            player.isKilling = false;
        }
    }
}
