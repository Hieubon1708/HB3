using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter
{
    public class Player : MonoBehaviour
    {
        [HideInInspector]
        public int startHp;
        [HideInInspector]
        public int hp;
        [HideInInspector]
        public int armor;
        [HideInInspector]
        public PlayerHealth health;

        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [HideInInspector]
        public GameObject lookAt;
        [HideInInspector]
        public PlayerIndexes playerIndexes;

        public ParticleSystem blood;
        public CapsuleCollider attackRangeCollider;
        public Transform hand;

        [HideInInspector]
        public bool isKilling;
        public Transform hips;

        [HideInInspector]
        public Rigidbody[] rbs;
        [HideInInspector]
        public CapsuleCollider col;
        [HideInInspector]
        public PlayerWeapon weapon;
        public SkinnedMeshRenderer meshRenderer;

        [HideInInspector]
        public List<GameObject> bots = new List<GameObject>();
        Tween delayKill;
        Material defaultMaterial;
        public Outline outline;

        [HideInInspector]
        public int amountSmoke;
        LayerMask layer;
        [HideInInspector]
        public GetMoney takeMoney;

        TextDamage textDamage;

        public void Awake()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerIndexes = GetComponent<PlayerIndexes>();
            col = GetComponent<CapsuleCollider>();
            health = GetComponentInChildren<PlayerHealth>();
            takeMoney = GetComponentInChildren<GetMoney>();
            textDamage = GetComponentInChildren<TextDamage>();

            lookAt = gameObject;
        }

        public virtual void Start()
        {
            defaultMaterial = meshRenderer.material;
            layer = LayerMask.GetMask("Bot", "Wall");
        }

        public virtual void Init(int playerLevel)
        {
            DOVirtual.DelayedCall(0.06f, delegate
            {
                rbs = hips.GetComponentsInChildren<Rigidbody>();
                IsKinematic(true);
            });
            PlayerController.instance.Init(takeMoney, this);
            playerIndexes.Init(playerLevel);
        }

        public void InitWeapon(PlayerWeapon weapon)
        {
            this.weapon = weapon;
            this.weapon.Init(this);
            attackRangeCollider.radius = weapon.attackRange;
        }

        public void LoadWeapon(GameController.WeaponType weaponType)
        {
            PlayerController.instance.weaponEquip.Equip(this, weaponType);
        }

        public void OnTriggerStay(Collider other)
        {
            if (!col.enabled || weapon == null) return;
            if (other.CompareTag("Bot"))
            {
                RaycastHit hit;
                Vector3 from = transform.position;
                Vector3 to = other.transform.position;
                from.y += 0.5f;
                to.y = from.y;
                Physics.Linecast(from, to, out hit, layer);
                Debug.DrawLine(from, to, Color.yellow, 20);
                if (hit.collider != null && hit.collider.CompareTag("Bot"))
                {
                    if (weapon.weaponType == GameController.WeaponType.Knife)
                    {
                        if (!bots.Contains(other.gameObject))
                        {
                            bots.Add(other.gameObject);
                        }
                        if (isKilling) return;

                        int damage = weapon.damage;

                        if (!isKilling)
                        {
                            PlayerController.instance.player.playerIndexes.Combo(ref damage);
                        }

                        isKilling = true;
                        lookAt = other.gameObject;
                        animator.SetTrigger("Hit");
                        PlayerController.instance.player.navMeshAgent.speed /= 2;
                        delayKill = DOVirtual.DelayedCall(0.35f, delegate
                        {
                            AudioController.instance.PlaySoundNVibrate(AudioController.instance.cut, 0);
                            ChangeLookAt();
                            PlayerController.instance.player.navMeshAgent.speed *= 2;
                            for (int i = 0; i < bots.Count; i++)
                            {
                                Bot bot = LevelController.instance.GetBot(bots[i].gameObject);
                                if (bot != null) bot.SubtractHp(damage, transform, false);
                            }
                            DOVirtual.DelayedCall(0.35f, delegate
                            {
                                isKilling = false;
                                bots.Clear();
                            });
                        });
                    }
                    else
                    {
                        if (isKilling) return;
                        isKilling = true;
                        lookAt = other.gameObject;
                        PlayerController.instance.player.navMeshAgent.speed /= 2;
                        StartCoroutine(weapon.Attack(other.transform));
                    }
                }
            }
        }

        void CheckIndexes(GameController.CharacterIndex characterIndex, Vector3 position)
        {
            switch (characterIndex)
            {
                case GameController.CharacterIndex.ReceiveBlood:
                    break;
            }
        }

        public void SubtractHp(int hp, Transform killer)
        {
            // && UIController.instance.gamePlay.tempStageType == StageType.StealthBoss

            if (this.hp <= 0 || LevelController.instance.bots.Count == 0 || playerIndexes.fxShield != null && playerIndexes.fxShield.isPlaying) return;

            if (playerIndexes.IsDodge())
            {
                textDamage.ShowMiss();
                return;
            }

            if (armor > 0)
            {
                armor -= hp;

                if (armor < 0)
                {
                    this.hp = Mathf.Clamp(this.hp + armor, 0, this.hp);
                }
            }
            else
            {
                this.hp = Mathf.Clamp(this.hp - hp, 0, this.hp);
            }

            textDamage.ShowDamage(hp);

            AudioController.instance.PlaySoundNVibrate(AudioController.instance.playerDie, 50);
            PlayBlood();
            health.SubtractHp();
            UIInGame.instance.virtualCam.StartShakeCam(5f);
            //Vibration.Vibrate(75);
            if (this.hp <= 0)
            {
                Die(killer);
            }
        }

        public void Die(Transform killer)
        {
            weapon.Die();
            health.gameObject.SetActive(false);
            PlayerController.instance.IsHasKey(gameObject);
            LevelController.instance.RemovePlayer(this);
            delayKill.Kill();
            isKilling = false;
            CancelInvoke(nameof(ChangeLookAt));
            UIInGame.instance.virtualCam.ShakeCancel();
            col.enabled = false;
            animator.enabled = false;
            navMeshAgent.enabled = false;
            IsKinematic(false);
            Vector3 dir = transform.position - killer.position;
            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].AddForce(new Vector3(dir.x, dir.y + 0.5f, dir.z) * 1.5f, ForceMode.Impulse);
            }
        }

        public void IsKinematic(bool isKinematic)
        {
            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].isKinematic = isKinematic;
            }
        }

        public void ChangeLookAt()
        {
            lookAt = gameObject;
        }

        public void PlayBlood()
        {
            blood.Play();
        }

        public void SetMaterial(Material material)
        {
            if (material == null)
            {
                amountSmoke--;
            }
            else
            {
                meshRenderer.material = material;
                if (weapon != null)
                {
                    weapon.meshRenderer.material = material;
                    weapon.outline.enabled = false;
                }
                outline.enabled = false;
                amountSmoke++;
            }
            if (amountSmoke == 0)
            {
                meshRenderer.material = defaultMaterial;
                if (weapon != null)
                {
                    weapon.meshRenderer.material = weapon.defaultMaterial;
                    weapon.outline.enabled = true;
                }
                outline.enabled = true;
            }
        }

        public void LoadHealth()
        {
            float percent = GameManager.instance.PercentBlood;
            if (percent == 0) percent = 100;

            hp = (int)(startHp * percent / 100);

            health.healthBar.fillAmount = hp / startHp;
            health.healthDamagerBar.fillAmount = hp / startHp;
        }

        public void InitPlayer()
        {
            ACEPlay.Bridge.BridgeController.instance.Debug_LogError("Percent Hp " + GameManager.instance.PercentBlood.ToString(), false);

            LoadHealth();

            transform.position = Vector3.zero;
            navMeshAgent.angularSpeed = 0;
            IsKinematic(true);
            animator.enabled = true;
            navMeshAgent.enabled = true;
            isKilling = false;
            transform.rotation = Quaternion.identity;
            col.enabled = true;

            ACEPlay.Bridge.BridgeController.instance.Debug_Log("Hp " + startHp);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
