using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HieuBon
{
    public class UIInventory : MonoBehaviour
    {
        public Transform container;

        public List<UIEquipInformation> equipInfos = new List<UIEquipInformation>();

        bool isQuality;

        public UIEquipInformation fakeEquipHat;
        public UIEquipInformation fakeEquipArmor;
        public UIEquipInformation fakeEquipGlove;
        public UIEquipInformation fakeEquipRing;
        public UIEquipInformation fakeEquipShoe;
        public UIEquipInformation fakeEquipGlass;

        bool isTweeningHat;
        bool isTweeningArmor;
        bool isTweeningGlass;
        bool isTweeningShoe;
        bool isTweeningRing;
        bool isTweeningGlove;

        public TextMeshProUGUI textSort;

        public void OnClickSort()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.button, 0);
            DisableFakeEquips();
            isQuality = !isQuality;
            Sort();
        }

        public void DisableFakeEquips()
        {
            fakeEquipHat.transform.SetParent(transform);
            fakeEquipArmor.transform.SetParent(transform);
            fakeEquipGlove.transform.SetParent(transform);
            fakeEquipRing.transform.SetParent(transform);
            fakeEquipShoe.transform.SetParent(transform);
            fakeEquipGlass.transform.SetParent(transform);

            isTweeningHat = false;
            isTweeningArmor = false;
            isTweeningGlass = false;
            isTweeningShoe = false;
            isTweeningRing = false;
            isTweeningGlove = false;
        }

        public UIEquipInformation GetFakeEquip(PlayerInformation.EquipType equipType)
        {
            switch (equipType)
            {
                case PlayerInformation.EquipType.Hat: return fakeEquipHat;
                case PlayerInformation.EquipType.Armor: return fakeEquipArmor;
                case PlayerInformation.EquipType.Glove: return fakeEquipGlove;
                case PlayerInformation.EquipType.Ring: return fakeEquipRing;
                case PlayerInformation.EquipType.Shoe: return fakeEquipShoe;
                case PlayerInformation.EquipType.Glass: return fakeEquipGlass;
            }

            return null;
        }

        public bool GetTweening(PlayerInformation.EquipType equipType)
        {
            switch (equipType)
            {
                case PlayerInformation.EquipType.Hat: return isTweeningHat;
                case PlayerInformation.EquipType.Armor: return isTweeningArmor;
                case PlayerInformation.EquipType.Glove: return isTweeningGlove;
                case PlayerInformation.EquipType.Ring: return isTweeningRing;
                case PlayerInformation.EquipType.Shoe: return isTweeningShoe;
                case PlayerInformation.EquipType.Glass: return isTweeningGlass;
            }

            return false;
        }
        
        public void SetTweening(PlayerInformation.EquipType equipType, bool value)
        {
            switch (equipType)
            {
                case PlayerInformation.EquipType.Hat: isTweeningHat = value; break;
                case PlayerInformation.EquipType.Armor: isTweeningArmor = value; break;
                case PlayerInformation.EquipType.Glove: isTweeningGlove = value; break;
                case PlayerInformation.EquipType.Ring: isTweeningRing = value; break;
                case PlayerInformation.EquipType.Shoe: isTweeningShoe = value; break;
                case PlayerInformation.EquipType.Glass: isTweeningGlass = value; break;
            }
        }

        public void Sort()
        {
            if (isQuality)
            {
                SortByQuality();
                textSort.text = "Class";
            }
            else
            {
                SortByClass();
                textSort.text = "Quality";
            }
        }

        public void AddEquip(UIEquipInformation equipInformation)
        {
            equipInfos.Add(equipInformation);
        }

        public void RemoveEquip(UIEquipInformation equipInformation)
        {
            equipInfos.Remove(equipInformation);
        }

        public void SortByClass()
        {
            List<UIEquipInformation> glasses = new List<UIEquipInformation>();
            List<UIEquipInformation> hats = new List<UIEquipInformation>();
            List<UIEquipInformation> armors = new List<UIEquipInformation>();
            List<UIEquipInformation> shoes = new List<UIEquipInformation>();
            List<UIEquipInformation> gloves = new List<UIEquipInformation>();
            List<UIEquipInformation> rings = new List<UIEquipInformation>();

            foreach (UIEquipInformation equip in equipInfos)
            {
                switch (equip.equipType)
                {
                    case PlayerInformation.EquipType.Glove:
                        gloves.Add(equip); break;
                    case PlayerInformation.EquipType.Hat:
                        hats.Add(equip); break;
                    case PlayerInformation.EquipType.Armor:
                        armors.Add(equip); break;
                    case PlayerInformation.EquipType.Shoe:
                        shoes.Add(equip); break;
                    case PlayerInformation.EquipType.Glass:
                        glasses.Add(equip); break;
                    case PlayerInformation.EquipType.Ring:
                        rings.Add(equip); break;
                }
            }

            List<List<UIEquipInformation>> all = new List<List<UIEquipInformation>>() { glasses, hats, rings, armors, gloves, shoes };

            foreach (var equipType in all)
            {
                SortByQualityLevel(equipType);
            }

            for (int i = 0; i < all.Count; i++)
            {
                for (int j = 0; j < all[i].Count; j++)
                {
                    all[i][j].transform.SetAsLastSibling();
                }
            }
        }

        public void SortByQuality()
        {
            List<UIEquipInformation> reds = new List<UIEquipInformation>();
            List<UIEquipInformation> purples = new List<UIEquipInformation>();
            List<UIEquipInformation> yellows = new List<UIEquipInformation>();
            List<UIEquipInformation> blues = new List<UIEquipInformation>();
            List<UIEquipInformation> greens = new List<UIEquipInformation>();
            List<UIEquipInformation> grays = new List<UIEquipInformation>();

            foreach (UIEquipInformation equip in equipInfos)
            {
                switch (equip.qualityLevel)
                {
                    case PlayerInformation.QualityLevel.Red:
                        reds.Add(equip); break;
                    case PlayerInformation.QualityLevel.Purple:
                        purples.Add(equip); break;
                    case PlayerInformation.QualityLevel.Yellow:
                        yellows.Add(equip); break;
                    case PlayerInformation.QualityLevel.Blue:
                        blues.Add(equip); break;
                    case PlayerInformation.QualityLevel.Green:
                        greens.Add(equip); break;
                    case PlayerInformation.QualityLevel.Gray:
                        grays.Add(equip); break;
                }
            }

            List<List<UIEquipInformation>> all = new List<List<UIEquipInformation>>() { reds, purples, yellows, blues, greens, grays };

            foreach (var equipQuality in all)
            {
                SortByEquipType(equipQuality);
            }

            for (int i = 0; i < all.Count; i++)
            {
                for (int j = 0; j < all[i].Count; j++)
                {
                    all[i][j].transform.SetAsLastSibling();
                }
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SortByClass();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SortByQuality();
            }
        }

        void SortByEquipType(List<UIEquipInformation> equips)
        {
            for (int i = 0; i < equips.Count - 1; i++)
            {
                for (int j = i + 1; j < equips.Count; j++)
                {
                    if (equips[i].equipType < equips[j].equipType)
                    {
                        UIEquipInformation temp = equips[i];
                        equips[i] = equips[j];
                        equips[j] = temp;
                    }
                }
            }
        }

        void SortByQualityLevel(List<UIEquipInformation> equips)
        {
            for (int i = 0; i < equips.Count - 1; i++)
            {
                for (int j = i + 1; j < equips.Count; j++)
                {
                    if (equips[i].qualityLevel < equips[j].qualityLevel)
                    {
                        UIEquipInformation temp = equips[i];
                        equips[i] = equips[j];
                        equips[j] = temp;
                    }
                }
            }
        }
    }
}
