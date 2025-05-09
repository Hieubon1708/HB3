using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    public class EndCard2 : MonoBehaviour
    {
        Boss boss;

        void Start()
        {
            boss = LevelController.instance.GetBoss() as Boss;
        }

        private void Update()
        {
            if (boss.hp <= 0)
            {
                DOVirtual.DelayedCall(1f, delegate
                {
                });
            }
        }
    }
}