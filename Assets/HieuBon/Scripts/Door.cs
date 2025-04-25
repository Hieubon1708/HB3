using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace HieuBon
{
    public class Door : MonoBehaviour
    {
        public Rigidbody[] locks;
        public BoxCollider col;
        public ParticleSystem open;
        public ParticleSystem parLock;
        public GameObject door;
        public TrapKey key;
        public NavMeshObstacle obstacle;

        private void Start()
        {
            foreach (var l in locks)
            {
                l.gameObject.SetActive(true);
            }

            door.SetActive(true);
        }

        public void OpenDoor()
        {
            parLock.Stop();
            locks[0].isKinematic = false;
            locks[1].isKinematic = false;
            col.enabled = false;
            open.Play();
            obstacle.enabled = false;
            door.transform.DOLocalRotate(Vector3.up * 90, 0.5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (LevelController.instance.IsKey(key))
                {
                    AudioController.instance.PlaySoundNVibrate(AudioController.instance.openDoor, 0);
                    OpenDoor();
                    key.gameObject.SetActive(false);
                }
            }
        }
    }
}
