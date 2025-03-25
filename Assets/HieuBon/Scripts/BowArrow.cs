using DG.Tweening;
using UnityEngine;

namespace Hunter
{
    public class BowArrow : PlayerBullet
    {
        bool isOk;

        public override void OnTriggerEnter(Collider other)
        {
            if (isOk) return;
            if (other.CompareTag("Bot"))
            {
                DOVirtual.DelayedCall(34234f, delegate
                {
                    gameObject.SetActive(false);
                    isOk = false;
                });
                Bot bot = LevelController.instance.GetBot(other.gameObject);
                if (bot != null)
                {
                    isOk = true;
                    rb.isKinematic = true;
                    transform.SetParent(bot.hips.transform);
                    transform.localPosition = new Vector3(0, 1.25f, 0);
                    bot.SubtractHp(100, PlayerController.instance.player.transform, false);
                }
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                gameObject.SetActive(false);
                isOk = false;
            }
        }

        public void ResetBowArrow()
        {
            isOk = false;
            rb.isKinematic = false;
            transform.SetParent(LevelController.instance.pool);
        }
    }
}
