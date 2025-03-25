using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Hunter
{
    public class PlayerWeaponRange : PlayerWeapon
    {
        public SpriteRenderer laser1;
        public SpriteRenderer laser2;
        public BoxCollider col;

        public override void Init(Player player)
        {
            base.Init(player);
            player.animator.SetBool("Two Handed", false);
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

            laser1.DOKill();
            laser2.DOKill();
            laser1.color = new Color(1, 1, 1, 0);
            laser2.color = new Color(1, 1, 1, 0);
            parWeapon.Play();
            col.enabled = true;
            laser1.transform.localScale = new Vector3(laser1.transform.localScale.x, Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z)) * 4, laser1.transform.localScale.z);
            laser2.transform.localScale = new Vector3(laser2.transform.localScale.x, Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z)) * 4, laser2.transform.localScale.z);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.pistol, 0);

            laser1.DOFade(70f / 255f, 0.025f).OnComplete(delegate
            {
                Bot bot = LevelController.instance.GetBot(target.gameObject);
                if (bot != null)
                {
                    bot.SubtractHp(100, PlayerController.instance.player.transform, false);
                }
                player.ChangeLookAt();
                col.enabled = false;
                laser1.DOFade(0, 0f);
                PlayerController.instance.player.navMeshAgent.speed *= 2;
            }).SetUpdate(true);
            laser2.DOFade(70f / 255f, 0.025f).OnComplete(delegate
            {
                laser2.DOFade(0, 0f);
            }).SetUpdate(true);
            player.animator.SetTrigger("No Shot");
            DOVirtual.Float(1f, 0f, 0.467f, (v) =>
            {
                aimConstraint.weight = v;
            }).SetUpdate(true).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.467f);
            player.isKilling = false;
        }
    }
}
