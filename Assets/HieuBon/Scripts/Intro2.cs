using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    public class Intro2 : MonoBehaviour
    {
        public ParticleSystem fxEnd;
        public Transform helicopter;
        public Transform pointSpawnPlayer;
        public GameObject cam;

        private void Start()
        {
            LevelController.instance.LoadPlayer(pointSpawnPlayer.position);

            PlayerController.instance.transform.SetParent(pointSpawnPlayer.transform);
            PlayerController.instance.transform.localRotation = Quaternion.Euler(0, PlayerController.instance.transform.localEulerAngles.y + 180, 0);

            ActivePlayer(false);
        }

        void ActivePlayer(bool isActive)
        {
            PlayerController.instance.player.navMeshAgent.enabled = isActive;
            PlayerController.instance.player.outline.enabled = isActive;
            PlayerController.instance.player.weapon.gameObject.SetActive(isActive);
            PlayerController.instance.player.health.gameObject.SetActive(isActive);
        }

        IEnumerator LookAtPlayer()
        {
            while (cam.activeSelf)
            {
                Vector3 lookDirection = PlayerController.instance.transform.position - cam.transform.position;
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 10);
                yield return new WaitForEndOfFrame();
            }
        }

        public void HelicopterLanded()
        {
            PlayerController.instance.player.animator.SetTrigger("Land");

            DOVirtual.DelayedCall(0.75f, delegate
            {
                StartCoroutine(LookAtPlayer());

                PlayerController.instance.transform.DOJump(Vector3.zero, 5f, 1, 1f).SetEase(Ease.InOutQuad).OnComplete(delegate
                {
                    PlayerController.instance.transform.SetParent(LevelController.instance.transform);

                    ActivePlayer(true);

                    UIInGame.instance.virtualCam.StartShakeCam(5f);

                    DOVirtual.DelayedCall(0.5f, delegate
                    {
                        cam.SetActive(false);

                        PlayerController.instance.transform.DOLocalRotate(Vector3.zero, 0.5f).OnComplete(delegate
                        {
                            UIInGame.instance.LoadUI(false);
                        });
                    });

                    UIInGame.instance.virtualCam.ResetCam();

                    PlayerController.instance.player.animator.SetTrigger("AfterLand");

                    helicopter.DOMove(new Vector3(helicopter.position.x - 10, helicopter.position.y + 10, helicopter.position.z), 2.5f).SetEase(Ease.InOutQuad).SetDelay(0.5f).OnComplete(delegate
                    {
                        helicopter.gameObject.SetActive(false);
                    });
                });

                DOVirtual.DelayedCall(0.85f, delegate
                {
                    fxEnd.transform.position = Vector3.zero;
                    fxEnd.Play();
                });
            });
        }
    }
}
