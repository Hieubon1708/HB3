using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class ButtonHeroAndEquip : MonoBehaviour
    {
        public GameObject backgroundOn;
        public GameObject iconOn;
        public GameObject textOn;

        public Image[] backgroundImages;

        Button button;

        public void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            PlayerInformation.instance.OnClickButtonHeroAndEquip(this);
        }

        public void Enable()
        {
            if (button == null) return;

            backgroundOn.SetActive(true);
            iconOn.SetActive(true);
            textOn.SetActive(true);

            button.targetGraphic = backgroundImages[0];
        }
        
        public void Disable()
        {
            if(button == null) return;

            backgroundOn.SetActive(false);
            iconOn.SetActive(false);
            textOn.SetActive(false);

            button.targetGraphic = backgroundImages[1];
        }
    }
}
