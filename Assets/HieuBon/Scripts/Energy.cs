using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace Hunter
{
    public class Energy : MonoBehaviour
    {
        const int energyPerPlay = 5;
        const int totalEnergy = 30;

        public Slider slider;

        public GameObject notEnough;
        public TextMeshProUGUI textEnergy;

        int energy;

        public void Start()
        {
            Init();
        }

        void Init()
        {
            energy = GameManager.instance.Energy;

            energy = Mathf.Clamp(energy, 0, totalEnergy);

            slider.value = energy / totalEnergy;
            textEnergy.text = energy + "/" + totalEnergy;

            if (notEnough != null) notEnough.SetActive(energy >= energyPerPlay);
        }
        public void Play()
        {
            if (energy < energyPerPlay) return;

            energy -= energyPerPlay;
            GameManager.instance.Energy = energy;

            textEnergy.text = energy + "/" + totalEnergy;

            slider.value = energy / totalEnergy;

            notEnough.SetActive(energy >= energyPerPlay);


        }
    }
}
