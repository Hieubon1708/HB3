using UnityEngine;

namespace Hunter
{
    public abstract class BotWeapon : MonoBehaviour
    {
        public ParticleSystem parWeapon;
        public Transform startBullet;
        public Bot bot;

        public abstract void Attack(Transform target);
    }
}
