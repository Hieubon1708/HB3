using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class IndexPlayerLevel : MonoBehaviour
    {
        IndexBarPlayerLevel[] indexBarPlayerLevels;

        public GameObject[] stars;

        public TextMeshProUGUI playerName;
        public TextMeshProUGUI skillName;
        public Image skillImage;

        public void Awake()
        {
            indexBarPlayerLevels = GetComponentsInChildren<IndexBarPlayerLevel>();         
        }

        public void Start()
        {
            Reload(GameManager.instance.CurrentPlayer);
        }

        public void Reload(GameController.PlayerType playerType)
        {
            PlayerInShop playerInShop = PlayerInformation.instance.GetPlayerInShop(playerType);

            playerName.text = playerInShop.indexPlayerLevelData.playerName;
            skillName.text = playerInShop.indexPlayerLevelData.skillName;
            if (playerInShop.indexPlayerLevelData.skillSprite != null) skillImage.sprite = playerInShop.indexPlayerLevelData.skillSprite;

            int level = playerInShop.GetPlayerData().level;

            for (int i = 0; i < indexBarPlayerLevels.Length; i++)
            {
                if (i < level)
                {
                    indexBarPlayerLevels[i].IsUnlocked(true);
                    stars[i].SetActive(true);
                }
                else
                {
                    indexBarPlayerLevels[i].IsUnlocked(false);
                    stars[i].SetActive(false);
                }
                indexBarPlayerLevels[i].SetText(playerInShop.indexPlayerLevelData.indexTextLevels[i].content, playerInShop.indexPlayerLevelData.indexTextLevels[i].colorType, i + 1);
            }
        }
    }
}
