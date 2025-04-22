using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
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
        }

        public override IEnumerator Attack(GameObject poppy)
        {
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");

            yield return new WaitForSeconds(aiming);

            weapon.Attack(poppy.transform);

            AudioController.instance.PlaySoundNVibrate(name.Contains("Swat") ? AudioController.instance.ak47Gun : AudioController.instance.laserGun, 0);

            Vector3 lookAt = new Vector3(poppy.transform.position.x, bulletBounces[indexBulletBounce].transform.position.y, poppy.transform.position.z);

            //bulletBounces[indexBulletBounce].Init(damage, "Player", 5, 0, startBullet.position, lookAt, 1, bulletBounces);

            indexBulletBounce++;
            if (indexBulletBounce == bulletBounces.Length) indexBulletBounce = 0;

            yield return new WaitForSeconds(2);

            StopAttack();
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {

            }
        }
    }
}
