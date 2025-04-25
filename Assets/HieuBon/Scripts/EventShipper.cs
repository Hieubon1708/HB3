using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    public class EventShipper : MonoBehaviour
    {
        public Transform hips;
        public GameObject mesh;
        public ParticleSystem pum;

        public void Transformation()
        {
            mesh.SetActive(false);
            LevelController.instance.LoadPlayer(hips.position);

            DOVirtual.Float(12.452f, 20.60969f, 0.35f, (v) =>
            {
                UIInGame.instance.virtualCam.cinemachineCam.m_Lens.FieldOfView = v;
            }).SetUpdate(true).OnComplete(delegate
            {
                UIInGame.instance.LoadUI(false);
                UIInGame.instance.EndIntro();
            });
        }

        public void Pum()
        {
            pum.Play();

            PlaySound();
        }

        public void PlaySound()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.transformation, 0);
        }
    }
}
