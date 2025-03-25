using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class LaserLine : MonoBehaviour
    {
        public ParticleSystem laserDamage;
        Coroutine loop;
        int damage = 10;

        public void Start()
        {
            //damage = (int)(damage * (1 + 0.09f * (Manager.instance.Chapter - 1)));
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LevelController.instance.Alert(GameController.AlertType.Laser, gameObject);
                loop = StartCoroutine(Loop(other.gameObject));
            }
        }

        IEnumerator Loop(GameObject other)
        {
            Player player = LevelController.instance.GetPlayer(other);
            while (player.col.enabled)
            {
                laserDamage.Play();
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.laser, 0);
                player.SubtractHp(damage, transform);
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StopCoroutine(loop);
            }
        }
    }
}
