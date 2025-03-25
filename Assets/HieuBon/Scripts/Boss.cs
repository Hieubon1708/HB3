using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public abstract class Boss : Bot
    {
        [HideInInspector]
        public GameObject arrow;

        [HideInInspector]
        public Health health;

        public Transform startBullet;

        public override void Awake()
        {
            base.Awake();

            health = GetComponentInChildren<Health>();
            arrow = transform.Find("EnemyArrow").gameObject;
        }

        public void FixedUpdate()
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

        public override void SubtractHp(int hp, Transform killer, bool isOnlyBurn)
        {
            if (this.hp <= 0 || PlayerController.instance.player.hp <= 0) return;

            PlayBlood();
            StopProbe();
            StopAttack();

            health.SubtractHp();
            if (this.hp <= 0)
            {
                Debug.Log("ASdad");

                LevelController.instance.StopProbes();
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDie, 0);
                UIInGame.instance.camAni.Play("CamBossZoom");
                PlayerController.instance.playerTouchMovement.HandleLoseFinger();
                UIInGame.instance.layerCover.raycastTarget = true;
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
            else
            {
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDamage, 0);
            }
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
