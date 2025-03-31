using ACEPlay.Bridge;
using UnityEngine;

namespace Hunter
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        [HideInInspector]
        public GameObject map;

        public GameObject[] prePlayers;

        [HideInInspector]
        public Transform container;
        public Transform playerViewContainer;

        public GameObject preFxHealth;
        public GameObject preFxHealthRegen;
        public GameObject preFxShield;
        public GameObject preFxStun;
        public GameObject preFxPoison;
        public GameObject preFxBurn;
        public GameObject preFxArmor;

        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            Init();
        }

        public void Init()
        {
            LoadLevel(GameManager.instance.Level);
        }

        public enum BotType
        {
            Normal,
            Boss
        }

        public enum PathType
        {
            Repeat,
            Circle
        }

        public enum WeaponType
        {
            AK47,
            Crossbow,
            Silencer,
            Knife,
            Pistol,
            RPG,
            Shotgun,
            Default
        }

        public enum AlertType
        {
            Camera, Laser
        }

        public enum PlayerType
        {
            Megamon,
            NinjaSonic,
            GreenNative,
            None
        }

        public enum CharacterIndex
        {
            None,
            Attack,
            Hp,
            BloodSucking,
            Speed,
            critDamage,
            CritRate,
            IncreaseGold,
            Combo,
            ComboRate,
            ReceiveBlood,
            ReceiveBloodRate,
            GetArmor,
            GetArmorRate,
            Dodge,
            Stun,
            Poison,
            PoisonRate,
            Burn,
            BurnRate,
            Intrinsic
        }

        public enum TextDamageType
        {
            None, Normal, Crit
        }

        public void LoadLevel(int level)
        {
            BridgeController.instance.LogLevelStartWithParameter("stealk", level);
            AudioController.instance.ResetAudio();
            BridgeController.instance.Debug_Log(level.ToString());

            PlayerController.instance.ResetFxDollars();

            if (map != null) Destroy(map);
            map = Instantiate(Resources.Load<GameObject>(level.ToString()), container);
        }

        public void ReceiveBlood(Vector3 position)
        {
            Player player = PlayerController.instance.player;

            if (player != null)
            {
                if (player.playerIndexes.receiveBloodRate == 0) return;

                int random = Random.Range(1, 100 + 1);

                if (random <= player.playerIndexes.receiveBloodRate)
                {
                    Instantiate(preFxHealth, position + Vector3.up * 1.85f, Quaternion.identity, LevelController.instance.pool);
                }
            }
        }
        
        public void ReceiveArmor(Vector3 position)
        {
            Player player = PlayerController.instance.player;

            if (player != null)
            {
                if (player.playerIndexes.getArmorRate == 0) return;

                int random = Random.Range(1, 100 + 1);

                if (random <= player.playerIndexes.getArmorRate)
                {
                    Instantiate(preFxArmor, position + Vector3.up * 1.85f, Quaternion.identity, LevelController.instance.pool);
                }
            }
        }

        public void InstanceHealthRegen(Transform parent, out ParticleSystem particleSystem)
        {
            GameObject healthRegen = Instantiate(preFxHealthRegen, parent.position, Quaternion.identity, parent);
            healthRegen.transform.localPosition = new Vector3(0, 3f, 0);
            particleSystem = healthRegen.GetComponent<ParticleSystem>();
        }
        
        public void InstanceShield(Transform parent, out ParticleSystem particleSystem)
        {
            GameObject shield = Instantiate(preFxShield, parent.position, Quaternion.identity, parent);
            shield.transform.localPosition = new Vector3(0, 3.5f, 0);
            particleSystem = shield.GetComponentInChildren<ParticleSystem>();
        }
    }
}
