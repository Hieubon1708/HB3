using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    public class Bullet : MonoBehaviour
    {
        [HideInInspector]
        public string targetTag;
        [HideInInspector]
        public int damage;
        [HideInInspector]
        public TrailRenderer trailRenderer;

        [HideInInspector]
        public Rigidbody rb;
        protected SphereCollider col;
        protected MeshRenderer mesh;
        protected float speed;
        protected float angularSpeed;

        [HideInInspector]
        public bool isPoison;

        protected Coroutine poison;

        public virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<SphereCollider>();
            mesh = GetComponentInChildren<MeshRenderer>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        //bullet hide
        public virtual void Init(int damage, string tag, float timeHide, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt)
        {
            Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
        }

        //bullet bounce
        public virtual void Init(int damage, string tag, int countBounce, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt)
        {
            Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
        }

        //bullet spawn
        public virtual void Init(int damage, string tag, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt, float timeSpawn, Bullet[] bulletSpawns)
        {
            Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
        }

        //bullet slough
        public virtual void Init(int damage, string tag, Vector3 startPosition, Vector3 lookAt, Vector3 targetPosition, float jumpPower, float duration)
        {
            Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
        }

        //default
        public virtual void Init(int damage, string tag, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt)
        {
            this.damage = damage;
            this.targetTag = tag;
            this.speed = speed;
            this.angularSpeed = angularSpeed;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.position = startPosition;
            transform.LookAt(lookAt);

            if (trailRenderer != null) trailRenderer.Clear();

            mesh.gameObject.SetActive(true);
            col.enabled = true;

            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            col.enabled = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            mesh.gameObject.SetActive(false);

            if (isPoison)
            {
                StartPoison(5);
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                Disable();

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
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Disable();
            }
        }

        IEnumerator Poison(int damage)
        {
            float time = 5f;

            Player player = PlayerController.instance.player;

            while (player.hp > 0 && time > 0)
            {
                yield return new WaitForSeconds(0.25f);
                player.SubtractHp(damage, null);
                time -= 0.25f;
            }
        }

        public void StopPoison()
        {
            if (poison != null)
            {
                StopCoroutine(poison);
                poison = null;
            }
        }

        public void StartPoison(int damage)
        {
            if (poison == null) poison = StartCoroutine(Poison(damage));
        }
    }
}