using ACEPlay.Bridge;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public class Boss2 : Boss
    {
        int amoutLightning = 14;

        public GameObject preLightning;

        List<PathInfo> allies;

        [HideInInspector]
        public Lightning[] lightnings;

        [HideInInspector]
        public BulletGlider[] bulletGliders;

        [HideInInspector]
        public int indexLightning;

        float radius = 2f;

        public void Start()
        {
            lightnings = new Lightning[amoutLightning];
            for (int i = 0; i < amoutLightning; i++)
            {
                GameObject b = Instantiate(preLightning, LevelController.instance.pool);
                lightnings[i] = b.GetComponent<Lightning>();
                b.SetActive(false);
            }
            transform.LookAt(PlayerController.instance.transform, Vector3.up);

            allies = LevelController.instance.GetComponentsInChildren<PathInfo>(true).ToList();

            for (int i = 0; i < allies.Count; i++)
            {
                if (allies[i].gameObject.activeSelf)
                {
                    allies.RemoveAt(i);
                    i--;
                    continue;
                }
                allies[i].gameObject.SetActive(true);
            }
        }

        void GenerateAllies()
        {
            for (int i = 0; i < allies.Count; i++)
            {
                allies[i].Init();
            }
        }

        public override IEnumerator Attack(GameObject poppy)
        {
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");

            yield return new WaitForSeconds(aiming);

            weapon.Attack(poppy.transform);

            AudioController.instance.PlaySoundNVibrate(name.Contains("Swat") ? AudioController.instance.ak47Gun : AudioController.instance.laserGun, 0);

            Vector3 target = PlayerController.instance.player.transform.position;

            int i = 0;
            for (; i < 6; i++)
            {
                int index = i;
                Lightning lightning = lightnings[indexLightning];

                DOVirtual.DelayedCall(i * 0.15f, delegate
                {
                    lightning.Init(new Vector3(
                   target.x + radius * Mathf.Cos(2 * Mathf.PI * index / 6),
                   target.y,
                   target.z + radius * Mathf.Sin(2 * Mathf.PI * index / 6)), damage, 1, "Player");
                });

                indexLightning++;
                if (indexLightning == lightnings.Length) indexLightning = 0;
            }

            DOVirtual.DelayedCall(i * 0.15f, delegate
            {
                lightnings[indexLightning].Init(target, damage, 1, "Player");
            });

            indexLightning++;
            if (indexLightning == lightnings.Length) indexLightning = 0;

            yield return new WaitForSeconds(2);

            StopAttack();
        }
    }
}
