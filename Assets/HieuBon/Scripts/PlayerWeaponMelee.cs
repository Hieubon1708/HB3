using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class PlayerWeaponMelee : PlayerWeapon
    {
        public override void Init(Player player)
        {
            base.Init(player);
            player.animator.SetBool("Two Handed", false);
        }

        public override IEnumerator Attack(Transform target)
        {
            yield return null;
        }
    }
}
