using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HieuBon
{
    public class BulletSlough : Bullet
    {
        public GameObject slough;

        private Vector3 lastPosition;

        int count;
        int countBounce;

        public override void Awake()
        {
            rb = GetComponent<Rigidbody>();
            mesh = transform.GetChild(2).GetComponent<MeshRenderer>();
            col = GetComponentInChildren<SphereCollider>();
        }

        public override void Init(int damage, string tag, Vector3 startPosition, Vector3 lookAt, Vector3 targetPosition, float jumpPower, float duration)
        {
            gameObject.SetActive(true);

            this.damage = damage;
            this.targetTag = tag;

            transform.position = startPosition;
            transform.LookAt(lookAt);

            slough.SetActive(false);
            mesh.gameObject.SetActive(true);

            transform.DOJump(targetPosition, jumpPower, 1, duration).SetEase(Ease.Linear).OnComplete(delegate
            {
                Disable();

                DOVirtual.DelayedCall(3f, delegate
                {
                    slough.SetActive(false);
                });
            });
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                StopPoison();
                StartPoison(5);
            }
        }

        public override void Disable()
        {
            slough.SetActive(true);
            mesh.gameObject.SetActive(false);
        }

        void Update()
        {
            if (!mesh.gameObject.activeSelf) return;
            Vector3 dir = transform.position - lastPosition;
            transform.rotation = Quaternion.LookRotation(dir);
            lastPosition = transform.position;
        }
    }
}
