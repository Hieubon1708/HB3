using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
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
            mesh = GetComponent<MeshRenderer>();
            col = GetComponentInChildren<SphereCollider>();
        }

        public override void Init(int damage, string tag, Vector3 startPosition, Vector3 lookAt, Vector3 targetPosition, float jumpPower, float duration)
        {
            this.damage = damage;
            this.targetTag = tag;

            transform.position = startPosition;
            transform.LookAt(lookAt);

            slough.SetActive(false);
            mesh.enabled = true;

            transform.DOJump(targetPosition, jumpPower, 1, duration);
        }

        public override void Disable()
        {
            mesh.enabled = false;
            slough.SetActive(true);
        }

        void Update()
        {
            if (!mesh.enabled) return;
            Vector3 dir = transform.position - lastPosition;
            transform.rotation = Quaternion.LookRotation(dir);
            lastPosition = transform.position;
        }
    }
}
