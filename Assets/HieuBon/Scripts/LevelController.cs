using DG.Tweening;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static Hunter.GameController;

namespace Hunter
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController instance;

        public Transform pool;

        public NavMeshData navMeshData;

        public AlertCamera[] alertCameras;
        public Laser[] lasers;
        public Turrel[] turrels;
        public Barrel[] barrels;
        public Door[] doors;
        public Alarm[] alarms;
        public ObjectBroken[] objectBrokens;

        public bool isAlert;

        public PathInfo[] pathInfos;

        public List<Bot> bots;
        public List<Bot> botsReserve;

        public List<Player> players;

        public AlertType alertType;

        public void Awake()
        {
            instance = this;
            NavMeshSurface navMeshSurface = GetComponent<NavMeshSurface>();
            navMeshSurface.navMeshData = navMeshData;
            navMeshSurface.AddData();

            pool = new GameObject("pool").transform;
            pool.SetParent(transform);

            pathInfos = GetComponentsInChildren<PathInfo>();
            alertCameras = GetComponentsInChildren<AlertCamera>();
            lasers = GetComponentsInChildren<Laser>();
            turrels = GetComponentsInChildren<Turrel>();
            barrels = GetComponentsInChildren<Barrel>();
            alarms = GetComponentsInChildren<Alarm>();
            objectBrokens = GetComponentsInChildren<ObjectBroken>();

            foreach (var pathInfo in pathInfos)
            {
                pathInfo.Init();
            }

            LoadPlayer();
        }

        public void Start()
        {
            InitBots();
            botsReserve = new List<Bot>(bots);

            StartDoor startDoor = GetComponentInChildren<StartDoor>();
            UIInGame.instance.LoadUI(startDoor != null);
            if (startDoor != null) StartCoroutine(startDoor.ElevatorMoveUp(players));
            else
            {
                if (IsBoss()) StartCoroutine(UIInGame.instance.BossIntro());
            }
        }

        public void StopProbes()
        {
            List<Bot> temp = new List<Bot>(bots);
            for (int i = 0; i < temp.Count; i++)
            {
                if (!(temp[i] is Boss1)) temp[i].SubtractHp(999, PlayerController.instance.player.transform, false);
            }
        }

        void ReloadPlayer()
        {
            players[0].LoadHealth();
        }

        public bool IsKilling()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].isKilling) return true;
            }
            return false;
        }

        public void SetAngularSpeed(float angularSpeed)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].navMeshAgent.angularSpeed = angularSpeed;
            }
        }

        bool IsAllPoppyHaveWeapon()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].weapon == null) return false;
            }
            return true;
        }

        public void AddWeapon(WeaponType weaponType)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].weapon == null)
                {
                    players[i].LoadWeapon(weaponType);
                    return;
                }
            }
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].weapon.weaponType == WeaponType.Knife)
                {
                    players[i].LoadWeapon(weaponType);
                    return;
                }
            }
            int indexRandom = Random.Range(0, players.Count);
            players[indexRandom].LoadWeapon(weaponType);
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

        public void LoadPlayer()
        {
            if (players.Count > 0)
            {
                Destroy(players[0].gameObject);
                players.Clear();
            }

            PlayerType playerType = GameManager.instance.CurrentPlayer;
            int playerLevel = 1;
            WeaponType weaponType = (WeaponType)GameManager.instance.Weapon;
            AddPlayer(playerLevel, playerType, weaponType);

            PlayerController.instance.playerTouchMovement.Init(players[0]);
            UIInGame.instance.virtualCam.Init(players[0]);
            players[0].InitPlayer();
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

        public void RemovePlayer(Player player)
        {
            int indexOf = players.IndexOf(player);
            if (indexOf == -1)
            {
                Debug.LogError("!!! " + player.name);
                Debug.LogError("players Count " + players.Count);
            }
            players.Remove(player);
            if (players.Count == 0)
            {
                PlayerController.instance.Lose();
                UIInGame.instance.Lose();
                EndDoor endDoor = GetComponentInChildren<EndDoor>();
                if (endDoor != null) endDoor.StopDoor();
                PlayerController.instance.player.navMeshAgent.isStopped = true;
            }
        }

        public Player GetPlayer(GameObject player)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].gameObject == player)
                {
                    return players[i];
                }
            }
            return null;
        }

        public Laser GetLaser(GameObject laser)
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

        public AlertCamera GetCamera(GameObject camera)
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

        public Player AddPlayer(int playerLevel, PlayerType playerType, WeaponType weaponType)
        {
            GameObject p = Instantiate(GameController.instance.prePlayers[(int)playerType], transform);
            Player sc = p.GetComponent<Player>();
            players.Add(sc);
            sc.LoadWeapon(weaponType);
            sc.Init(playerLevel);
            return sc;
        }

        void RemovePlayers()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].gameObject.activeSelf)
                {
                    Destroy(players[i].gameObject);
                }
            }
        }

        public void ActiveNavMesh(bool isActive)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].navMeshAgent.enabled = isActive;
            }
        }

        public void DoMoveY(float y, float time)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.DOMoveY(y, time);
            }
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

        public void SetBot(PathInfo pathInfo)
        {
            bots.Add(Instantiate(pathInfo.prefab, transform).GetComponent<Bot>());
            bots[bots.Count - 1].Init(pathInfo);
        }

        public void StartProbes()
        {
            for (int i = 0; i < bots.Count; i++)
            {
                bots[i].StartProbe(1);
            }
        }

        public Barrel GetBarrel(GameObject barrel)
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
    }
}