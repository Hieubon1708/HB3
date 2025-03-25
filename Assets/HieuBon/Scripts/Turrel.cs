using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class Turrel : MonoBehaviour
    {
        public List<GameObject> players = new List<GameObject>();
        Coroutine shot;
        Coroutine rotateBody;
        public float[] angles;
        public float time;
        public SpriteRenderer spot;
        int indexPoint = 1;
        public ParticleSystem light1;
        public ParticleSystem light2;
        public SpriteRenderer laser1;
        public SpriteRenderer laser2;
        public GameObject gun1;
        public GameObject gun2;
        public GameObject body;
        int damage = 5;


        private void Start()
        {
            transform.rotation = Quaternion.Euler(0, angles[0], 0);
            //damage = (int)(damage * (1 + 0.09f * (Manager.instance.Chapter - 1)));
        }

        public void Play()
        {
            Rotate();
        }

        public void ResetTurrel()
        {
            players.Clear();
            transform.DOKill();
            transform.localRotation = Quaternion.Euler(0, angles[0], 0);
            indexPoint = 1;
            spot.color = Color.white;
            if (shot != null) StopCoroutine(shot);
            if (rotateBody != null) StopCoroutine(rotateBody);
            rotateBody = null;
            shot = null;
        }

        void Rotate()
        {
            transform.DOLocalRotate(new Vector3(0, angles[indexPoint], 0), time, RotateMode.Fast).OnComplete(delegate
            {
                indexPoint++;
                if (indexPoint == angles.Length) indexPoint = 0;
                Rotate();
            }).SetEase(Ease.Linear).SetDelay(0.5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (PlayerController.instance.player.amountSmoke > 0) return;
            if (other.CompareTag("Player"))
            {
                if (!players.Contains(other.gameObject))
                {
                    players.Add(other.gameObject);
                }
                if (shot == null)
                {
                    AudioController.instance.PlayTurrel();
                    spot.color = new Color(1, 0, 0, 0.5f);
                    transform.DOPause();
                    shot = StartCoroutine(Shot());
                    rotateBody = StartCoroutine(RotateBody());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (players.Contains(other.gameObject))
                {
                    players.Remove(other.gameObject);
                }
                if (players.Count == 0)
                {
                    Resum();
                }
            }
        }


        void Resum()
        {
            AudioController.instance.StopTurrel();
            spot.color = Color.white;
            transform.DOPlay();
            if (shot != null) StopCoroutine(shot);
            if (rotateBody != null) StopCoroutine(rotateBody);
            rotateBody = null;
            shot = null;
        }

        IEnumerator RotateBody()
        {
            yield return null;
        }

        IEnumerator Shot()
        {
            Player player = LevelController.instance.GetPlayer(players[0]);
            while (player.col.enabled)
            {
                yield return new WaitForSeconds(0.15f);
                light1.Play();
                gun1.transform.DOLocalMoveZ(0.3f, 0.15f / 2).SetEase(Ease.Linear).OnComplete(delegate
                {
                    gun1.transform.DOLocalMoveZ(-0.0349713f, 0.15f / 2).SetEase(Ease.Linear);
                });
                laser1.DOFade(30f / 255f, 0.15f / 2).SetEase(Ease.Linear).OnComplete(delegate
                {
                    laser1.DOFade(0f, 0.15f / 2).SetEase(Ease.Linear);
                });
                player.SubtractHp(damage, transform);
                yield return new WaitForSeconds(0.15f);
                light2.Play();
                gun2.transform.DOLocalMoveZ(0.3f, 0.15f / 2).SetEase(Ease.Linear).OnComplete(delegate
                {
                    gun2.transform.DOLocalMoveZ(-0.0349713f, 0.15f / 2f).SetEase(Ease.Linear);
                });
                laser2.DOFade(30f / 255f, 0.15f / 2).SetEase(Ease.Linear).OnComplete(delegate
                {
                    laser2.DOFade(0f, 0.15f / 2).SetEase(Ease.Linear);
                });
            }
            Resum();
        }

        void DoKill()
        {
            transform.DOKill();
            gun1.transform.DOKill();
            gun2.transform.DOKill();
            laser1.transform.DOKill();
            laser2.transform.DOKill();
        }

        private void OnDestroy()
        {
            DoKill();
        }
    }
}
