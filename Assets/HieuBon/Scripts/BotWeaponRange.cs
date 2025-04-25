using UnityEngine;

namespace HieuBon
{
    public class BotWeaponRange : BotWeapon
    {
        public override void Attack(Transform target)
        {
            parWeapon.Play();
        }
    }
}
