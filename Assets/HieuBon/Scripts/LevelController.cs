using DG.Tweening;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static HieuBon.GameController;

namespace HieuBon
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController instance;

        [HideInInspector]
        public Transform pool;

        public NavMeshData navMeshData;

        [HideInInspector]
        public TrapAlertCamera[] alertCameras;
        [HideInInspector]
        public TrapLaser[] lasers;
        [HideInInspector]
        public TrapTurrel[] turrels;
        [HideInInspector]
        public TrapBarrel[] barrels;
        [HideInInspector]
        public Door[] doors;
        [HideInInspector]
        public TrapAlarm[] alarms;
        [HideInInspector]
        public ObjectBroken[] objectBrokens;
        [HideInInspector]
        public bool isAlert;
        [HideInInspector]
        public PathInfo[] pathInfos;
        [HideInInspector]
        public List<Bot> bots;
        [HideInInspector]
        public List<Bot> botsReserve;
        [HideInInspector]
        public AlertType alertType;
        [HideInInspector]
        public List<Hostage> hostages;

        public Dictionary<TrapKey, GameObject> keys = new Dictionary<TrapKey, GameObject>();

        public void Awake()
        {
            instance = this;
            NavMeshSurface navMeshSurface = GetComponent<NavMeshSurface>();
            navMeshSurface.navMeshData = navMeshData;
            navMeshSurface.AddData();

            pool = new GameObject("pool").transform;
            pool.SetParent(transform);

            pathInfos = GetComponentsInChildren<PathInfo>();
            alertCameras = GetComponentsInChildren<TrapAlertCamera>();
            lasers = GetComponentsInChildren<TrapLaser>();
            turrels = GetComponentsInChildren<TrapTurrel>();
            barrels = GetComponentsInChildren<TrapBarrel>();
            alarms = GetComponentsInChildren<TrapAlarm>();
            objectBrokens = GetComponentsInChildren<ObjectBroken>();

            foreach (var pathInfo in pathInfos)
            {
                pathInfo.Init();
            }
        }

        public void Start()
        {
            InitBots();
            botsReserve = new List<Bot>(bots);

            if (GameManager.instance.Level != 1 && GameManager.instance.Level != 4)
            {
                LoadPlayer(Vector3.zero);
                UIInGame.instance.LoadUI(false);
            }

            /*StartDoor startDoor = GetComponentInChildren<StartDoor>();
            UIInGame.instance.LoadUI(startDoor != null);
            if (startDoor != null) StartCoroutine(startDoor.ElevatorMoveUp(players));
            else
            {
                if (IsBoss()) StartCoroutine(UIInGame.instance.BossIntro());
            }*/
        }

        public void StopProbes()
        {
            List<Bot> temp = new List<Bot>(bots);
            for (int i = 0; i < temp.Count; i++)
            {
                if (!(temp[i] is Boss1)) temp[i].SubtractHp(999, PlayerController.instance.player.transform);
            }
        }

        public void Play()
        {
            StartBots();
            for (int i = 0; i < turrels.Length; i++)
            {
                turrels[i].Play();
            }
            for (int i = 0; i < alertCameras.Length; i++)
            {
                alertCameras[i].Play();
            }
        }

        public void StartBots()
        {
            StartProbes();
        }

        public void LoadPlayer(Vector3 position)
        {
            if (PlayerController.instance != null)
            {
                Destroy(PlayerController.instance.gameObject);
            }

            PlayerType playerType = GameManager.instance.CurrentPlayer;
            int playerLevel = 1;
            WeaponType weaponType = (WeaponType)GameManager.instance.Weapon;
            AddPlayer(playerLevel, playerType, weaponType, position);

            UIInGame.instance.virtualCam.Init(PlayerController.instance.player.transform);
            PlayerController.instance.player.ReloadPlayer();
        }

        public bool IsBoss()
        {
            for (int i = 0; i < pathInfos.Length; i++)
            {
                if (pathInfos[i].botType == BotType.Boss) return true;
            }
            return false;
        }

        public Bot GetBoss()
        {
            for (int i = 0; i < pathInfos.Length; i++)
            {
                if (pathInfos[i].botType == BotType.Boss) return bots[i];
            }
            return null;
        }

        public void RemovePlayer()
        {
            PlayerController.instance.HideTouch();
            UIInGame.instance.Lose();
            EndDoor endDoor = GetComponentInChildren<EndDoor>();
            if (endDoor != null) endDoor.StopDoor();
            PlayerController.instance.player.navMeshAgent.isStopped = true;
        }

        public Player GetPlayer(GameObject player)
        {
            return PlayerController.instance.player;
        }

        public TrapLaser GetLaser(GameObject laser)
        {
            for (int i = 0; i < lasers.Length; i++)
            {
                if (lasers[i].gameObject == laser)
                {
                    return lasers[i];
                }
            }
            return null;
        }

        public TrapAlertCamera GetCamera(GameObject camera)
        {
            for (int i = 0; i < alertCameras.Length; i++)
            {
                if (alertCameras[i].gameObject == camera)
                {
                    return alertCameras[i];
                }
            }
            return null;
        }

        public Player AddPlayer(int playerLevel, PlayerType playerType, WeaponType weaponType, Vector3 position)
        {
            GameObject p = Instantiate(GameController.instance.prePlayers[(int)playerType], position, Quaternion.identity, transform);
            Player sc = p.GetComponent<Player>();
            sc.LoadWeapon(playerType, weaponType);
            sc.Init(playerLevel);
            return sc;
        }

        public Bot GetBot(GameObject bot)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i].gameObject == bot)
                {
                    return bots[i];
                }
            }
            return null;
        }
        
        public Bot GetBot()
        {
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i] is Boss)
                {
                    return bots[i];
                }
            }
            return null;
        }

        public void RemoveBot(GameObject bot)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i].gameObject == bot)
                {
                    bots.RemoveAt(i);
                }
            }
        }

        public void InitBots()
        {
            for (int i = 0; i < bots.Count; i++)
            {
                bots[i].InitBot();
            }
        }

        public Bot SetBot(PathInfo pathInfo)
        {
            Bot bot = Instantiate(pathInfo.prefab, transform).GetComponent<Bot>();
            bots.Add(bot);
            bots[bots.Count - 1].Init(pathInfo);

            return bot;
        }

        public void StartProbes()
        {
            for (int i = 0; i < bots.Count; i++)
            {
                bots[i].StartProbe(1);
            }
        }

        public TrapBarrel GetBarrel(GameObject barrel)
        {
            for (int i = 0; i < barrels.Length; i++)
            {
                if (barrels[i].gameObject == barrel) return barrels[i];
            }
            return null;
        }

        public Bot GetBoneNBot(List<GameObject> bones, GameObject bone)
        {
            Bot bot = null;
            for (int i = 0; i < botsReserve.Count; i++)
            {
                for (int j = 0; j < botsReserve[i].rbs.Length; j++)
                {
                    if (botsReserve[i].rbs[j].gameObject == bone)
                    {
                        bot = botsReserve[i];
                        break;
                    }
                }
                if (bot != null) break;
            }
            if (bot != null)
            {
                for (int i = 0; i < bot.rbs.Length; i++)
                {
                    bones.Add(bot.rbs[i].gameObject);
                }
            }
            return bot;
        }

        public Bot GetBotByBone(GameObject bone)
        {
            for (int i = 0; i < botsReserve.Count; i++)
            {
                for (int j = 0; j < botsReserve[i].rbs.Length; j++)
                {
                    if (botsReserve[i].rbs[j].gameObject == bone)
                    {
                        return botsReserve[i];
                    }
                }
            }
            return null;
        }

        public void Alert(AlertType alertType, GameObject target)
        {
            CancelInvoke(nameof(AlertOff));
            this.alertType = alertType;
            isAlert = true;
            AudioController.instance.PlayAlert();
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i] is BotSentry && bots[i].col.enabled)
                {
                    BotSentry sentryBot = (BotSentry)bots[i];
                    if (sentryBot.questionRotate.transform.localScale == Vector3.zero)
                    {
                        sentryBot.StopProbe();
                        sentryBot.StopLostTrack();
                        sentryBot.StartLostTrack(target);
                    }
                }
            }
            for (int i = 0; i < alarms.Length; i++)
            {
                alarms[i].Alert();
            }
            Invoke(nameof(AlertOff), 7);
        }

        void AlertOff()
        {
            AudioController.instance.StopAlert();
            isAlert = false;
            for (int i = 0; i < alarms.Length; i++)
            {
                alarms[i].StopAlert();
            }
        }

        public bool IsKey(TrapKey key)
        {
            if (keys.ContainsKey(key))
            {
                keys.Remove(key);
                return true;
            }
            return false;
        }

        public void IsHasKey(GameObject value)
        {
            foreach (var item in keys)
            {
                if (item.Value == value)
                {
                    item.Key.ResetKey();
                    keys.Remove(item.Key);
                    return;
                }
            }
        }

        public void SetKey(TrapKey key, GameObject player)
        {
            keys.Add(key, player);
        }
    }
}