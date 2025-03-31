using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Hunter
{
    public class BulletLaze : MonoBehaviour
    {
        Transform parent;
        LayerMask mask;
        string targetTag;
        int damage;

        public void Init(int damage, string targetTag, Vector3 lookAt, Transform parent)
        {
            this.parent = parent;
            this.targetTag = targetTag;
            this.damage = damage;

            mask = LayerMask.GetMask(targetTag, "Wall");

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            transform.LookAt(transform.position + lookAt);
            transform.localScale = Vector3.zero;

            gameObject.SetActive(true);
            Debug.Log(gameObject.activeSelf);
        }

        public void Update()
        {
            RaycastHit hit;

            Physics.Raycast(parent.position, transform.forward, out hit, 10, mask);
            Debug.DrawRay(parent.position, transform.forward * 5, Color.yellow);

            if (hit.collider != null)
            {
                float distance = Vector3.Distance(parent.position, hit.point);
                transform.localScale = new Vector3(1, 1, distance + 1);
                transform.position = parent.position + transform.forward * ((distance + 1) / 2);
            }
        }

        public void OnTriggerEnter(Collider other)
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
