using DG.Tweening;
using UnityEngine;

namespace Hunter
{
    public class PlayerRPGBullet : PlayerBullet
    {
        public GameObject explosion;
        public ParticleSystem fxExplosion;

        public override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bot") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                DisableBullet();
                explosion.SetActive(true);
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.blastBarrel, 50);
                UIInGame.instance.virtualCam.StartShakeCam(7.5f);
                fxExplosion.transform.position = new Vector3(fxExplosion.transform.position.x, other.transform.position.y, fxExplosion.transform.position.z);
                fxExplosion.Play();
                DOVirtual.DelayedCall(0.1f, delegate
                {
                    explosion.SetActive(false);
                });
            }
        }
    }
}
