using DG.Tweening;
using System;
using UnityEngine;

namespace Hunter
{
    public class BotRPGBullet : MonoBehaviour
    {
        public Rigidbody rb;
        public TrailRenderer trailRenderer;
        Vector3 lastPosition;
        public ParticleSystem par;
        public GameObject mesh;
        public SphereCollider col;
        public Bot bot;

        private void Update()
        {
            if (!mesh.activeSelf) return;
            Vector3 dir = transform.position - lastPosition;
            transform.rotation = Quaternion.LookRotation(dir);
            lastPosition = transform.position;
        }

        public void Init(Vector3 position)
        {
            col.enabled = false;
            gameObject.SetActive(false);
            mesh.SetActive(true);
            transform.position = position;
            transform.rotation = Quaternion.identity;
            trailRenderer.Clear();
            gameObject.SetActive(true);
        }

        public void OnGround()
        {
            mesh.SetActive(false);
            par.Play();
            col.enabled = true;
            trailRenderer.Clear();
            UIInGame.instance.virtualCam.StartShakeCam(7.5f);
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.blastBarrel, 50);
            DOVirtual.DelayedCall(0.02f, delegate
            {
                col.enabled = false;
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player player = LevelController.instance.GetPlayer(other.gameObject);
                player.SubtractHp(bot.damage, transform);
            }
        }
    }
}
