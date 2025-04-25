using ACEPlay.Bridge;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace HieuBon
{
    public class Boss7 : Boss
    {
        int amountSlough = 3;
        int amountBulletBounce = 3;

        public GameObject preSlough;
        public GameObject preBulletBounce;

        [HideInInspector]
        public BulletSlough[] bulletSloughs;
        [HideInInspector]
        public BulletBounce[] bulletBounces;

        [HideInInspector]
        public int indexBulletSlough;
        [HideInInspector]
        public int indexBulletBounce;

        bool isSlough;

        public void Start()
        {
            bulletSloughs = new BulletSlough[amountSlough];
            for (int i = 0; i < amountSlough; i++)
            {
                GameObject b = Instantiate(preSlough, LevelController.instance.pool);
                bulletSloughs[i] = b.GetComponent<BulletSlough>();
                b.SetActive(false);
            }
            bulletBounces = new BulletBounce[amountBulletBounce];
            for (int i = 0; i < amountBulletBounce; i++)
            {
                GameObject b = Instantiate(preBulletBounce, LevelController.instance.pool);
                bulletBounces[i] = b.GetComponent<BulletBounce>();
                b.SetActive(false);
            }
            transform.LookAt(PlayerController.instance.transform, Vector3.up);

            DOVirtual.DelayedCall(10f, null).OnStepComplete(delegate
            {
                isSlough = !isSlough;
            });
        }

        public override IEnumerator Attack(GameObject poppy)
        {
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");

            yield return new WaitForSeconds(aiming);

            weapon.Attack(poppy.transform);

            AudioController.instance.PlaySoundNVibrate(name.Contains("Swat") ? AudioController.instance.ak47Gun : AudioController.instance.laserGun, 0);

            Vector3 lookAt = new Vector3(poppy.transform.position.x, bulletSloughs[indexBulletSlough].transform.position.y, poppy.transform.position.z);

            if (!isSlough)
            {
                bulletSloughs[indexBulletSlough].Init(damage, "Player", startBullet.position, lookAt, poppy.transform.position, 5, 1f);

                indexBulletSlough++;
                if (indexBulletSlough == bulletSloughs.Length) indexBulletSlough = 0;
            }
            else
            {
                bulletBounces[indexBulletBounce].Init(damage, "Player", 2, 5, 0, transform.position, transform.position + transform.forward * 5);

                indexBulletBounce++;
                if (indexBulletBounce == bulletBounces.Length) indexBulletBounce = 0;
            }

            yield return new WaitForSeconds(1.5f); 

            StopAttack();
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(Attack(PlayerController.instance.gameObject));
            }
        }
    }
}
