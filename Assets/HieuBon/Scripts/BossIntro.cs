using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    public class BossIntro : MonoBehaviour
    {
        public CinemachineVirtualCamera cam1;
        public CinemachineVirtualCamera cam2;

        Boss boss;

        private void Start()
        {
            boss = LevelController.instance.GetBoss() as Boss;
            cam1.m_LookAt = boss.transform;

            boss.health.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<BoxCollider>().enabled = false;

                StartCoroutine(StartIntro());
            }
        }

        IEnumerator StartIntro()
        {
            PlayerController.instance.Pause();

            cam1.gameObject.SetActive(true);

            yield return new WaitForSeconds(6.5f);

            cam1.gameObject.SetActive(false);

            cam2.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            cam2.gameObject.SetActive(false);

            boss.health.gameObject.SetActive(true);

            PlayerController.instance.Resume();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(StartIntro());

            }
        }
    }
}
