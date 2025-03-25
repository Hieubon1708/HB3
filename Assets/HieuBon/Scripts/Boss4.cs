using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public class Boss4 : Boss
    {
        int amountBulletChase = 3;
        int amountBulletStraight = 24;

        public GameObject preBulletChase;
        public GameObject preBulletStraight;

        [HideInInspector]
        public BulletChase[] bulletChases;

        [HideInInspector]
        public BulletStraight[] bulletStraights;

        [HideInInspector]
        public int indexBulletChase;
        [HideInInspector]
        public int indexBulletStraight;

        int count;
        
        public void Start()
        {
            bulletChases = new BulletChase[amountBulletChase];
            for (int i = 0; i < amountBulletChase; i++)
            {
                GameObject b = Instantiate(preBulletChase, LevelController.instance.pool);
                bulletChases[i] = b.GetComponent<BulletChase>();
                b.SetActive(false);
            }
            bulletStraights = new BulletStraight[amountBulletStraight];
            for (int i = 0; i < amountBulletStraight; i++)
            {
                GameObject b = Instantiate(preBulletStraight, LevelController.instance.pool);
                bulletStraights[i] = b.GetComponent<BulletStraight>();
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

            if (count < 4)
            {
                Vector3 lookAt = new Vector3(poppy.transform.position.x, bulletChases[indexBulletChase].transform.position.y, poppy.transform.position.z);

                bulletChases[indexBulletChase].Init(damage, "Player", 4, 5, 200, startBullet.position, lookAt);

                indexBulletChase++;
                if (indexBulletChase == bulletChases.Length) indexBulletChase = 0;

                count++;
            }
            else
            {
                float VisionAngle = 360f * Mathf.Deg2Rad;
                float Currentangle = -VisionAngle / 2;
                float angleIcrement = VisionAngle / (12 - 1);
                float Sine;
                float Cosine;

                for (int i = 0; i < 12; i++)
                {
                    Sine = Mathf.Sin(Currentangle);
                    Cosine = Mathf.Cos(Currentangle);

                    Vector3 dir = (transform.forward * -Cosine) + (transform.right * -Sine);

                    bulletStraights[indexBulletStraight].Init(damage, "Player", 5, 0, startBullet.position, startBullet.position + dir * 5);

                    indexBulletStraight++;
                    if (indexBulletStraight == bulletStraights.Length) indexBulletStraight = 0;

                    Currentangle += angleIcrement;
                }

                count = 0;
            }

            yield return new WaitForSeconds(2);

            StopAttack();
        }
    }
}
