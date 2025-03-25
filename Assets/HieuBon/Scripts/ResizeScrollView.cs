using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class ResizeScrollView : MonoBehaviour
    {
        public RectTransform canvas;

        public void Awake()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            float height = canvas.sizeDelta.y / 2 + rectTransform.anchoredPosition.y;

            rectTransform.sizeDelta = new Vector2(0, height);
        }
    }
}
