using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
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

        public virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<SphereCollider>();
            mesh = GetComponentInChildren<MeshRenderer>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        public virtual void Init(int damage, string tag, float timeHide, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt)
        {
            Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
        }
        
        public virtual void Init(int damage, string tag, int countBounce, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt)
        {
            Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
        }
        
        public virtual void Init(int damage, string tag, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt, float timeSpawn, Bullet[] bulletSpawns)
        {
            Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
        }

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

            trailRenderer.Clear();

            mesh.gameObject.SetActive(true);
            col.enabled = true;

            if(!gameObject.activeSelf) gameObject.SetActive(true);
        }

        public void Disable()
        {
            col.enabled = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            mesh.gameObject.SetActive(false);
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                Disable();

                if (targetTag == "Player")
                {
                    Debug.Log(col.enabled);
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
    }
}