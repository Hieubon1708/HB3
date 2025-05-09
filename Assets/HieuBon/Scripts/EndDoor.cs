using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace HieuBon
{
    public class EndDoor : MonoBehaviour
    {
        public GameObject leftDoor;
        public GameObject rightDoor;
        public float targetXLeft;
        public float targetXRight;
        public float time;
        Coroutine openDoor;
        public BoxCollider col;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                openDoor = StartCoroutine(Win(transform.position));
            }
        }

        public void StopDoor()
        {
            if (openDoor != null)
            {
                StopCoroutine(openDoor);
                openDoor = null;
            }
        }

        public IEnumerator Win(Vector3 endPointPosition)
        {
            PlayerController.instance.Pause();

            OpenDoor();
            yield return new WaitForSeconds(time / 3);
            PlayerController.instance.player.navMeshAgent.destination = endPointPosition;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => PlayerController.instance.player.navMeshAgent.remainingDistance == PlayerController.instance.player.navMeshAgent.stoppingDistance);
            yield return new WaitForSeconds(time / 3);
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.openDoor, 0);
            leftDoor.transform.DOLocalMoveX(0, time * 0.75f).SetEase(Ease.Linear);
            rightDoor.transform.DOLocalMoveX(0, time * 0.75f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(time);
            Action action = () =>
            {
                PlayerController.instance.player.navMeshAgent.enabled = false;
                PlayerController.instance.player.transform.DOLocalMoveY(PlayerController.instance.player.transform.localPosition.y + 10, 1f);
                PlayerController.instance.player.navMeshAgent.enabled = false;
                PlayerController.instance.transform.DOMoveY(PlayerController.instance.transform.position.y + 10, 1f);
                //UIInGame.instance.virtualCam.ElevatorMoveUp(1f);
                UIInGame.instance.ChangeMap();
            };
            UIInGame.instance.Win(action, 0f);
        }

        public void OpenDoor()
        {
            if (leftDoor.transform.localPosition.x == targetXLeft) return;
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.openDoor, 0);
            leftDoor.transform.DOLocalMoveX(targetXLeft, time).SetEase(Ease.Linear);
            rightDoor.transform.DOLocalMoveX(targetXRight, time).SetEase(Ease.Linear);
        }
    }
}