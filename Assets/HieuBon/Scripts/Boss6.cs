using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public class Boss6 : Boss
    {
        int amountLaze = 5;

        public GameObject preLaze;

        public Transform pool;

        [HideInInspector]
        public BulletLaze[] bulletLazes;

        [HideInInspector]
        public int indexBulletLaze = 1;

        bool isAttacking;

        int amountShotTime;

        public void Start()
        {
            bulletLazes = new BulletLaze[amountLaze];
            for (int i = 0; i < amountLaze; i++)
            {
                GameObject b = Instantiate(preLaze, pool);
                bulletLazes[i] = b.GetComponent<BulletLaze>();
                b.SetActive(false);
            }
            transform.LookAt(PlayerController.instance.transform, Vector3.up);
        }

        public override void FixedUpdate()
        {
            if (!col.enabled || !navMeshAgent.enabled) return;
            if (radarView.target != null)
            {
                if (!navMeshAgent.isStopped)
                {
                    StopProbe();
                    radarView.SetColor(true);
                    navMeshAgent.isStopped = true;
                    animator.SetBool("Walking", false);
                }
                //transform.LookAt(radarView.target.transform.position);

                Vector3 targetDirection = radarView.target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3.5f);
                float angle = Quaternion.Angle(transform.rotation, targetRotation);
                if (angle < 5)
                {
                    if (attack == null)
                    {
                        StartAttack(radarView.target);
                    }
                }
            }
            else
            {
                if (isAttacking) return;
                if (!isKilling && navMeshAgent.isStopped)
                {
                    //BridgeController.instance.Debug_Log("Start");
                    StopAttack();
                    StartProbe(index);
                    radarView.SetColor(false);
                    navMeshAgent.isStopped = false;
                    animator.SetBool("Walking", pathInfo.isUpdatePosition);
                }
            }
        }

        public override IEnumerator Attack(GameObject poppy)
        {
            isAttacking = true;

            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");

            yield return new WaitForSeconds(aiming);

            if (amountShotTime < 5)
            {
                BulletLaze bulletLaze = bulletLazes[indexBulletLaze];

                weapon.Attack(poppy.transform);

                AudioController.instance.PlaySoundNVibrate(name.Contains("Swat") ? AudioController.instance.ak47Gun : AudioController.instance.laserGun, 0);

                bulletLaze.Init(poppy.transform.position - transform.position, transform);

                indexBulletLaze++;
                if (indexBulletLaze == bulletLazes.Length) indexBulletLaze = 0;


                yield return new WaitForSeconds(2);

                bulletLaze.gameObject.SetActive(false);
            }
            else
            {
                float VisionAngle = 360f * Mathf.Deg2Rad;
                float Currentangle = -VisionAngle / 2;
                float angleIcrement = VisionAngle / (bulletLazes.Length - 1);
                float Sine;
                float Cosine;

                foreach (var laze in bulletLazes)
                {
                    Sine = Mathf.Sin(Currentangle);
                    Cosine = Mathf.Cos(Currentangle);

                    Vector3 dir = (transform.forward * -Cosine) + (transform.right * -Sine);

                    laze.Init(dir, transform);

                    Currentangle += angleIcrement;
                }

                yield return new WaitForSeconds(12);

                foreach (var laze in bulletLazes)
                {
                    laze.gameObject.SetActive(false);
                }
            }

            StopAttack();

            amountShotTime++;

            if (amountShotTime > 5) amountShotTime = 1;

            isAttacking = false;
        }

        public void Update()
        {
            if (amountShotTime == 5)
            {
                pool.Rotate(Vector3.up * Time.deltaTime * 50);
            }
        }
    }
}
