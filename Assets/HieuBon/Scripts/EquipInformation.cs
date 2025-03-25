using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class EquipInformation : MonoBehaviour
    {
        public PlayerInformation.EquipType equipType;
        public PlayerInformation.QualityLevel qualityLevel;

        [HideInInspector]
        public TextMeshProUGUI text;
        [HideInInspector]
        public RectTransform rectTransform;

        Button button;
        ButtonScale buttonScale;
        Image frame;

        [HideInInspector]
        public bool isWeared;

        [HideInInspector]
        public Canvas canvas;

        [HideInInspector]
        public GraphicRaycaster raycaster;

        public void Awake()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            button = GetComponent<Button>();
            frame = GetComponent<Image>();
            buttonScale = GetComponent<ButtonScale>();
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();

            if (button != null)
            {
                button.onClick.AddListener(OnClick);
                button.targetGraphic = frame;
            }
        }

        public void Start()
        {
            if (button != null) name = equipType.ToString() + " " + qualityLevel.ToString();
            else
            {
                name = "Fakeeeeeeeeeeeeeeee";
            }
        }

        public void Init(string name, PlayerInformation.EquipType equipType, PlayerInformation.QualityLevel qualityLevel, bool isWeared)
        {
            text.text = name;
            this.equipType = equipType;
            this.qualityLevel = qualityLevel;
            this.isWeared = isWeared;
        }

        public void Wearing(Transform parent)
        {
            isWeared = true;

            buttonScale.enabled = false;

            button.transition = Selectable.Transition.ColorTint;

            rectTransform.SetParent(parent);

            rectTransform.sizeDelta = new Vector2(190f, 190f);
            rectTransform.localScale = Vector3.one;

            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        }

        public void NotWearing()
        {
            isWeared = false;

            buttonScale.enabled = true;

            button.transition = Selectable.Transition.None;

            rectTransform.SetParent(PlayerInformation.instance.inventory.container);

            rectTransform.anchoredPosition3D = new Vector3(0, 16.6f, 0);
            rectTransform.localScale = Vector3.one;
        }

        void OnClick()
        {
            PlayerInformation.instance.SwapEquipCurrency(this);
        }

        public Equip GetEquip()
        {
            return new Equip(equipType, qualityLevel, isWeared);
        }
    }
}
