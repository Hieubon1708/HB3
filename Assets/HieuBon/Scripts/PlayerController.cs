using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        public PlayerWeaponEquip weaponEquip;

        [HideInInspector]
        public Player player;
        public PlayerTouchMovement playerTouchMovement;
        public HandTutorial handTutorial;

        [HideInInspector]
        public GetMoney takeMoney;
        public ClusterDollar[] clusterDollars;
        int indexClusterDollar;
        public Dictionary<Key, GameObject> keys = new Dictionary<Key, GameObject>();
        public ParticleSystem fxBum;

        public void Awake()
        {
            instance = this;
        }

        public void Init(GetMoney takeMoney, Player player)
        {
            this.player = player;
            this.takeMoney = takeMoney;
        }

        public void ResetFxDollars()
        {
            for (int i = 0; i < clusterDollars.Length; i++)
            {
                clusterDollars[i].ResetFx();
            }
        }

        public void PlayDollars(GameObject target, Vector3 startPos, int coin)
        {
            clusterDollars[indexClusterDollar].transform.position = startPos;
            clusterDollars[indexClusterDollar].gameObject.SetActive(true);
            clusterDollars[indexClusterDollar].PlayDollars(target, coin);
            indexClusterDollar++;
            if (indexClusterDollar == clusterDollars.Length) indexClusterDollar = 0;
        }

        public bool IsKey(Key key)
        {
            if(keys.ContainsKey(key))
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
                if(item.Value == value)
                {
                    item.Key.ResetKey();
                    keys.Remove(item.Key);
                    return;
                }
            }
        }

        public void SetKey(Key key, GameObject player)
        {
            keys.Add(key, player);
        }

        public void Win()
        {
            playerTouchMovement.HandleLoseFinger();
        }

        public void Lose()
        {
            playerTouchMovement.HandleLoseFinger();
        }
    }
}
