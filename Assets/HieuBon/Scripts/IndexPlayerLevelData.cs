using Hunter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    [CreateAssetMenu(fileName = "PlayerTextLevelData", menuName = "ScriptableObjects/PlayerTextLevelData")]
    public class IndexPlayerLevelData : ScriptableObject
    {
        public string playerName;
        public string skillName;
        public Sprite skillSprite;

        public List<IndexTextLevel> indexTextLevels = new List<IndexTextLevel>();
    }

    [System.Serializable]
    public class IndexTextLevel
    {
        public string content;
        public PlayerInformation.ColorType colorType;
    }
}
