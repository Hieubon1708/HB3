using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public abstract class Bot : MonoBehaviour
    {
        public int startHp;

        [HideInInspector]
        public int hp;
        public int damage;
        public float delayShot;
        protected int indexPath;

        [HideInInspector]
        public RadarView radarView;
        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [HideInInspector]
        public PathInfo pathInfo;

        [HideInInspector]
        public ParticleSystem blood;
        [HideInInspector]
        public CapsuleCollider col;
        public Transform hips;
        [HideInInspector]
        public Rigidbody[] rbs;
        [HideInInspector]
        public GameObject scream;

        public float time;
        public float speed;
        public float rotateSpeed;
        public float detectSpeed;
        public float rotateDetectSpeed;
        public float rateOfFire;

        [HideInInspector]
        public float distanceFinding;
        public float bulletSpeed;
        public float aiming = 0.15f;

        [HideInInspector]
        public bool isKilling;
        [HideInInspector]
        public bool isDodging;

        protected Coroutine probe;
        protected Coroutine attack;
        protected Coroutine poison;
        protected Coroutine burn;

        [HideInInspector]
        public Transform aimTarget;

        [HideInInspector]
        public BotWeapon weapon;
        TextDamage textDamage;

        ParticleSystem fxPoison;
        ParticleSystem fxBurn;

        public virtual void Awake()
        {
            radarView = GetComponentInChildren<RadarView>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            pathInfo = GetComponentInChildren<PathInfo>();
            weapon = GetComponentInChildren<BotWeapon>();
            textDamage = GetComponentInChildren<TextDamage>();
            col = GetComponent<CapsuleCollider>();

            blood = transform.Find("Blood").GetComponent<ParticleSystem>();

            Transform tranScream = transform.Find("Scream");
            if (tranScream) scream = tranScream.gameObject;
        }

        public void Init(PathInfo pathInfo)
        {
            //damage = (int)(damage * (1 + 0.03f * (Manager.instance.Chapter - 1)));
            this.pathInfo = pathInfo;
            UpgradeEnemy.MoveSpeed(ref speed);
            UpgradeEnemy.MoveSpeedDetect(ref detectSpeed);
            UpgradeEnemy.RotateSpeed(ref rotateSpeed);
            UpgradeEnemy.RotateSpeedDetect(ref rotateDetectSpeed);
            UpgradeEnemy.DistanceFinding(ref distanceFinding);
            UpgradeEnemy.TimeDelayPoint(ref time);
            UpgradeEnemy.RateOfFire(ref rateOfFire);
            UpgradeEnemy.BulletSpeed(ref bulletSpeed);
        }

        public void StopProbe()
        {
            if (probe != null)
            {
                StopCoroutine(probe);
                probe = null;
            }
        }

        public void StartProbe(int currentIndex)
        {
            if (probe == null) probe = StartCoroutine(Probe(currentIndex));
        }

        public void StopAttack()
        {
            if (attack != null)
            {
                isKilling = false;
                animator.SetTrigger("NoAiming");
                StopCoroutine(attack);
                attack = null;
            }
        }

        public void StopPoison()
        {
            if (poison != null)
            {
                StopCoroutine(poison);
                poison = null;
            }
        }

        public void StartPoison(int damage)
        {
            if (poison == null) poison = StartCoroutine(Poison(damage));
        }

        public void StopBurn()
        {
            if (burn != null)
            {
                StopCoroutine(burn);
                burn = null;
            }
        }

        public void StartBurn(int damage)
        {
            if (burn == null) burn = StartCoroutine(Burn(damage));
        }

        public void StartAttack(GameObject target)
        {
            if (isKilling || attack != null) return;
            animator.ResetTrigger("NoAiming");
            animator.ResetTrigger("Fire");
            attack = StartCoroutine(Attack(target));
            isKilling = true;
        }

        public void ChangeSpeed(float speed, float rotateSpeed)
        {
            navMeshAgent.speed = speed;
        }

        public virtual void SubtractHp(int hp, Transform killer, bool isOnlyBurn)
        {
            GameController.TextDamageType textDamageType = GameController.TextDamageType.None;

            if (!isOnlyBurn)
            {
                PlayerController.instance.player.playerIndexes.DamageCrit(ref hp, ref textDamageType);

                int damagePoison = 0;
                int damageBurn = 0;

                PlayerController.instance.player.playerIndexes.Poison(ref damagePoison, startHp);
                PlayerController.instance.player.playerIndexes.Burn(ref damageBurn, startHp);

                if (damageBurn != 0)
                {
                    StopBurn();
                    StartBurn(damageBurn);
                }

                if (damagePoison != 0)
                {
                    StopPoison();
                    StartPoison(damagePoison);
                }
            }

            this.hp = Mathf.Clamp(this.hp - hp, 0, this.hp);

            if (textDamageType == GameController.TextDamageType.Crit)
            {
                textDamage.ShowDamageCrit(hp);
            }
            else
            {
                textDamage.ShowDamage(hp);
            }

            if (this.hp <= 0 && LevelController.instance.players.Count > 0)
            {
                //EventManager.SetDataGroup(EventVariables.UpdateMission, this as BossBot ? MissionType.KillBoss : MissionType.KillEnemy, 1);
                //EventManager.EmitEvent(EventVariables.UpdateMission);

                int coin = 0;

                if (this as BotNormal || this as BotDemolition)
                {
                    coin = Random.Range(1, 3);
                    BridgeController.instance.Debug_Log("Enemy_1 " + coin);
                }
                else if (this as BotSniper)
                {
                    coin = Random.Range(1, 4);
                    BridgeController.instance.Debug_Log("Enemy_2 " + coin);
                }
                else if (this as Boss)
                {
                    coin = Random.Range(4, 9);
                    BridgeController.instance.Debug_Log("Boss " + coin);
                }
                else if (this as MiniBoss)
                {
                    coin = Random.Range(3, 6);
                    BridgeController.instance.Debug_Log("MiniBoss " + coin);
                }
                else
                {
                    coin = 1;
                    BridgeController.instance.Debug_Log("!!! " + this);
                }
                Player player = LevelController.instance.players[0];

                if (player != null)
                {
                    PlayerController.instance.PlayDollars(player.gameObject, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), coin);
                }

                if (!(this as Boss))
                {
                    GameController.instance.ReceiveBlood(transform.position);
                    GameController.instance.ReceiveArmor(transform.position);
                }

                if (!isOnlyBurn) PlayerController.instance.player.playerIndexes.BloodSucking();

                if (fxBurn != null && fxBurn.isPlaying) fxBurn.Stop();
                if (fxPoison != null && fxPoison.isPlaying) fxPoison.Stop();
            }
        }

        [HideInInspector]
        public int index;

        IEnumerator Probe(int currentIndex)
        {
            if (pathInfo.paths[0].Length > 1)
            {
                ChangeSpeed(speed, rotateSpeed);
                index = currentIndex;
                if (pathInfo.isUpdatePosition)
                {
                    if (pathInfo.pathType == GameController.PathType.Circle)
                    {
                        while (col.enabled)
                        {
                            Vector3 direction = new Vector3(pathInfo.paths[indexPath][index].x, transform.position.y, pathInfo.paths[indexPath][index].z) - transform.position;
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
                            {
                                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pathInfo.botType == GameController.BotType.Normal ? rotateSpeed : pathInfo.rotateSpeedForBoss);
                                yield return new WaitForFixedUpdate();
                            }
                            yield return new WaitForSeconds(time);
                            animator.SetBool("Walking", true);
                            navMeshAgent.destination = pathInfo.paths[indexPath][index];
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
                            if (index == pathInfo.paths[indexPath].Length - 1) index = 0;
                            else index++;
                        }
                    }
                    else if (pathInfo.pathType == GameController.PathType.Repeat)
                    {
                        bool isIncrease = true;
                        while (col.enabled)
                        {
                            Vector3 direction = new Vector3(pathInfo.paths[indexPath][index].x, transform.position.y, pathInfo.paths[indexPath][index].z) - transform.position;
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
                            {
                                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pathInfo.botType == GameController.BotType.Normal ? rotateSpeed : pathInfo.rotateSpeedForBoss);
                                yield return new WaitForFixedUpdate();
                            }
                            yield return new WaitForSeconds(time);
                            animator.SetBool("Walking", true);
                            navMeshAgent.destination = pathInfo.paths[indexPath][index];
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
                            if (index == pathInfo.paths[indexPath].Length - 1 || index == 0) isIncrease = !isIncrease;
                            if (isIncrease) index++;
                            else index--;
                        }
                    }
                }
                else
                {
                    if (pathInfo.pathType == GameController.PathType.Circle)
                    {
                        while (col.enabled)
                        {
                            Vector3 direction = new Vector3(pathInfo.paths[indexPath][index].x, transform.position.y, pathInfo.paths[indexPath][index].z) - transform.position;
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
                            {
                                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pathInfo.botType == GameController.BotType.Normal ? rotateSpeed : pathInfo.rotateSpeedForBoss);
                                yield return new WaitForFixedUpdate();
                            }
                            yield return new WaitForSeconds(time);
                            if (index == pathInfo.paths[indexPath].Length - 1) index = 1;
                            else index++;
                        }
                    }
                    else if (pathInfo.pathType == GameController.PathType.Repeat)
                    {
                        bool isIncrease = false;
                        while (col.enabled)
                        {
                            float startRotate = transform.right.x;
                            Vector3 direction = new Vector3(pathInfo.paths[indexPath][index].x, transform.position.y, pathInfo.paths[indexPath][index].z) - transform.position;
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
                            {
                                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pathInfo.botType == GameController.BotType.Normal ? rotateSpeed : pathInfo.rotateSpeedForBoss);
                                yield return new WaitForFixedUpdate();
                            }
                            yield return new WaitForSeconds(time);
                            if (index == pathInfo.paths[indexPath].Length - 1 || index == 1) isIncrease = !isIncrease;
                            if (isIncrease) index++;
                            else index--;
                        }
                    }
                }
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0, pathInfo.angle, 0);
                while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pathInfo.botType == GameController.BotType.Normal ? rotateSpeed : pathInfo.rotateSpeedForBoss);
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(time);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BlastZone"))
            {
                SubtractHp(100, other.gameObject.layer == LayerMask.NameToLayer("Weapon") ? PlayerController.instance.player.transform : other.transform, false);
            }
        }


        public abstract IEnumerator Attack(GameObject target);

        public IEnumerator Die()
        {
            name = "Die";
            scream.SetActive(true);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            scream.SetActive(false);
        }

        public void IsKinematic(bool isKinematic)
        {
            if (rbs.Length == 0)
            {
                rbs = hips.GetComponentsInChildren<Rigidbody>();
                for (int i = 0; i < rbs.Length; i++)
                {
                    rbs[i].collisionDetectionMode = CollisionDetectionMode.Continuous;
                }
            }
            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].isKinematic = isKinematic;
            }
        }

        public void PlayBlood()
        {
            blood.Play();
        }

        public abstract void InitBot();


        IEnumerator Poison(int damage)
        {
            float time = 3f;

            if (fxPoison == null)
            {
                fxPoison = Instantiate(GameController.instance.preFxPoison, transform).GetComponentInChildren<ParticleSystem>();
                fxPoison.transform.localPosition = Vector3.up * 4f;
            }

            fxPoison.Play();

            while (col.enabled && time > 0)
            {
                yield return new WaitForSeconds(0.25f);
                SubtractHp(damage, PlayerController.instance.player.transform, true);
                time -= 0.25f;
            }

            fxPoison.Stop();
        }

        IEnumerator Burn(int damage)
        {
            float time = 3f;

            if (fxBurn == null)
            {
                fxBurn = Instantiate(GameController.instance.preFxBurn, transform).GetComponentInChildren<ParticleSystem>();
                fxBurn.transform.localPosition = Vector3.up * 3.5f;
            }

            fxBurn.Play();

            while (col.enabled && time > 0)
            {
                yield return new WaitForSeconds(0.25f);
                SubtractHp(damage, PlayerController.instance.player.transform, true);
                time -= 0.25f;
            }

            fxBurn.Stop();
        }
    }
}