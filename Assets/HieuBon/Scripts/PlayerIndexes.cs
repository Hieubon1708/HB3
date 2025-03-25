using DG.Tweening;
using UnityEngine;
using static Hunter.GameController;

namespace Hunter
{
    public class PlayerIndexes : MonoBehaviour
    {
        public float attack;
        public float hp;
        public float bloodSucking;
        public float speed;
        public float critDamage;
        public float critRate;
        public float increaseGold;
        public float combo;
        public float comboRate;
        public float receiveBlood;
        public float receiveBloodRate;
        public float getArmor;
        public float getArmorRate;
        public float dodge;
        public float stun;
        public float poison;
        public float poisonRate;
        public float burn;
        public float burnRate;

        public PlayerIndexesData playerIndexesData;
        public PlayerBaseData playerBaseData;

        Player player;

        ParticleSystem fxHealthRegen;

        public ParticleSystem fxShield;

        public void Awake()
        {
            player = GetComponent<Player>();
        }

        public void Init(int playerLevel)
        {
            for (int i = 0; i < playerIndexesData.playerIndexes.Count; i++)
            {
                PlayerIndex playerIndex = playerIndexesData.playerIndexes[i];

                if (playerIndex.level < playerLevel) continue;

                float value = playerIndex.value;

                switch (playerIndex.characterIndex)
                {
                    case CharacterIndex.Attack:
                        attack += value;
                        break;
                    case CharacterIndex.Hp:
                        hp += value;
                        break;
                    case CharacterIndex.BloodSucking:
                        bloodSucking += value;
                        break;
                    case CharacterIndex.Speed:
                        speed += value;
                        break;
                    case CharacterIndex.critDamage:
                        critDamage += value;
                        break;
                    case CharacterIndex.CritRate:
                        critRate += value;
                        break;
                    case CharacterIndex.IncreaseGold:
                        increaseGold += value;
                        break;
                    case CharacterIndex.Combo:
                        combo += value;
                        break;
                    case CharacterIndex.ComboRate:
                        comboRate += value;
                        break;
                    case CharacterIndex.ReceiveBlood:
                        receiveBlood += value;
                        break;
                    case CharacterIndex.ReceiveBloodRate:
                        receiveBloodRate += value;
                        break;
                    case CharacterIndex.GetArmor:
                        getArmor += value;
                        break;
                    case CharacterIndex.GetArmorRate:
                        getArmorRate += value;
                        break;
                    case CharacterIndex.Dodge:
                        dodge += value;
                        break;
                    case CharacterIndex.Stun:
                        stun += value;
                        break;
                    case CharacterIndex.Poison:
                        poison += value;
                        break;
                    case CharacterIndex.Burn:
                        burn += value;
                        break;
                    case CharacterIndex.PoisonRate:
                        poisonRate += value;
                        break;
                    case CharacterIndex.BurnRate:
                        burnRate += value;
                        break;
                    case CharacterIndex.Intrinsic:
                        break;

                    default: break;
                }
            }

            if (receiveBloodRate > 0)
            {
                instance.InstanceHealthRegen(transform, out fxHealthRegen);
            }
            
            if (getArmorRate > 0)
            {
                instance.InstanceShield(transform, out fxShield);
            }

            player.navMeshAgent.speed = playerBaseData.speed + playerBaseData.speed * speed / 100;
            player.startHp = (int)(playerBaseData.hp + playerBaseData.hp * hp / 100);
            player.hp = player.startHp;
        }

        public void HealthRegen()
        {
            fxHealthRegen.Play();
            player.hp = Mathf.Clamp((int)(player.hp + player.startHp * receiveBlood / 100), player.hp, player.startHp);
            player.health.PlusHp();
        }

        Tween delayShield;

        public void ArmorRegen()
        {
            fxShield.Play();

            delayShield.Kill();
            delayShield = DOVirtual.DelayedCall(getArmor, delegate
            {
                fxShield.Stop();
            }).SetUpdate(true);
        }

        public void BloodSucking()
        {
            if (bloodSucking == 0) return;

            player.hp = Mathf.Clamp((int)(player.hp + player.startHp * bloodSucking / 100), player.hp, player.startHp);
            player.health.PlusHp();
        }

        public void IncreaseGold(ref int gold)
        {
            if (increaseGold == 0) return;

            float random = Random.Range(1f, 100f + 1f);

            if (random <= increaseGold)
            {
                gold = (int)(gold * 1.5f);
            }
        }

        public void DamageCrit(ref int damage, ref TextDamageType textDamageType)
        {
            if (critRate == 0) return;

            float random = Random.Range(1f, 100f + 1f);

            if (random <= critDamage)
            {
                textDamageType = TextDamageType.Crit;
                damage += (int)(damage * critDamage / 100);
            }
        }

        public void Combo(ref int damage)
        {
            if (comboRate == 0) return;

            float random = Random.Range(1f, 100f + 1f);

            if (random <= comboRate)
            {
                damage += (int)(damage * combo / 100);
            }
        }

        public bool IsDodge()
        {
            if (dodge == 0) return false;

            float random = Random.Range(1f, 100f + 1f);

            if (random <= dodge)
            {
                return true;
            }

            return false;
        }

        public bool Stun()
        {
            if (stun == 0) return false;

            float random = Random.Range(1f, 100f + 1f);

            if (random <= stun)
            {
                return true;
            }

            return false;
        }

        public void Poison(ref int damage, int totalHp)
        {
            if (poisonRate == 0) return;

            float random = Random.Range(1f, 100f + 1f);

            if (random <= poisonRate)
            {
                damage = (int)poison * totalHp / 100;
            }
        }
        
        public void Burn(ref int damage, int totalHp)
        {
            if (burnRate == 0) return;

            float random = Random.Range(1f, 100f + 1f);

            if (random <= burnRate)
            {
                damage = (int)burn * totalHp / 100;
            }
        }
    }
}