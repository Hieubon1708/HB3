using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    [CreateAssetMenu(fileName = "PlayerBaseData", menuName = "ScriptableObjects/PlayerBaseData")]
    public class PlayerBaseData : ScriptableObject
    {
        public int hp;
        public float speed;
    }
}
