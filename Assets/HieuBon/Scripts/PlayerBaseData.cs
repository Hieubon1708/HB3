using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    [CreateAssetMenu(fileName = "PlayerBaseData", menuName = "ScriptableObjects/PlayerBaseData")]
    public class PlayerBaseData : ScriptableObject
    {
        public int hp;
        public float speed;
    }
}
