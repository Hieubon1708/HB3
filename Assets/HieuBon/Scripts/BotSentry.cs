using ACEPlay.Bridge;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public abstract class BotSentry : Bot
    {
        [HideInInspector]
        public ListeningDistance listeningDistance;
        [HideInInspector]
        public QuestionRotate questionRotate;
        [HideInInspector]
        public BotHealth health;

        protected Coroutine lostTrack;
        protected Coroutine lastTrace;
        protected Coroutine hear;
        Coroutine dodging;

        [HideInInspector]
        public bool isFind;
        protected float timeOff;
        public float targetTimeOff;

        [HideInInspector]
        public GameObject target;

        List<GameObject> allies = new List<GameObject>();

        [HideInInspector]
        public LayerMask rayLayer;

        ParticleSystem fxStun;

        public override void Awake()
        {
            listeningDistance = GetComponentInChildren<ListeningDistance>();
            questionRotate = GetComponentInChildren<QuestionRotate>();
            health = GetComponentInChildren<BotHealth>();

            base.Awake();
        }

        public virtual void Start()
        {
            rayLayer = LayerMask.GetMask("Wall", "Player");
        }

        public void StopLostTrack()
        {
            if (lostTrack != null)
            {
                StopCoroutine(lostTrack);
                lostTrack = null;
            }
        }

        public void StopLastTrace()
        {
            if (lastTrace != null)
            {
                StopCoroutine(lastTrace);
                lastTrace = null;
            }
        }

        public void StopHear()
        {
            if (hear != null)
            {
                StopCoroutine(hear);
                hear = null;
            }
        }

        public void StartDodging(Transform killer)
        {
            if (dodging == null)
            {
                dodging = StartCoroutine(Dodging(killer));
            }
        }

        public void StopDodging()
        {
            if (dodging != null)
            {
                StopCoroutine(dodging);
                dodging = null;
            }
        }


        public void StartHear(GameObject target)
        {
            if (hear == null) hear = StartCoroutine(Hear(target));
        }

        public void StartLostTrack(GameObject target)
        {
            if (lostTrack == null) lostTrack = StartCoroutine(LostTrack(target));
        }

        public void StartLastTrace(GameObject target)
        {
            if (lastTrace == null) lastTrace = StartCoroutine(LastTrace(target));
        }

        public void FixedUpdate()
        {
            if (!col.enabled || (fxStun != null && fxStun.isPlaying)) return;
            if (radarView.target != null)
            {
                timeOff = 0;
                if (!isFind)
                {
                    isFind = true;
                    ChangeSpeed(detectSpeed, rotateDetectSpeed);
                }
                if (isDodging) return;
                questionRotate.Hide();
                StopProbe();
                StopHear();
                StopLastTrace();
                StopLostTrack();
                radarView.SetColor(true);
                navMeshAgent.isStopped = true;
                animator.SetBool("Walking", false);
                target = radarView.target;

                Vector3 targetDirection = target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5.5f);
                float angle = Quaternion.Angle(transform.rotation, targetRotation);
                if (angle < 5)
                {
                    if (attack == null)
                    {
                        StartAttack(target);
                    }
                }
            }
            else
            {
                if (isFind)
                {
                    questionRotate.Show();
                    if (attack != null)
                    {
                        StopAttack();
                    }
                    radarView.SetColor(false);
                    timeOff += Time.fixedDeltaTime;
                    if (timeOff < targetTimeOff) return;
                    RaycastHit hit;
                    Vector3 from = transform.position;
                    Vector3 to = target == null ? PlayerController.instance.player.transform.position : target.transform.position;
                    Vector3 dir = (to - from).normalized;
                    Physics.Raycast(from, dir, out hit, radarView.VisionRange + distanceFinding, rayLayer);
                    if (hit.collider != null && hit.collider.tag == "Player")
                    {
                        if (target == null) target = hit.collider.gameObject;
                        Vector3 targetDirection = target.transform.position - transform.position;
                        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5.5f);
                        StopHear();
                        StopLastTrace();
                        StopLostTrack();
                        navMeshAgent.isStopped = false;
                        animator.SetBool("Walking", true);
                        navMeshAgent.destination = hit.transform.position;
                        BridgeController.instance.Debug_LogError("Đã thấy " + hit.collider.name, false);

                    }
                    else if (target != null)
                    {
                        navMeshAgent.stoppingDistance = 0;
                        StartLastTrace(target);
                    }
                    else
                    {
                        BridgeController.instance.Debug_Log("Đang tìmmm");
                    }
                    if (LevelController.instance.players.Count == 0) isFind = false;
                }
            }
        }

        public IEnumerator Hear(GameObject target)
        {
            BridgeController.instance.Debug_Log("Nghe thấy đồng mình ngỏm");
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDetect, 0);
            ChangeSpeed(detectSpeed, rotateDetectSpeed);
            StopProbe();
            StopLastTrace();
            StopLostTrack();
            navMeshAgent.stoppingDistance = Random.Range(1f, 2f);
            isFind = true;
            questionRotate.Show();
            navMeshAgent.isStopped = true;
            animator.SetBool("Walking", false);
            yield return new WaitForSeconds(time);
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = target.transform.position;
            animator.SetBool("Walking", true);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            float timeWalk = 0;
            while (col.enabled)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.2f) animator.SetBool("Walking", false);
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.1f || timeWalk >= 3f) break;
                yield return new WaitForFixedUpdate();
                timeWalk += Time.fixedDeltaTime;
            }
            StartLostTrack(target);
        }

        public IEnumerator LastTrace(GameObject target)
        {
            if (target == null) BridgeController.instance.Debug_Log("Player = null " + target.name);
            BridgeController.instance.Debug_Log("Điểm nhìn thấy cuối");
            navMeshAgent.isStopped = false;
            animator.SetBool("Walking", true);
            navMeshAgent.destination = target.transform.position;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            while (col.enabled)
            {
                if (navMeshAgent.remainingDistance <= 0.1f) animator.SetBool("Walking", false);
                if (navMeshAgent.remainingDistance == navMeshAgent.stoppingDistance) break;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(time);
            StartLostTrack(target);
        }

        public IEnumerator LostTrack(GameObject target)
        {
            // bỏ qua nghe thấy đồng mình chết và bỏ qua lần di chuyển cuối tới kẻ địch 
            StopHear();
            StopLastTrace();
            navMeshAgent.isStopped = true;
            animator.SetBool("Walking", false);
            if (target.CompareTag("Alert"))
            {
                ChangeSpeed(detectSpeed, rotateDetectSpeed);
                if (LevelController.instance.alertType == GameController.AlertType.Camera)
                {
                    BridgeController.instance.Debug_Log("Camera báo động");
                    AlertCamera alertCamera = null;
                    navMeshAgent.destination = target.transform.position;
                    yield return new WaitForFixedUpdate();
                    yield return new WaitForFixedUpdate();
                    yield return new WaitForFixedUpdate();
                    if (navMeshAgent.remainingDistance < 10)
                    {
                        navMeshAgent.stoppingDistance = Random.Range(1f, 3f);
                        alertCamera = LevelController.instance.GetCamera(target);
                    }
                    if (alertCamera != null)
                    {
                        target = alertCamera.spot.gameObject;
                    }
                    else
                    {
                        target = null;
                        BridgeController.instance.Debug_Log("Camera = null");
                    }
                }
                else if (LevelController.instance.alertType == GameController.AlertType.Laser)
                {
                    BridgeController.instance.Debug_Log("Laser báo động");
                    Laser laser = null;
                    navMeshAgent.destination = target.transform.position;
                    yield return new WaitForFixedUpdate();
                    yield return new WaitForFixedUpdate();
                    yield return new WaitForFixedUpdate();
                    if (navMeshAgent.remainingDistance < 10)
                    {
                        navMeshAgent.stoppingDistance = Random.Range(1f, 3f);
                        laser = LevelController.instance.GetLaser(target);
                    }
                    if (laser != null)
                    {
                        target = laser.cols[0].gameObject;
                    }
                    else
                    {
                        target = null;
                        BridgeController.instance.Debug_Log("Laser = null");
                    }
                }
                isFind = true;
                questionRotate.Show();
                yield return new WaitForSeconds(time);
                if (target != null)
                {
                    BridgeController.instance.Debug_Log("Di chuyển tới điểm báo động");
                    navMeshAgent.isStopped = false;
                    animator.SetBool("Walking", true);
                    navMeshAgent.destination = target.transform.position;
                    yield return new WaitForFixedUpdate();
                    yield return new WaitForFixedUpdate();
                    yield return new WaitForFixedUpdate();
                    float time = 0;
                    while (col.enabled)
                    {
                        //BridgeController.instance.Debug_LogError(navMeshAgent.remainingDistance);
                        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance - 0.2f) animator.SetBool("Walking", false);
                        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance - 0.1f || time >= 3f) break;
                        yield return new WaitForFixedUpdate();
                        time += Time.fixedDeltaTime;
                    }
                    BridgeController.instance.Debug_Log(" độngasdasdasdasdasdasd");
                    navMeshAgent.isStopped = true;
                    animator.SetBool("Walking", false);
                    yield return new WaitForSeconds(time);
                }
            }
            else yield return new WaitForSeconds(time);
            ChangeSpeed(detectSpeed, rotateDetectSpeed);
            navMeshAgent.isStopped = false;
            animator.SetBool("Walking", true);
            int step = Random.Range(2, 4);
            BridgeController.instance.Debug_Log("Dò xung quanh");
            navMeshAgent.stoppingDistance = 0;
            while (step > 0 || LevelController.instance.isAlert)
            {
                while (col.enabled)
                {
                    List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                    int indexRandom = Random.Range(0, list.Count);
                    float angle = list[indexRandom] * 45f;
                    Vector3 randomDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                    float distance = Random.Range(3f, 8f);
                    //Debug.DrawLine(transform.position, transform.position + randomDirection * distance, Color.red, 111);
                    navMeshAgent.destination = transform.position + randomDirection * distance;
                    yield return new WaitForFixedUpdate();
                    yield return new WaitForFixedUpdate();
                    yield return new WaitForFixedUpdate();
                    if (navMeshAgent.remainingDistance <= 10) break;
                    else list.RemoveAt(indexRandom);
                }
                animator.SetBool("Walking", true);
                //BridgeController.instance.Debug_LogError("Position = " + randomDestination + " IsUpdatePosition = " + navMeshAgent.updatePosition + " Step = " + step);
                while (col.enabled)
                {
                    if (navMeshAgent.remainingDistance <= 0.1f) animator.SetBool("Walking", false);
                    if (navMeshAgent.remainingDistance == navMeshAgent.stoppingDistance) break;
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(time);
                step--;
            }
            BridgeController.instance.Debug_Log("Về vị trí ban đầu");
            isFind = false;
            questionRotate.Hide();
            navMeshAgent.destination = pathInfo.paths[0][0];
            animator.SetBool("Walking", true);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            while (col.enabled)
            {
                if (navMeshAgent.remainingDistance <= 0.1f) animator.SetBool("Walking", false);
                if (navMeshAgent.remainingDistance == navMeshAgent.stoppingDistance) break;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(time);
            StartProbe(1);
        }

        public IEnumerator Dodging(Transform killer)
        {
            isDodging = true;
            navMeshAgent.isStopped = false;
            radarView.SetColor(false);
            isFind = true;
            ChangeSpeed(detectSpeed, rotateDetectSpeed);
            Vector3 dirOfAttack = transform.position - killer.position;
            navMeshAgent.destination = transform.position + dirOfAttack * 2;
            ChangeSpeed(detectSpeed, rotateDetectSpeed);
            animator.SetBool("Walking", true);
            animator.SetTrigger("Dodging");
            yield return new WaitForSeconds(targetTimeOff);
            BridgeController.instance.Debug_Log("Dodging");
            navMeshAgent.isStopped = true;
            animator.SetBool("Walking", false);
            target = killer.gameObject;
            isDodging = false;
            StopDodging();
        }

        public override void SubtractHp(int hp, Transform killer, bool isOnlyBurn)
        {
            if (this.hp <= 0) return;

            base.SubtractHp(hp, killer, isOnlyBurn);

            if(!isOnlyBurn)
            {
                PlayBlood();
                StopProbe();
                StopAttack();
                StopHear();
                StopLastTrace();
                StopLostTrack();
            }

            health.SubtractHp();

            if (this.hp <= 0)
            {
                StopDodging();

                if(fxStun != null && fxStun.isPlaying) fxStun.Stop();

                health.gameObject.SetActive(false);

                AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDie, 50);
                UIInGame.instance.HitEffect();
                UIInGame.instance.virtualCam.StartShakeCam(7.5f);
                isFind = false;
                index = 0;
                col.enabled = false;
                animator.enabled = false;
                navMeshAgent.enabled = false;
                radarView.gameObject.SetActive(false);
                questionRotate.Hide();
                IsKinematic(false);
                StartCoroutine(Die());

                if (!isOnlyBurn)
                {
                    Vector3 dir = (transform.position - PlayerController.instance.player.transform.position).normalized;
                    for (int i = 0; i < rbs.Length; i++)
                    {
                        rbs[i].AddForce(new Vector3(dir.x, dir.y, dir.z) * PlayerController.instance.player.weapon.force, ForceMode.Impulse);
                    }
                }

                LevelController.instance.RemoveBot(gameObject);
                UIInGame.instance.gamePlay.UpdateRemainingEnemy();
            }
            else
            {
                if(!isOnlyBurn)
                {
                    AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDamage, 50);
                    StartDodging(killer);
                    if (PlayerController.instance.player.playerIndexes.Stun())
                    {
                        Stun();
                    }
                }
            }
        }

        void Stun()
        {
            if(fxStun == null)
            {
                fxStun = Instantiate(GameController.instance.preFxStun, transform).GetComponentInChildren<ParticleSystem>();
            }

            fxStun.Play();

            CancelInvoke(nameof(CancelStun));
            Invoke(nameof(CancelStun), 1.5f);
        }

        void CancelStun()
        {
            fxStun.Stop();
        }

        public override void InitBot()
        {
            isFind = false;
            radarView.SetColor(false);
            questionRotate.transform.localScale = Vector3.zero;
            hp = startHp;
            indexPath = 0;
            IsKinematic(true);
            col.enabled = true;
            animator.enabled = true;
            isKilling = false;
            radarView.gameObject.SetActive(true);
            transform.position = pathInfo.paths[0][0];
            navMeshAgent.enabled = true;
            navMeshAgent.isStopped = false;
            if (pathInfo.paths[0].Length == 1) transform.rotation = Quaternion.Euler(transform.eulerAngles.x, pathInfo.angle, transform.eulerAngles.z);
            else if (pathInfo.paths[0].Length > 1) transform.LookAt(pathInfo.paths[0][1], Vector3.up);
        }
    }
}
