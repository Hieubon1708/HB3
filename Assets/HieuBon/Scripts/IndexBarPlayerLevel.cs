using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class IndexBarPlayerLevel : MonoBehaviour
    {
        public TextMeshProUGUI content;
        public GameObject lockLayer;
        public TextMeshProUGUI textLevel;

        public void SetText(string text, PlayerInformation.ColorType colorType, int level)
        {
            content.text = text;
            textLevel.text = "Lv." + level;

            Color color = PlayerInformation.instance.GetColorType(PlayerInformation.ColorType.Default);

            content.color = color;
        }

        public void IsUnlocked(bool locked)
        {
            lockLayer.SetActive(!locked);
        }
    }
}
