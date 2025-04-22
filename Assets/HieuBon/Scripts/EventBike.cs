using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Hunter
{
    public class BikeEvent : MonoBehaviour
    {
        public Animator characterAnimator;
        public Transform bike;
        public Transform hips;

        public void Jump()
        {
            characterAnimator.SetTrigger("Jump");
            bike.DOLocalMoveY(0f, 0.5f).SetDelay(0.25f).SetUpdate(true);
        }

        void Start()
        {
            UIInGame.instance.virtualCam.Init(hips);

        }
    }
}
