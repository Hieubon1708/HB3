using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class BulletBounce : Bullet
    {
        private Vector3 previousPosition;

        int count;
        int countBounce;

        public override void Init(int damage, string tag, int countBounce, float speed, float angularSpeed, Vector3 startPosition, Vector3 lookAt)
        {
            base.Init(damage, tag, speed, angularSpeed, startPosition, lookAt);
            this.countBounce = countBounce;

            count = 0;
        }

        void FixedUpdate()
        {
            if (!col.enabled) return;

            previousPosition = transform.position;

            rb.velocity = transform.forward * speed;
        }

        public override void OnTriggerEnter(Collider other)
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
                count++;

                Vector3 collisionDirection = transform.position - previousPosition;
                collisionDirection.Normalize();

                RaycastHit hit;
                if (Physics.Raycast(transform.position, collisionDirection, out hit, 5))
                {
                    Vector3 normal = hit.normal;
                    Vector3 reflectionDirection = Vector3.Reflect(collisionDirection, normal);

                    //Debug.Log("Hướng phản xạ (raycast): " + reflectionDirection);

                    rb.velocity = reflectionDirection * 5;

                    transform.LookAt(transform.position + rb.velocity);
                }
                else
                {
                    //Debug.Log("Raycast không chạm vào gì cả.");
                }
                if (count > 2) Disable();
            }
        }

        Vector3 CalculateReflection(Vector3 collisionDirection, Vector3 wallNormal)
        {
            return Vector3.Reflect(collisionDirection, wallNormal);
        }
    }
}
