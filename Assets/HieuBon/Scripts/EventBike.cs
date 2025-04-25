using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HieuBon
{
    public class BikeEvent : MonoBehaviour
    {
        public Animator characterAnimator;
        public Transform bike;
        public Transform hips;
        public Animation ani;

        public void Jump()
        {
            characterAnimator.SetTrigger("Jump");
            bike.DOLocalMoveY(0f, 0.5f).SetDelay(0.25f).SetUpdate(true);
        }

        void Start()
        {
            ani.Play();
            UIInGame.instance.virtualCam.Init(hips);
            UIInGame.instance.virtualCam.cinemachineCam.m_Lens.FieldOfView = 12.452f;
        }
    }
}
