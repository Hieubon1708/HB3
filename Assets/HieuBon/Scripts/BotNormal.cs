using System.Collections;
using UnityEngine;

namespace HieuBon
{
    public class BotNormal : BotSentry
    {
        public override IEnumerator Attack(GameObject target)
        {
            Player player = LevelController.instance.GetPlayer(target);
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");
            yield return new WaitForSeconds(0.867f / 3f);
            while (player.col.enabled && col.enabled)
            {
                player.SubtractHp(damage, transform);
                weapon.Attack(player.transform);
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.ak47Gun, 0);
                yield return new WaitForSeconds(rateOfFire);
            }
        }
    }
}
