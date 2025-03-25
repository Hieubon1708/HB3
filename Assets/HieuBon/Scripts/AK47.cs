using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class AK47 : PlayerWeaponRangeBullet
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
            aim.position = target.position;
            DOVirtual.Float(0f, 1f, 0.3f, (v) =>
            {
                aimConstraint.weight = v;
            }).SetUpdate(true).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.3f);
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.ak47Gun, 0);
            parWeapon.Play();
            player.ChangeLookAt();

            bullets[indexBullet].gameObject.SetActive(false);
            bullets[indexBullet].ResetBullet();
            bullets[indexBullet].rb.velocity = Vector3.zero;

            Vector3 dir = (new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position).normalized;
            bullets[indexBullet].transform.position = startBullet.transform.position;
            bullets[indexBullet].rb.velocity = dir * speedBullet;
            bullets[indexBullet].transform.LookAt(bullets[indexBullet].transform.position + dir);
            bullets[indexBullet].trailRenderer.Clear();
            bullets[indexBullet].gameObject.SetActive(true);
            indexBullet++;
            if (indexBullet == bullets.Length) indexBullet = 0;

            player.animator.SetTrigger("No Shot");
            DOVirtual.Float(1f, 0f, 0.3f, (v) =>
            {
                aimConstraint.weight = v;
            }).SetUpdate(true).SetEase(Ease.Linear);
            PlayerController.instance.player.navMeshAgent.speed *= 2;
            yield return new WaitForSeconds(0.3f);
            player.isKilling = false;
        }
    }
}