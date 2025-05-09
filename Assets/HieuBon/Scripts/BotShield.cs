using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;

namespace HieuBon
{
    public class BotShield : BotSentry
    {
        public override IEnumerator Attack(GameObject target)
        {
            Player player = LevelController.instance.GetPlayer(target);
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");
            yield return new WaitForSeconds(aiming);
            player.SubtractHp(damage, transform);
            weapon.Attack(player.transform);
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.ak47Gun, 0);
            yield return new WaitForSeconds(rateOfFire);
            StopAttack();
        }
    }
}
