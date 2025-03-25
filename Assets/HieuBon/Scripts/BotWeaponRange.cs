using UnityEngine;

namespace Hunter
{
    public class BotWeaponRange : BotWeapon
    {
        public override void Attack(Transform target)
        {
            parWeapon.Play();
        }
    }
}
