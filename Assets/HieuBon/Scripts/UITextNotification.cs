using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hunter
{
    public class TextNofication : MonoBehaviour
    {
        public Animation aniNotification;
        public TextMeshProUGUI textNotification;

        public void ShowText(string text)
        {
            textNotification.text = text;
            aniNotification.Play();
        }
    }
}
