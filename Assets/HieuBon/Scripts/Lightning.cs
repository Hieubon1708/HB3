using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class Lightning : MonoBehaviour
    {
        string targetTag;
        int damage;

        ParticleSystem fxCircle;
        ParticleSystem fxLightning;

        Coroutine attack;
        public SphereCollider col;
        public GameObject lightning;
        Vector3 startScale;

        public void Awake()
        {
            fxCircle = GetComponent<ParticleSystem>();
            col = GetComponent<SphereCollider>();
            fxLightning = lightning.GetComponent<ParticleSystem>();
            startScale = transform.localScale;
        }

        public void Init(Vector3 position, int damage, int count, string tag)
        {
            transform.DOKill();
            transform.localScale = Vector3.zero;

            this.targetTag = tag;
            this.damage = damage;

            if (attack != null)
            {
                StopCoroutine(attack);
                lightning.SetActive(false);
                col.enabled = false;
                attack = null;
            }

            transform.position = position;

            if (!gameObject.activeSelf) gameObject.SetActive(true);

            transform.DOScale(startScale, 0.15f).OnComplete(delegate
            {
                attack = StartCoroutine(Damage(damage, count));
            }).SetEase(Ease.Linear);

            fxCircle.Play();
        }

        IEnumerator Damage(int damage, int count)
        {
            lightning.SetActive(true);
            col.enabled = true;
            fxLightning.Play();

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            col.enabled = false;
             
            yield return new WaitForSeconds(0.5f);

            transform.DOScale(0, 0.15f).SetEase(Ease.Linear);

            count--;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                if (targetTag == "Player")
                {
                    Player player = LevelController.instance.GetPlayer(other.gameObject);
                    if (player != null)
                    {
                        player.SubtractHp(damage, transform);
                    }
                }
                else if (targetTag == "Bot")
                {
                    Bot bot = LevelController.instance.GetBot(other.gameObject);
                    if (bot != null)
                    {
                        bot.SubtractHp(damage, transform, false);
                    }
                }
            }
        }
    }
}
