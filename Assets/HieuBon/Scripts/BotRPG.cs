using ACEPlay.Bridge;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public class RPGBot : Bot
    {
        public int amountBullet;
        public GameObject preBullet;
        public BotRPGBullet[] bullets;
        public int indexBullet;
        public float speedBullet;
        public float timeDelay;
        public SpriteRenderer healthBar1;
        public SpriteRenderer healthBar2;
        public SpriteRenderer healthBar3;
        Coroutine dodging;
        public ParticleSystem fxTarget;
        LayerMask playerLayer;
        public Collider[] targets;
        public GameObject weaponRadius;
        public Vector3 destination;

        public void Start()
        {
            fxTarget.transform.SetParent(GameController.instance.map.transform);
            playerLayer = LayerMask.GetMask("Player");
            bullets = new BotRPGBullet[amountBullet];
            for (int i = 0; i < amountBullet; i++)
            {
                GameObject b = Instantiate(preBullet, LevelController.instance.pool);
                bullets[i] = b.GetComponent<BotRPGBullet>();
                bullets[i].bot = this;
                b.SetActive(false);
            }
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

        private void FixedUpdate()
        {
            if (isDodging || !col.enabled) return;
            targets = Physics.OverlapSphere(transform.position, 10f, playerLayer);
            if (targets.Length > 0 && PlayerController.instance.player.amountSmoke == 0)
            {
                transform.LookAt(targets[0].transform);
                StartAttack(null);
            }
        }

        public override IEnumerator Attack(GameObject target)
        {
            animator.SetTrigger("Fire");
            yield return null;
        }

        public void Throw()
        {
            if (targets.Length == 0 || PlayerController.instance.player.amountSmoke > 0)
            {
                StopAttack();
                return;
            }
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.rpg, 0);
            weapon.Attack(null);
            destination = targets[0].transform.position;
            bullets[indexBullet].Init(weapon.startBullet.position);
            int index = indexBullet;
            fxTarget.transform.position = destination;
            fxTarget.transform.DOScale(10f / 4f, rateOfFire).SetEase(Ease.Linear).OnComplete(delegate
            {
                fxTarget.transform.localScale = Vector3.zero;
            });
            bullets[indexBullet].transform.DOJump(destination, 7, 1, rateOfFire).SetEase(Ease.Linear).OnComplete(delegate
            {
                bullets[index].OnGround();
                StopAttack();
            });
            indexBullet++;
            if (indexBullet == bullets.Length) indexBullet = 0;
        }

        IEnumerator Dodging(Transform killer)
        {
            isDodging = true;
            StopAttack();
            navMeshAgent.isStopped = false;
            Vector3 dirOfAttack = (transform.position - killer.position).normalized;
            navMeshAgent.destination = transform.position + dirOfAttack * 2;
            animator.SetBool("Walking", true);
            animator.SetTrigger("Dodging");
            yield return new WaitForSeconds(0.5f);
            navMeshAgent.isStopped = true;
            animator.SetBool("Walking", false);
            yield return new WaitForSeconds(0.5f);
            isDodging = false;
        }

        public override void SubtractHp(int hp, Transform killer, bool isOnlyBurn)
        {
            if (this.hp <= 0) return;

            base.SubtractHp(hp, killer, isOnlyBurn);

            PlayBlood();
            StopDodging();
            if (this.hp >= 200)
            {
                healthBar3.color = Color.black;
                StartDodging(killer);
            }
            else if (this.hp >= 100)
            {
                healthBar2.color = Color.black;
                StartDodging(killer);
            }
            if (this.hp <= 0)
            {
                Destroy(weaponRadius);
                healthBar1.enabled = false;
                healthBar2.enabled = false;
                healthBar3.enabled = false;
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDie, 50);
                UIInGame.instance.HitEffect();
                UIInGame.instance.virtualCam.StartShakeCam(7.5f);
                StopAttack();
                col.enabled = false;
                animator.enabled = false;
                IsKinematic(false);
                StartCoroutine(Die());

                Vector3 dir = transform.position - PlayerController.instance.player.transform.position;
                for (int i = 0; i < rbs.Length; i++)
                {
                    rbs[i].AddForce(new Vector3(dir.x, dir.y + 1, dir.z).normalized * 7, ForceMode.Impulse);
                }

                LevelController.instance.RemoveBot(gameObject);
                UIInGame.instance.gamePlay.UpdateRemainingEnemy();
            }
            else
            {
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.enemyDamage, 50);
            }
        }

        public void OnDestroy()
        {
            Destroy(weaponRadius);
        }

        public override void InitBot()
        {
            hp = startHp;
            indexPath = 0;
            IsKinematic(true);
            col.enabled = true;
            animator.enabled = true;
            isKilling = false;
            navMeshAgent.enabled = true;
            navMeshAgent.isStopped = false;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pathInfo.paths[0][0], out hit, 100, NavMesh.AllAreas))
            {
                navMeshAgent.Warp(hit.position);
            }
            else BridgeController.instance.Debug_LogWarning("!");
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, pathInfo.angle, transform.eulerAngles.z);
            navMeshAgent.destination = transform.position;

            healthBar1.enabled = true;
            healthBar2.enabled = true;
            healthBar3.enabled = true;
            healthBar2.color = Color.white;
            healthBar3.color = Color.white;
        }
    }
}
