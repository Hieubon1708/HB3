using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HieuBon
{
    public class IronDoor : MonoBehaviour
    {
        public BoxCollider col;
        public GameObject door;
        public TrapKey key;
        public NavMeshObstacle obstacle;

        private void Start()
        {
            door.SetActive(true);
        }

        public void OpenDoor()
        {
            col.enabled = false;
            obstacle.enabled = false;
            door.transform.DOLocalRotate(Vector3.up * 125f, 0.5f);
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
