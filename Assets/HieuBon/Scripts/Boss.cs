using ACEPlay.Bridge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    public abstract class Boss : Bot
    {
        [HideInInspector]
        public GameObject arrow;

        [HideInInspector]
        public UIHealth health;

        public Transform startBullet;

        Coroutine run;

        public override void Awake()
        {
            base.Awake();

            health = GetComponentInChildren<UIHealth>(true);
            arrow = transform.Find("EnemyArrow").gameObject;
        }

        void StartRun(Transform killer)
        {
            if (run == null)
            {
                run = StartCoroutine(Run(killer));
            }
        }

        void StopRun()
        {
            if (run != null)
            {
                StopCoroutine(run);
                run = null;
            }
        }

        public virtual void FixedUpdate()
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

        public override void SubtractHp(int hp, Transform killer, bool isBurnOrPoison = false, bool isReceiveMoney = true)
        {
            if (this.hp <= 0 || PlayerController.instance.player.hp <= 0) return;
            base.SubtractHp(hp, killer, isBurnOrPoison, isReceiveMoney);

            if (!isBurnOrPoison)
            {
                PlayBlood();
                StopProbe();
                StopAttack();

                if(this.hp > 0)
                {
                    AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDamage, 0);
                    StopRun();
                    StartRun(killer);
                }
            }

            health.SubtractHp();
            if (this.hp <= 0)
            {
                StopRun();

                LevelController.instance.StopProbes();
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDie, 0);
                UIInGame.instance.camAni.Play("CamBossZoom");

                PlayerController.instance.HideTouch();

                UIInGame.instance.HitEffect();

                col.enabled = false;
                animator.enabled = false;
                navMeshAgent.enabled = false;
                radarView.gameObject.SetActive(false);
                arrow.SetActive(false);
                IsKinematic(false);
                UIInGame.instance.BossEnd();
                Vector3 dir = (transform.position - PlayerController.instance.transform.position).normalized;
                for (int i = 0; i < rbs.Length; i++)
                {
                    rbs[i].AddForce(new Vector3(dir.x, dir.y, dir.z) * 10, ForceMode.Impulse);
                }
                LevelController.instance.RemoveBot(gameObject);
                UIInGame.instance.gamePlay.UpdateRemainingEnemy();
            }
        }

        IEnumerator Run(Transform killer)
        {
            BridgeController.instance.Debug_Log("Run");

            col.enabled = false;

            navMeshAgent.isStopped = false;

            radarView.SetColor(false);

            ChangeSpeed(detectSpeed, rotateDetectSpeed);

            Vector3 dirOfAttack = transform.position - killer.position;

            navMeshAgent.destination = transform.position + dirOfAttack * 2;

            animator.SetTrigger("Dodging");
            animator.SetBool("Walking", true);

            yield return new WaitForSeconds(0.35f);

            index++;

            if (index == pathInfo.paths[indexPath].Length) index = 0;
            if (index == pathInfo.paths[indexPath].Length + 1) index = 1;

            navMeshAgent.destination = pathInfo.paths[indexPath][index];
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            while (hp > 0)
            {
                if (navMeshAgent.remainingDistance <= 0.1f) animator.SetBool("Walking", false);
                if (navMeshAgent.remainingDistance == navMeshAgent.stoppingDistance) break;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(time);

            index += 1;
            if (index == pathInfo.paths[indexPath].Length) index = 0;

            col.enabled = true;

            StartProbe(index);

        }

        public override void InitBot()
        {
            radarView.SetColor(false);
            arrow.SetActive(true);
            hp = startHp;
            IsKinematic(true);
            col.enabled = true;
            animator.enabled = true;
            transform.position = pathInfo.paths[0][0];
            navMeshAgent.enabled = true;
            navMeshAgent.isStopped = false;
            radarView.gameObject.SetActive(true);
            transform.LookAt(pathInfo.paths[0][1], Vector3.up);
        }
    }
}
