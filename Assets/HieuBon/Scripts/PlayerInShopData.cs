using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    [CreateAssetMenu(fileName = "PlayerInShopData", menuName = "ScriptableObjects/PlayerInShopData")]
    public class PlayerInShopData : ScriptableObject
    {
        public GameController.PlayerType playerType;

        public bool isUnlockByPrice;

        public int price;

        public int[] totalUpgrades;
    }
}