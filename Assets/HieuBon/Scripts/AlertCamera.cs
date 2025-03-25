using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class AlertCamera : MonoBehaviour
    {
        public GameObject[] points;
        public SpriteRenderer spot;
        int indexPoint = 1;
        Vector3[] targetPoints;
        List<GameObject> bots = new List<GameObject>();
        List<GameObject> players = new List<GameObject>();

        private void Start()
        {
            targetPoints = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                targetPoints[i] = points[i].transform.position;
            }
            transform.LookAt(targetPoints[0]);
        }

        public void Play()
        {
            Rotate();
        }

        void Rotate()
        {
            transform.DOLookAt(targetPoints[indexPoint], 2f).OnComplete(delegate
            {
                indexPoint++;
                if (indexPoint == points.Length) indexPoint = 0;
                Rotate();
            }).SetEase(Ease.Linear).SetDelay(0.5f);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !LevelController.instance.isAlert && PlayerController.instance.player.amountSmoke == 0)
            {
                CancelInvoke(nameof(ResumScan));
                spot.color = new Color(1, 0, 0, 0.5f);
                transform.DOPause();
                LevelController.instance.Alert(GameController.AlertType.Camera, gameObject);
                Invoke(nameof(CheckRemainingPoppyInSpot), 3f);
            }
            if (other.attachedRigidbody != null && other.name.Contains("Hip") & other.transform.parent.name == "Die")
            {
                if (!bots.Contains(other.attachedRigidbody.gameObject))
                {
                    CancelInvoke(nameof(ResumScan));
                    bots.Add(other.attachedRigidbody.gameObject);
                    LevelController.instance.Alert(GameController.AlertType.Camera, gameObject);
                    spot.color = new Color(1, 0, 0, 0.5f);
                    transform.DOPause();
                    Invoke(nameof(ResumScan), 2f);
                }
            }
        }

        void CheckRemainingPoppyInSpot()
        {
            bool isRemaining = false;
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < LevelController.instance.players.Count; j++)
                {
                    if (players[i] == LevelController.instance.players[j].gameObject)
                    {
                        isRemaining = true;
                        break;
                    }
                }
                if (isRemaining) break;
                else players.RemoveAt(i);
            }
            if (!isRemaining) ResumScan();
            else Invoke(nameof(CheckRemainingPoppyInSpot), 1f);
        }

        void ResumScan()
        {
            spot.color = Color.white;
            transform.DOPlay();
            LevelController.instance.isAlert = false;
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                players.Remove(other.gameObject);
                if (players.Count == 0)
                {
                    ResumScan();
                    CancelInvoke(nameof(CheckRemainingPoppyInSpot));
                }
            }
        }

        public void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
