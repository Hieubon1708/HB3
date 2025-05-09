using System.Collections;
using System.Drawing;
using UnityEngine;

namespace HieuBon
{
    public class BotSniper : BotSentry
    {
        public int amountBullet;
        public GameObject preBullet;
        public BulletStraight[] bullets;
        public int indexBullet;

        public override void Start()
        {
            bullets = new BulletStraight[amountBullet];
            for (int i = 0; i < amountBullet; i++)
            {
                GameObject b = Instantiate(preBullet, LevelController.instance.pool);
                bullets[i] = b.GetComponent<BulletStraight>();
                b.SetActive(false);
            }
            base.Start();
        }
       
        public override IEnumerator Attack(GameObject target)
        {
            animator.SetTrigger("Aiming");

            yield return new WaitForSeconds(aiming * 3);

            animator.SetTrigger("Fire");

            AudioController.instance.PlaySoundNVibrate(AudioController.instance.shotGun, 0);

            weapon.Attack(target.transform);

            bullets[indexBullet].gameObject.SetActive(false);
            bullets[indexBullet].rb.velocity = Vector3.zero;

            Vector3 dir = (target.transform.position - transform.position).normalized;

            bullets[indexBullet].Init(damage, "Player", bulletSpeed, 0, weapon.startBullet.transform.position, weapon.startBullet.transform.position + dir * 5);

            indexBullet++;

            if (indexBullet == bullets.Length) indexBullet = 0;

            yield return new WaitForSeconds(rateOfFire);

            StopAttack();
        }       
    }
}
