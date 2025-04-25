using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HieuBon.PlayerInformation;

namespace HieuBon
{
    [CreateAssetMenu(fileName = "EquipData", menuName = "ScriptableObjects/EquipData")]
    public class EquipData : ScriptableObject
    {
        public EquipType equipType;
        public PlayerInformation.QualityLevel qualityLevel;

        public int damage;
        public int hp;
    }
}
