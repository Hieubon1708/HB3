using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public class Boss1 : Boss
    {
        int amountBulletChase = 3;
        int amountBulletGlider = 15;

        public GameObject preBulletChase;
        public GameObject preBulletGlider;

        [HideInInspector]
        public BulletChase[] bulletChases;

        [HideInInspector]
        public BulletGlider[] bulletGliders;

        [HideInInspector]
        public int indexBulletChase;
        [HideInInspector]
        public int indexBulletGlider;

        bool isChase;
        
        public void Start()
        {
            bulletChases = new BulletChase[amountBulletChase];
            for (int i = 0; i < amountBulletChase; i++)
            {
                GameObject b = Instantiate(preBulletChase, LevelController.instance.pool);
                bulletChases[i] = b.GetComponent<BulletChase>();
                b.SetActive(false);
            }
            bulletGliders = new BulletGlider[amountBulletGlider];
            for (int i = 0; i < amountBulletGlider; i++)
            {
                GameObject b = Instantiate(preBulletGlider, LevelController.instance.pool);
                bulletGliders[i] = b.GetComponent<BulletGlider>();
                b.SetActive(false);
            }
            transform.LookAt(PlayerController.instance.transform, Vector3.up);
        }

        public override IEnumerator Attack(GameObject poppy)
        {
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");

            yield return new WaitForSeconds(aiming);

            weapon.Attack(poppy.transform);

            AudioController.instance.PlaySoundNVibrate(name.Contains("Swat") ? AudioController.instance.ak47Gun : AudioController.instance.laserGun, 0);

            if (isChase)
            {
                Vector3 lookAt = new Vector3(poppy.transform.position.x, bulletChases[indexBulletChase].transform.position.y, poppy.transform.position.z);

                bulletChases[indexBulletChase].Init(damage, "Player", 4, 5, 200, startBullet.position, lookAt);

                indexBulletChase++;
                if (indexBulletChase == bulletChases.Length) indexBulletChase = 0;
            }
            else
            {
                float VisionAngle = 50f * Mathf.Deg2Rad;
                float Currentangle = -VisionAngle / 2;
                float angleIcrement = VisionAngle / (5 - 1);
                float Sine;
                float Cosine;

                for (int i = 0; i < 5; i++)
                {
                    Sine = Mathf.Sin(Currentangle);
                    Cosine = Mathf.Cos(Currentangle);

                    Vector3 dir = (transform.forward * Cosine) + (transform.right * Sine);

                    bulletGliders[indexBulletGlider].Init(damage, "Player", 5, 0, startBullet.position, startBullet.position + dir * 5);

                    indexBulletGlider++;
                    if (indexBulletGlider == bulletGliders.Length) indexBulletGlider = 0;

                    Currentangle += angleIcrement;
                }
            }

            yield return new WaitForSeconds(2);

            StopAttack();
        }
    }
}
