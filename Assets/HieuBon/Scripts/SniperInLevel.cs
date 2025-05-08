using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace HieuBon
{
    public class SniperInLevel : MonoBehaviour
    {
        public Transform sniper;
        public Transform sniperCaseUpper;

        bool isLookAtSniper;
        public GameObject fx;

        public CinemachineVirtualCamera cinemachineCam;

        public Image aimButton;
        public GameObject aim;
        public Transform cam;

        bool isAiming;

        public Vector4 clampCam;

        public float targetXAimAngle;
        public float zoom;

        Tween delayCancelAim;
        Tween doZoom;

        Vector2 startAngle;

        float targetZoom;
        float startZoom;

        Vector2 startMouse;
        Vector2 startEuler;

        bool isDrag;
        bool isShot;
        bool isReload;
        bool isShotFirst;
        bool isEnd;

        public GameObject[] bullets;

        int amountBullet = 4;

        public GameObject healthAndRemainingEnemy;

        LayerMask targetLayer;

        public TextMeshProUGUI amountEnemy;

        [HideInInspector]
        public int currentEnemy;

        float tempWeaponForce;

        public PathInfo[] bots;

        public SniperBulletInLevel sniperBulletInLevel;

        public Transform startBullet;

        private void Awake()
        {
            startAngle = cam.localEulerAngles;

            targetLayer = LayerMask.GetMask("Character");

            currentEnemy = bots.Length;
            amountEnemy.text = "" + currentEnemy + "/" + bots.Length;

            UpdateBullet();
        }

        void UpdateBullet()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (i < amountBullet)
                {
                    bullets[i].SetActive(true);
                }
                else
                {
                    bullets[i].SetActive(false);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<SphereCollider>().enabled = false;

                cinemachineCam.m_Lens.FieldOfView = UIInGame.instance.virtualCam.cinemachineCam.m_Lens.FieldOfView;
                cinemachineCam.gameObject.SetActive(true);

                startZoom = cinemachineCam.m_Lens.FieldOfView;
                targetZoom = cinemachineCam.m_Lens.FieldOfView + zoom;

                tempWeaponForce = PlayerController.instance.player.weapon.force;

                PlayerController.instance.player.weapon.force = 15f;
                PlayerController.instance.uIReceiveMoney.gameObject.SetActive(false);
                PlayerController.instance.player.health.gameObject.SetActive(false);

                fx.SetActive(false);

                isLookAtSniper = true;

                PlayerController.instance.HideTouch();

                StartCoroutine(ComeIn());
            }
        }

        IEnumerator ComeIn()
        {
            PlayerController.instance.Destination = transform.position;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => PlayerController.instance.player.navMeshAgent.remainingDistance == PlayerController.instance.player.navMeshAgent.stoppingDistance);

            PlayerController.instance.player.animator.Play("Sniper_case_opening");

            isLookAtSniper = false;

            sniperCaseUpper.DOLocalRotate(new Vector3(0, 0, -45), 0.25f).SetEase(Ease.OutBack).SetDelay(1.5f).SetUpdate(true).OnComplete(delegate
            {
                PlayerController.instance.player.transform.DORotateQuaternion(Quaternion.Euler(PlayerController.instance.player.transform.eulerAngles.x, cinemachineCam.transform.eulerAngles.y, PlayerController.instance.player.transform.eulerAngles.z), 0.5f).SetDelay(1f).OnComplete(delegate
                {
                    aimButton.gameObject.SetActive(true);
                    aimButton.DOFade(0.75f, 0.25f);
                });
            });

            DOVirtual.DelayedCall(2.5f, delegate
            {
                sniper.SetParent(PlayerController.instance.player.hand);
                sniper.localPosition = new Vector3(-0.720000029f, 2.58999991f, 0.949999988f);
                sniper.localRotation = Quaternion.Euler(new Vector3(300.880157f, 23.0169392f, 47.6958351f));
                PlayerController.instance.player.weapon.gameObject.SetActive(false);
            }).SetUpdate(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                foreach (var path in bots)
                {
                    (path.bot as BotSentry).StopProbe();
                    (path.bot as BotSentry).navMeshAgent.isStopped = true;
                    (path.bot as BotSentry).animator.SetBool("Walking", false);
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {

            }
            if (Input.GetKeyDown(KeyCode.D))
            {

            }
            if (isEnd) return;
            if (isLookAtSniper)
            {
                Vector3 targetPosition = new Vector3(sniper.position.x, PlayerController.instance.player.transform.position.y, sniper.position.z);

                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - PlayerController.instance.transform.position);

                PlayerController.instance.transform.rotation = Quaternion.Lerp(
                    PlayerController.instance.transform.rotation,
                    targetRotation,
                    0.1f
                );
            }
            if (isAiming && !isReload)
            {
                if (!isShot)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!healthAndRemainingEnemy.activeSelf)
                        {
                            healthAndRemainingEnemy.SetActive(true);
                        }

                        delayCancelAim.Kill();

                        startMouse = Input.mousePosition;
                        startEuler = cam.localEulerAngles;

                        isDrag = true;
                    }

                    if (Input.GetMouseButtonUp(0))
                    {

                        if (!isShotFirst)
                        {
                            isShotFirst = true;

                            foreach (var path in bots)
                            {
                                (path.bot as BotSentry).StartRunAmok();
                            }
                        }

                        float timeDelay = 1.5f;

                        isShot = true;
                        isDrag = false;

                        amountBullet--;

                        UpdateBullet();

                        cam.DOLocalRotate(new Vector3(cam.localEulerAngles.x + 3.5f, cam.localEulerAngles.y, cam.localEulerAngles.z), 0.05f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
                        {
                            isShot = false;
                        });

                        RaycastHit hit;

                        Physics.Raycast(cam.position, cam.forward, out hit, 100, targetLayer);
                        Debug.DrawRay(cam.position, cam.forward * 100, Color.yellow, 100);
                        if (hit.collider != null)
                        {
                            Bot bot = LevelController.instance.GetBotByBone(hit.collider.gameObject);

                            if (bot != null)
                            {
                                currentEnemy--;
                                
                                if (currentEnemy == 0)
                                {
                                    PlayerController.instance.player.animator.Play("Sniper_aiming");

                                    bot.navMeshAgent.isStopped = true;
                                    bot.animator.SetBool("Walking", false);
                                    (bot as BotSentry).StopRunAmok();

                                    DOVirtual.DelayedCall(0.15f, delegate
                                    {
                                        Quaternion lookAt = Quaternion.LookRotation(hit.point - transform.position);

                                        sniper.localPosition = new Vector3(1.14999998f, 2.42000008f, -0.970000029f);
                                        sniper.rotation = lookAt;

                                        Action action1 = () =>
                                        {
                                            bot.SubtractHp(100, PlayerController.instance.player.transform, false, false);
                                            amountEnemy.text = "" + currentEnemy + "/" + bots.Length;
                                        };

                                        Action action2 = () =>
                                        {
                                            sniper.gameObject.SetActive(false);

                                            PlayerController.instance.uIReceiveMoney.gameObject.SetActive(true);
                                            PlayerController.instance.player.health.gameObject.SetActive(true);
                                            PlayerController.instance.ShowTouch();
                                            PlayerController.instance.player.weapon.gameObject.SetActive(true);

                                            PlayerController.instance.player.animator.Play("Run blend");
                                            PlayerController.instance.player.navMeshAgent.ResetPath();
                                            healthAndRemainingEnemy.SetActive(false);
                                        };

                                        sniperBulletInLevel.Shoot(startBullet.position, hit.point, lookAt, action1, action2);
                                    });

                                    AimUp(0);
                                    isEnd = true;
                                    cinemachineCam.gameObject.SetActive(false);                                    
                                }
                                else
                                {
                                    bot.SubtractHp(100, PlayerController.instance.player.transform, false, false);
                                    amountEnemy.text = "" + currentEnemy + "/" + bots.Length;
                                }
                            }
                            else
                            {
                                Debug.LogError("!");
                            }
                        }

                        if (amountBullet == 0)
                        {
                            timeDelay = 0.25f;
                            isReload = true;
                            amountBullet = 4;

                            PlayerController.instance.player.animator.Play("Sniper_recharge");

                            DOVirtual.DelayedCall(1f, delegate
                            {
                                isReload = false;
                                isShot = false;
                            });
                        }

                        AimUp(timeDelay);
                    }

                    if (isDrag)
                    {
                        Vector2 currentMouse = Input.mousePosition;

                        float x = Mathf.Clamp((startMouse.y - currentMouse.y) * 0.025f + startEuler.x, clampCam.x, clampCam.y);
                        float y = Mathf.Clamp((currentMouse.x - startMouse.x) * 0.025f + startEuler.y, clampCam.z, clampCam.w);

                        cam.localEulerAngles = new Vector3(x, y, cam.localEulerAngles.z);
                    }
                }
            }

            if (PlayerController.instance.player.hp <= 0)
            {
                AimUp(0);
                isEnd = true;
                healthAndRemainingEnemy.SetActive(false);
                cinemachineCam.Priority = 0;
            }
        }

        public void AimDown()
        {
            if (isAiming || isReload) return;

            cam.DOKill();
            doZoom.Kill();

            doZoom = DOVirtual.Float(cinemachineCam.m_Lens.FieldOfView, targetZoom, 0.5f, (v) =>
            {
                cinemachineCam.m_Lens.FieldOfView = v;
            });

            aim.SetActive(true);
            aimButton.gameObject.SetActive(false);
            isAiming = true;
        }

        public void AimUp(float timeDelay)
        {
            delayCancelAim = DOVirtual.DelayedCall(timeDelay, delegate
            {
                isAiming = false;

                cam.DOKill();
                doZoom.Kill();

                cam.DOLocalRotate(new Vector3(startAngle.x, startAngle.y, cam.localEulerAngles.z), 0.5f);

                doZoom = DOVirtual.Float(cinemachineCam.m_Lens.FieldOfView, startZoom, 0.5f, (v) =>
                {
                    cinemachineCam.m_Lens.FieldOfView = v;
                });

                aim.SetActive(false);
                aimButton.gameObject.SetActive(!isEnd);
            });
        }
    }
}
