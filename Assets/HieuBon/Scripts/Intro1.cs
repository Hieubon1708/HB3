using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HieuBon
{
    public class Intro1 : MonoBehaviour
    {
        public GameObject map;

        public void Start()
        {
            UIInGame.instance.StartIntro();
        }

        public void SceneChange()
        {
            gameObject.SetActive(false);
            map.SetActive(true);
        }
    }
}
