using DG.Tweening;
using UnityEngine;

namespace Hunter
{
    public class PlayerBullet : MonoBehaviour
    {
        public Rigidbody rb;
        public TrailRenderer trailRenderer;
        public GameObject mesh;
        public SphereCollider col;

        public virtual void ResetBullet()
        {
            mesh.SetActive(true);
            col.enabled = true;
            rb.isKinematic = false;
        }

        public virtual void DisableBullet()
        {
            rb.isKinematic = true;
            mesh.SetActive(false);
            col.enabled = false;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                gameObject.SetActive(false);
            }

            if (other.CompareTag("Bot") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                DisableBullet();

                if (other.CompareTag("Bot"))
                {
                    Bot bot = LevelController.instance.GetBot(other.gameObject);
                    if (bot != null)
                    {
                        bot.SubtractHp(100, PlayerController.instance.player.transform, false);
                    }
                }
            }
        }
    }
}
