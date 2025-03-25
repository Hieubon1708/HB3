using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class BotNormal : BotSentry
    {
        public override IEnumerator Attack(GameObject target)
        {
            Player player = LevelController.instance.GetPlayer(target);
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");
            yield return new WaitForSeconds(aiming);
            while (player.col.enabled)
            {
                player.SubtractHp(damage, transform);
                weapon.Attack(player.transform);
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.ak47Gun, 0);
                yield return new WaitForSeconds(rateOfFire);
            }
        }
    }
}
