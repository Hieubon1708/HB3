using System.Collections.Generic;
using UnityEngine;

namespace HieuBon
{
    [CreateAssetMenu(fileName = "PlayerIndexesData", menuName = "ScriptableObjects/PlayerIndexesData")]
    public class PlayerIndexesData : ScriptableObject
    {
        public List<PlayerIndex> playerIndexes = new List<PlayerIndex>();
    }  

    [System.Serializable]
    public class PlayerIndex
    {
        public int level;
        public GameController.CharacterIndex characterIndex;
        public float value;
    }
}
