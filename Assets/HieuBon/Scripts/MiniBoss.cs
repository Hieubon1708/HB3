using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class MiniBoss : BotSentry
    {
        public int amountBullet;
        public GameObject preBullet;
        public BulletChase[] bullets;
        public int indexBullet;
        Coroutine dodging;
        public Health health;

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

        void StartDodging(Transform killer)
        {
            if (dodging == null)
            {
                dodging = StartCoroutine(Dodging(killer));
            }
        }

        void StopDodging()
        {
            if (dodging != null)
            {
                StopCoroutine(dodging);
                dodging = null;
            }
        }

        public override IEnumerator Attack(GameObject target)
        {
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");
            while(target != null)
            {
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
            }
            StopAttack();
        }

        IEnumerator Dodging(Transform killer)
        {
            isDodging = true;
            isFind = true;
            StopAttack();
            navMeshAgent.isStopped = false;
            radarView.SetColor(false);
            ChangeSpeed(detectSpeed, rotateDetectSpeed);
            Vector3 dirOfAttack = (transform.position - killer.position).normalized;
            navMeshAgent.destination = transform.position + dirOfAttack * 2;
            animator.SetBool("Walking", true);
            animator.SetTrigger("Dodging");
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Walking", false);
            target = killer.gameObject;
            BridgeController.instance.Debug_Log("Dodging");
            isDodging = false;
        }

        public override void SubtractHp(int hp, Transform killer, bool isOnlyBurn)
        {
            if (this.hp <= 0) return;

            base.SubtractHp(hp, killer, isOnlyBurn);

            PlayBlood();
            StopDodging();
            StopProbe();
            if (this.hp == 0)
            {
                GameController.instance.map.GetComponentInChildren<EndDoor>().col.enabled = true;
                health.gameObject.SetActive(false);
            }
            else
            {
                StartDodging(killer);
            }
            health.SubtractHp();
        }
    }
}
