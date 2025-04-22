using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
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
        }

        public void Pum()
        {
            pum.Play();
        }
    }
}
