using UnityEngine;

namespace HieuBon
{
    public class BossWeapon : BotWeapon
    {
        public override void Attack(Transform target)
        {
            parWeapon.Play();
        }
    }
}