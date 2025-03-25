using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hunter
{
    public class CurrencyEquipTypeInformation : MonoBehaviour
    {
        public EquipInformation equipInformation;
        public RectTransform rectTransform;
        public TextMeshProUGUI textLevel;

        public void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            textLevel = GetComponentInChildren<TextMeshProUGUI>();

            textLevel.enabled = false;
            textLevel.text = "LV 1";
        }

        public void UpdateTextLevel()
        {

        }

        public void Wearing()
        {
            textLevel.enabled = true;
        }

        public void NotWearing()
        {
            textLevel.enabled = false;
        }
    }
}
