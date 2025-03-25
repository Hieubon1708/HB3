using UnityEngine;

namespace Hunter
{
    public class AK47Bullet : PlayerBullet
    {
        public override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bot"))
            {
                Bot bot = LevelController.instance.GetBot(other.gameObject);
                if (bot != null)
                {
                    bot.SubtractHp(100, PlayerController.instance.player.transform, false);
                }
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                DisableBullet();
            }
        }
    }
}
