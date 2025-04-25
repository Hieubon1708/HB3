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
            UIInGame.instance.layerCover.DOFade(1f, 0.5f).OnComplete(delegate
            {
                gameObject.SetActive(false);
                map.SetActive(true);

                UIInGame.instance.layerCover.DOFade(0f, 0.5f).SetUpdate(true);
            }).SetUpdate(true);
        }
    }
}
