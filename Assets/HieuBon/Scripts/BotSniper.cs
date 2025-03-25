using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class BotSniper : BotSentry
    {
        public int amountBullet;
        public GameObject preBullet;
        public BulletChase[] bullets;
        public int indexBullet;
        public float timeDelay;
        public SpriteRenderer healthBar1;
        public SpriteRenderer healthBar2;

        public override void Start()
        {
            bullets = new BulletChase[amountBullet];
            for (int i = 0; i < amountBullet; i++)
            {
                GameObject b = Instantiate(preBullet, LevelController.instance.pool);
                bullets[i] = b.GetComponent<BulletChase>();
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
            bullets[indexBullet].transform.position = weapon.startBullet.transform.position;
            bullets[indexBullet].rb.velocity = dir * bulletSpeed;
            bullets[indexBullet].transform.LookAt(bullets[indexBullet].transform.position + dir);
            bullets[indexBullet].trailRenderer.Clear();
            bullets[indexBullet].gameObject.SetActive(true);
            indexBullet++;
            if (indexBullet == bullets.Length) indexBullet = 0;
            yield return new WaitForSeconds(rateOfFire);
            StopAttack();
        }       

        public override void SubtractHp(int hp, Transform killer, bool isOnlyBurn)
        {
            if (this.hp <= 0) return;

            base.SubtractHp(hp, killer, isOnlyBurn);

            PlayBlood();
            StopProbe();
            StopAttack();
            if (healthBar2.color != Color.black)
            {
                healthBar2.color = Color.black;
                StartDodging(killer);
            }
            else
            {
                healthBar1.enabled = false;
                healthBar2.enabled = false;
                StopDodging();
            }
        }

        public override void InitBot()
        {
            base.InitBot();
            healthBar1.enabled = true;
            healthBar2.enabled = true;
            healthBar2.color = Color.white;
            //StopDodging();
        }
    }
}
