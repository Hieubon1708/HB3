using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public class Boss5 : Boss
    {
        int amountBulletSpawn = 3;
        int amountBulletBounce = 10;

        public GameObject preBulletSpawn;
        public GameObject preBulletBounce;

        [HideInInspector]
        public BulletSpawn[] bulletSpawns;
        [HideInInspector]
        public BulletBounce[] bulletBounces;

        [HideInInspector]
        public int indexBulletSpawn;
        [HideInInspector]
        public int indexBulletBounce;

        public void Start()
        {
            bulletSpawns = new BulletSpawn[amountBulletSpawn];
            for (int i = 0; i < amountBulletSpawn; i++)
            {
                GameObject b = Instantiate(preBulletSpawn, LevelController.instance.pool);
                bulletSpawns[i] = b.GetComponent<BulletSpawn>();
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
        }

        public override IEnumerator Attack(GameObject poppy)
        {
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");

            yield return new WaitForSeconds(aiming);

            weapon.Attack(poppy.transform);

            AudioController.instance.PlaySoundNVibrate(name.Contains("Swat") ? AudioController.instance.ak47Gun : AudioController.instance.laserGun, 0);

            Vector3 lookAt = new Vector3(poppy.transform.position.x, bulletSpawns[indexBulletSpawn].transform.position.y, poppy.transform.position.z);

            bulletSpawns[indexBulletSpawn].Init(damage, "Player", 5, 0, startBullet.position, lookAt, 1, bulletBounces);

            indexBulletSpawn++;
            if (indexBulletSpawn == bulletSpawns.Length) indexBulletSpawn = 0;

            yield return new WaitForSeconds(2);

            StopAttack();
        }
    }
}
