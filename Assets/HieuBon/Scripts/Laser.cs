using DG.Tweening;
using UnityEngine;

namespace Hunter
{
    public class Laser : MonoBehaviour
    {
        public GameObject[] laserLines;
        public BoxCollider[] cols;
        public GameObject button;
        public MeshRenderer[] tubeMeshs;
        public Color disableColor;

        public void LaserOn()
        {
            ActiveLaser(true);
        }

        public void LaserOff()
        {
            ActiveLaser(false);
            for (int i = 0; i < tubeMeshs.Length; i++)
            {
                tubeMeshs[i].material.color = disableColor;
            }
        }

        void ActiveLaser(bool isActive)
        {
            button.transform.DOLocalMoveY(-0.2f, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
            {
                for (int i = 0; i < laserLines.Length; i++)
                {
                    laserLines[i].SetActive(isActive);
                }
            });
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].enabled = isActive;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LaserOff();            
            }
        }
    }
}
