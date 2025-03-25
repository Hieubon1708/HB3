using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public class Boss3 : Boss
    {
        int amountBulletStraight = 16;

        public GameObject preBulletStraight;

        [HideInInspector]
        public BulletStraight[] bulletStraights;

        [HideInInspector]
        public int indexBulletStraight;

        public void Start()
        {
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

            yield return new WaitForSeconds(2);

            StopAttack();
        }
    }
}
