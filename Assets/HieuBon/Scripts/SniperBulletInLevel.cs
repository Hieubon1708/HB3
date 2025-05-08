using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace HieuBon
{
    public class SniperBulletInLevel : MonoBehaviour
    {
        public SniperInLevel sniperInLevel;
        CinemachineVirtualCamera cinemachineCam;
        public ParticleSystem smoke;
        public GameObject mesh;

        private void Awake()
        {
            cinemachineCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
        }

        public void Shoot(Vector3 startPosition, Vector3 target, Quaternion lookAt, Action callBack1, Action callBack2)
        {
            smoke.Play();

            transform.position = startPosition;

            float time = 3f;

            mesh.SetActive(true);
            cinemachineCam.gameObject.SetActive(true);

            transform.rotation = lookAt;

            transform.DOMove(target, time).SetUpdate(true).SetEase(Ease.Linear).OnComplete(delegate
            {
                if (callBack1 != null) callBack1.Invoke();

                mesh.SetActive(false);

                DOVirtual.DelayedCall(1f, delegate
                {
                    cinemachineCam.gameObject.SetActive(false);
                    if (callBack2 != null) callBack2.Invoke();
                });

            }).SetUpdate(UpdateType.Normal);

            cinemachineCam.transform.DOLocalMoveZ(3f, time).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal);

            CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

            float brainTimeỎigin = brain.m_DefaultBlend.m_Time;

            brain.m_DefaultBlend.m_Time = 0f;

            cinemachineCam.transform.parent.DOLocalRotate(new Vector3(0, 160f, 0), time).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal);

            DOVirtual.Float(0.15f, 1f, time / 3, (v) =>
            {
                Time.timeScale = v;
            }).SetEase(Ease.Linear);
        }
    }
}
