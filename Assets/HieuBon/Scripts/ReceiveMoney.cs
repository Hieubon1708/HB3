using DG.Tweening;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    public class ReceiveMoney : MonoBehaviour
    {
        public GameObject[] dollars;
        public Money[] scDollars;
        public Vector3[] dirs;
        public float[] distances;
        LayerMask wallLayer;
        public bool isOk;
        int coin;

        public void Start()
        {
            wallLayer = LayerMask.GetMask("Wall");
        }

        public void FlyMoney(GameObject target, int coin)
        {
            PlayerController.instance.player.playerIndexes.IncreaseGold(ref coin);

            this.coin = coin;
            isOk = false;
            List<float> tempDistances = new List<float>(distances);
            List<float> randomDistances = new List<float>();

            while (tempDistances.Count > 0)
            {
                int indexRandom = Random.Range(0, tempDistances.Count);
                randomDistances.Add(tempDistances[indexRandom]);
                tempDistances.RemoveAt(indexRandom);
            }
            for (int i = 0; i < dollars.Length; i++)
            {
                scDollars[i].Out(new Vector3(dirs[i].x * randomDistances[i], Random.Range(2.5f, 4.5f), dirs[i].z * randomDistances[i]));
            }

            DOVirtual.DelayedCall(0.5f, delegate
            {
                for (int i = 0; i < dollars.Length; i++)
                {
                    scDollars[i].In(target.transform, wallLayer);
                }
            });
        }

        public int GetCoin()
        {
            if (isOk) return 0;
            isOk = true;
            return coin;
        }
    }
}
