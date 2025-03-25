using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class StartDoor : MonoBehaviour
    {
        public GameObject leftDoor;
        public GameObject rightDoor;
        public float targetXLeft;
        public float targetXRight;
        public float time;

        public void OpenDoor()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.openDoor, 0);
            leftDoor.transform.DOLocalMoveX(targetXLeft, time).SetEase(Ease.Linear);
            rightDoor.transform.DOLocalMoveX(targetXRight, time).SetEase(Ease.Linear);
        }

        public IEnumerator ElevatorMoveUp(List<Player> players)
        {
            UIInGame.instance.layerCover.raycastTarget = true;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 10, transform.localPosition.z);
            Transform playerController = PlayerController.instance.transform;
            playerController.position = Vector3.zero;
            playerController.DOKill();
            float startY = playerController.localPosition.y;
            PlayerController.instance.handTutorial.Hide();
            PlayerController.instance.player.navMeshAgent.enabled = false;
            playerController.localPosition = new Vector3(playerController.localPosition.x, playerController.localPosition.y - 10, playerController.localPosition.z);
            for (int i = 0; i < players.Count; i++)
            {
                players[i].DOKill();
                players[i].navMeshAgent.enabled = false;
                players[i].transform.localPosition = new Vector3(players[i].transform.localPosition.x, players[i].transform.localPosition.y - 10, players[i].transform.localPosition.z);
            }
            yield return new WaitForSeconds(0.5f);
            AudioController.instance.PlayElevator();
            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.DOLocalMoveY(startY, 1f);
            }
            playerController.DOLocalMoveY(startY, 1f).OnComplete(delegate
            {
                PlayerController.instance.player.navMeshAgent.enabled = true;
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].navMeshAgent.enabled = true;
                }
                AudioController.instance.StopElevator();
                OpenDoor();
                if (LevelController.instance.IsBoss()) StartCoroutine(UIInGame.instance.BossIntro());
                else
                {
                    PlayerController.instance.handTutorial.PlayHand();
                    //UIManager.instance.ShowUIHome();
                    DOVirtual.DelayedCall(0.5f, delegate
                    {
                        UIInGame.instance.layerCover.raycastTarget = false;
                    });
                }
            });
            transform.DOLocalMoveY(0, 1f);
        }
    }
}
