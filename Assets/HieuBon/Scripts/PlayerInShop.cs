using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class PlayerInShop : MonoBehaviour
    {
        public bool isUsed;

        Button button;

        public GameObject frameSelect;

        public TextMeshProUGUI textPieceAmount;
        public TextMeshProUGUI textPriceUnlock;

        public Slider sliderPiece;

        public GameObject frameUnlock;

        public GameObject buttonArea;

        public GameObject buttonUnlock;
        public GameObject buttonUse;
        public GameObject buttonUnlockByPrice;
        public GameObject buttonUpgrade;

        public Vector2 sizeOneButton;
        public Vector2 sizeTwoButton;

        public RectTransform rectFrameSelect;

        public PlayerInShopData playerInShopData;

        public IndexPlayerLevelData indexPlayerLevelData;

        public PlayerData playerData;

        public GameObject inUse;

        //test
        public int level;
        public int piece;
        public bool isUnlock;
        public bool isUnlockByPrice;
        public int price;

        public void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public void Start()
        {
            GetPlayerData();
            UpdatePiece();
            CheckUnlock();
            CheckFullUpgrade();
        }

        public PlayerData GetPlayerData()
        {
            if (playerData == null)
            {
                playerData = GameManager.instance.GetPlayerData(playerInShopData.playerType);
                playerData.isUnlocked = isUnlock;
                playerData.level = level;
                playerData.amountPiece = piece;
                playerInShopData.isUnlockByPrice = isUnlockByPrice;
                playerInShopData.price = price;
            }

            return playerData;
        }

        public void DisableFrameSelect()
        {
            frameSelect.SetActive(false);
            buttonArea.SetActive(false);
        }

        void CheckFullUpgrade()
        {
            sliderPiece.gameObject.SetActive(!IsUpgradeFull());
        }

        bool IsUpgradeFull()
        {
            return playerData.level >= playerInShopData.totalUpgrades.Length - 1;
        }

        bool IsEnoughPiece()
        {
            return playerData.amountPiece >= playerInShopData.totalUpgrades[playerData.level];
        }

        void OnClick()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.button, 0);

            bool isUpgradeFull = IsUpgradeFull();
            bool isEnoughPiece = isUpgradeFull ? false : IsEnoughPiece();

            if (isUsed && isUpgradeFull || frameSelect.activeSelf

                || playerData.isUnlocked && !isEnoughPiece && isUsed

                || !playerData.isUnlocked && !isEnoughPiece && !playerInShopData.isUnlockByPrice) return;

            GameController.PlayerType currentPlayerType = PlayerInformation.instance.GetCurrentPlayerModelType();

            PlayerInformation.instance.DisableFrameSelect();

            PlayerInformation.instance.SelectPlayer(playerInShopData.playerType);

            if (currentPlayerType != playerInShopData.playerType)
            {
                PlayerModelInShop playerModelInShop = PlayerInformation.instance.GetPlayerModelInShop(playerInShopData.playerType);

                if (playerModelInShop != null)
                {
                    playerModelInShop.Use();
                }
                else
                {
                    Debug.LogError("!");
                }
            }

            PlayerInformation.instance.ReloadIndexLevel(playerInShopData.playerType);

            ActiveFrameSelect(true);

            if (frameSelect.activeSelf)
            {
                bool isOneButton = false;

                if (!playerData.isUnlocked)
                {
                    buttonUpgrade.SetActive(false);
                    buttonUse.SetActive(false);
                    buttonUnlockByPrice.SetActive(false);

                    buttonUnlock.SetActive(!playerInShopData.isUnlockByPrice);
                    buttonUnlockByPrice.SetActive(playerInShopData.isUnlockByPrice);

                    if (playerInShopData.isUnlockByPrice)
                    {
                        textPriceUnlock.text = "<sprite=1>" + playerInShopData.price;
                    }

                    isOneButton = true;
                }
                else
                {
                    if (!isUpgradeFull) buttonUpgrade.SetActive(isEnoughPiece);
                    buttonUse.SetActive(!isUsed);

                    if (isUsed || isUpgradeFull || !isEnoughPiece) isOneButton = true;
                }

                SetFrameSelectSize(isOneButton);
            }
        }

        void ActiveFrameSelect(bool isActive)
        {
            frameSelect.SetActive(isActive);
            buttonArea.SetActive(isActive);
        }

        void SetFrameSelectSize(bool isOneButton)
        {
            float y = isOneButton ? sizeOneButton.x : sizeTwoButton.x;
            float height = isOneButton ? sizeOneButton.y : sizeTwoButton.y;

            rectFrameSelect.anchoredPosition = new Vector2(rectFrameSelect.anchoredPosition.x, y);
            rectFrameSelect.sizeDelta = new Vector2(rectFrameSelect.sizeDelta.x, height);
        }

        void CheckUnlock()
        {
            frameUnlock.SetActive(!playerData.isUnlocked);
        }

        public void UpdatePiece()
        {
            if (playerData.level >= playerInShopData.totalUpgrades.Length) return;
            textPieceAmount.text = playerData.amountPiece + "/" + playerInShopData.totalUpgrades[playerData.level];
            sliderPiece.value = (float)playerData.amountPiece / playerInShopData.totalUpgrades[playerData.level];
        }

        public void Unlock()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.unlockPlayer, 100);

            playerData.isUnlocked = true;
            frameUnlock.SetActive(false);
            buttonUnlock.SetActive(false);
            buttonUnlockByPrice.SetActive(false);

            PlayerModelInShop playerModelInShop = PlayerInformation.instance.GetPlayerModelInShop(playerInShopData.playerType);

            if (playerModelInShop != null)
            {
                playerModelInShop.Unlock();
            }
            else
            {
                Debug.LogError("!");
            }

            buttonUse.SetActive(true);

            playerData.amountPiece -= playerInShopData.totalUpgrades[playerData.level];

            UpdatePiece();

            buttonUpgrade.SetActive(!IsUpgradeFull());

            SetFrameSelectSize(!IsEnoughPiece());

            PlayerInformation.instance.UpgradePlayerData(PlayerInformation.PlayerUpgradeType.IsUnlock, playerInShopData.playerType);
        }

        public void Use()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.button, 0);

            PlayerInformation.instance.UpgradePlayerData(PlayerInformation.PlayerUpgradeType.IsUsed, playerInShopData.playerType);

            GameManager.instance.CurrentPlayer = playerInShopData.playerType;

            buttonUse.SetActive(false);
            if (buttonUpgrade.activeSelf) SetFrameSelectSize(true);
            else ActiveFrameSelect(false);
        }

        public void Upgrade()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.upgradePlayer, 75);

            playerData.level++;

            PlayerInformation.instance.UpgradePlayerData(PlayerInformation.PlayerUpgradeType.Level, playerInShopData.playerType);

            CheckFullUpgrade();

            if (IsUpgradeFull())
            {
                buttonUpgrade.SetActive(false);
                if (isUsed)
                {
                    ActiveFrameSelect(false);
                }
                else
                {
                    SetFrameSelectSize(true);
                }
            }

            playerData.amountPiece -= playerInShopData.totalUpgrades[playerData.level];

            UpdatePiece();

            if (!IsEnoughPiece())
            {
                buttonUpgrade.SetActive(false);
                if (!isUsed) SetFrameSelectSize(true);
                else ActiveFrameSelect(false);
            }

            PlayerModelInShop playerModelInShop = PlayerInformation.instance.GetPlayerModelInShop(playerInShopData.playerType);

            if (playerModelInShop != null)
            {
                playerModelInShop.Upgrade();
            }
            else
            {
                Debug.LogError("!");
            }

            PlayerInformation.instance.ReloadIndexLevel(playerInShopData.playerType);
        }
    }
}