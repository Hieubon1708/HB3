using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class Electric : MonoBehaviour
    {
        public ParticleSystem par;
        public ParticleSystem fxStop;
        public ParticleSystem dealth;
        public float timeOn;
        public float timeOff;
        public BoxCollider col;
        Coroutine loop;
        int damage = 10;

        void Start()
        {
            StartElectric();
            //damage = (int)(damage * (1 + 0.09f * (Manager.instance.Chapter - 1)));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                loop = StartCoroutine(Loop(other.gameObject));
            }
        }

        IEnumerator Loop(GameObject other)
        {
            Player player = LevelController.instance.GetPlayer(other);
            while (player.col.enabled)
            {
                dealth.Play();
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

        void StartElectric()
        {
            par.Play();
            Invoke(nameof(StopElectric), timeOn);
            Invoke(nameof(ColOn), 1f);
        }

        void ColOn()
        {
            col.enabled = true;
        }

        void StopElectric()
        {
            if(loop != null) StopCoroutine(loop);
            col.enabled = false;
            par.Stop();
            fxStop.Play();
            Invoke(nameof(StartElectric), timeOff);
        }
    }
}
