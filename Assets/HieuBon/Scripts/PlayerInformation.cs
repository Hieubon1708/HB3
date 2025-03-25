using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Hunter.PlayerInformation;
using static Hunter.PlayerInShop;

namespace Hunter
{
    public class PlayerInformation : MonoBehaviour
    {
        public static PlayerInformation instance;

        [HideInInspector]
        public Inventory inventory;

        public ButtonHeroAndEquip[] buttonHeroAndEquips;

        public GameObject preFrameGray;
        public GameObject preFrameGreen;
        public GameObject preFrameBlue;
        public GameObject preFrameYellow;
        public GameObject preFramePurple;
        public GameObject preFrameRed;

        public CurrencyEquipTypeInformation currentGlove;
        public CurrencyEquipTypeInformation currentHat;
        public CurrencyEquipTypeInformation currentArmor;
        public CurrencyEquipTypeInformation currentShoe;
        public CurrencyEquipTypeInformation currentGlass;
        public CurrencyEquipTypeInformation currentRing;

        List<EquipInformation> equipInfos = new List<EquipInformation>();

        public Color healthColor;
        public Color damageColor;
        public Color damageCritColor;
        public Color defaultColor;

        public WeaponData weaponData;

        public GameObject equipParent;
        public GameObject heroParent;

        public ScrollRect scrollRectIndex;

        public PlayerModelInShop[] prePlayers;

        [HideInInspector]
        public List<PlayerModelInShop> playerModels = new List<PlayerModelInShop>();

        public RectTransform background;

        List<PlayerData> playerDatas = new List<PlayerData>();

        PlayerInShop[] playerInShops;
        IndexPlayerLevel indexPlayerLevel;

        public EquipData[] equipDatas;

        IndexAttackAndHealth indexAttackAndHealth;

        [HideInInspector]
        public bool isDancing;

        [HideInInspector]
        public GameObject playerModel;

        public enum ColorType
        {
            Health, Damage, DamageCrit, Default
        }

        public enum EquipType
        {
            Shoe, Glove, Armor, Ring, Hat, Glass
        }

        public enum QualityLevel
        {
            Gray, Green, Blue, Yellow, Purple, Red
        }

        public enum WeaponUpgradeType
        {
            Weapon, Hat, Armor, Shoe, Ring, Necklace
        }

        public enum PlayerUpgradeType
        {
            Level, Piece, IsUnlock, IsUsed
        }

        public EquipData GetEquipData(EquipType equipType, PlayerInformation.QualityLevel qualityLevel)
        {
            foreach (var equipData in equipDatas)
            {
                if (equipData.equipType == equipType && equipData.qualityLevel == qualityLevel) return equipData;
            }

            return null;
        }

        public GameObject GetPrePlayer(GameController.PlayerType playerType)
        {
            for (int i = 0; i < prePlayers.Length; i++)
            {
                if (prePlayers[i].playerType == playerType) return prePlayers[i].gameObject;
            }

            return prePlayers[0].gameObject;
        }

        public void ReloadIndexLevel(GameController.PlayerType playerType)
        {
            indexPlayerLevel.Reload(playerType);
        }

        public GameObject GetPreFrame(QualityLevel qualityLevel)
        {
            switch (qualityLevel)
            {
                case QualityLevel.Green: return preFrameGreen;
                case QualityLevel.Blue: return preFrameBlue;
                case QualityLevel.Yellow: return preFrameYellow;
                case QualityLevel.Purple: return preFramePurple;
                case QualityLevel.Red: return preFrameRed;
                case QualityLevel.Gray: return preFrameGray;
            }
            return null;
        }

        public Transform GetParentEquipCurrency(EquipType equipType)
        {
            switch (equipType)
            {
                case EquipType.Glass: return currentGlass.transform;
                case EquipType.Shoe: return currentShoe.transform;
                case EquipType.Armor: return currentArmor.transform;
                case EquipType.Hat: return currentHat.transform;
                case EquipType.Glove: return currentGlove.transform;
                case EquipType.Ring: return currentRing.transform;
            }
            return null;
        }

        public PlayerModelInShop GetPlayerModelInShop(GameController.PlayerType playerType)
        {
            foreach (var playerModel in playerModels)
            {
                if (playerModel.playerType == playerType) return playerModel;
            }
            return null;
        }

        CurrencyEquipTypeInformation GetCurrencyEquipInfor(EquipType equipType)
        {
            switch (equipType)
            {
                case EquipType.Glass:
                    return currentGlass;
                case EquipType.Shoe:
                    return currentShoe;
                case EquipType.Armor:
                    return currentArmor;
                case EquipType.Hat:
                    return currentHat;
                case EquipType.Glove:
                    return currentGlove;
                case EquipType.Ring:
                    return currentRing;
            }
            return null;
        }

        public Color GetColorType(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.Health:
                    return healthColor;
                case ColorType.DamageCrit:
                    return damageCritColor;
                case ColorType.Damage:
                    return damageColor;
                default:
                    return defaultColor;
            }
        }

        public void UpgradeWeapon(WeaponUpgradeType weaponUpgradeType)
        {
            switch (weaponUpgradeType)
            {
                case WeaponUpgradeType.Weapon:
                    weaponData.weaponLevel++;
                    break;
                case WeaponUpgradeType.Shoe:
                    weaponData.shoeLevel++;
                    break;
                case WeaponUpgradeType.Hat:
                    weaponData.hatLevel++;
                    break;
                case WeaponUpgradeType.Ring:
                    weaponData.ringLevel++;
                    break;
                case WeaponUpgradeType.Necklace:
                    weaponData.necklaceLevel++;
                    break;
                case WeaponUpgradeType.Armor:
                    weaponData.armorLevel++;
                    break;
            }

            GameManager.instance.WeaponData = weaponData;
        }

        public void UpgradePlayerData(PlayerUpgradeType playerUpgradeType, GameController.PlayerType playerType, int amountPiece = 0)
        {
            PlayerData playerData = GameManager.instance.GetPlayerData(playerType);

            switch (playerUpgradeType)
            {
                case PlayerUpgradeType.Level:
                    playerData.level++;
                    break;
                case PlayerUpgradeType.Piece:
                    playerData.amountPiece += amountPiece;
                    break;
                case PlayerUpgradeType.IsUnlock:
                    playerData.isUnlocked = true;
                    break;
                case PlayerUpgradeType.IsUsed:
                    WhoUsed(playerType);
                    break;
            }

            GameManager.instance.SetPlayerData(playerData);
        }

        void WhoUsed(GameController.PlayerType playerType)
        {
            foreach (var playerInShop in playerInShops)
            {
                if (playerInShop.playerInShopData.playerType == playerType)
                {
                    playerInShop.isUsed = true;
                    playerInShop.inUse.SetActive(true);
                }
                else
                {
                    playerInShop.isUsed = false;
                    playerInShop.inUse.SetActive(false);
                }
            }
        }

        public void Awake()
        {
            instance = this;
            inventory = GetComponentInChildren<Inventory>();
            indexAttackAndHealth = GetComponentInChildren<IndexAttackAndHealth>(true);
            indexPlayerLevel = GetComponentInChildren<IndexPlayerLevel>(true);
            playerInShops = GetComponentsInChildren<PlayerInShop>(true);
        }

        public void Start()
        {
            GameManager.instance.Equipments = new List<Equip>() {
                new Equip(EquipType.Glass, PlayerInformation.QualityLevel.Red, false) ,
                new Equip(EquipType.Glass, PlayerInformation.QualityLevel.Purple, false) ,
                new Equip(EquipType.Shoe, PlayerInformation.QualityLevel.Blue, false) ,
                new Equip(EquipType.Shoe, PlayerInformation.QualityLevel.Green, false) ,
                new Equip(EquipType.Ring, PlayerInformation.QualityLevel.Gray, false) ,
                new Equip(EquipType.Ring, PlayerInformation.QualityLevel.Yellow, false) ,
                new Equip(EquipType.Glove, PlayerInformation.QualityLevel.Red, false) ,
                new Equip(EquipType.Glove, PlayerInformation.QualityLevel.Gray, false) ,
                new Equip(EquipType.Armor, PlayerInformation.QualityLevel.Green, false) ,
                new Equip(EquipType.Armor, PlayerInformation.QualityLevel.Yellow, false) ,
                new Equip(EquipType.Hat, PlayerInformation.QualityLevel.Blue, false),
                new Equip(EquipType.Hat, PlayerInformation.QualityLevel.Red, false)
            };

            GameController.PlayerType[] playerEnums = (GameController.PlayerType[])Enum.GetValues(typeof(GameController.PlayerType));

            for (int i = 0; i < playerEnums.Length; i++)
            {
                PlayerData playerData = GameManager.instance.GetPlayerData(playerEnums[i]);
                playerDatas.Add(playerData);
            }

            GameController.PlayerType playerType = GameManager.instance.CurrentPlayer;

            WhoUsed(playerType);

            OnClickButtonHeroAndEquip(buttonHeroAndEquips[0]);

            List<Equip> equips = GameManager.instance.Equipments;

            foreach (Equip equip in equips)
            {
                GameObject preFrame = GetPreFrame(equip.qualityLevel);

                GameObject e = Instantiate(preFrame);

                EquipInformation equipInformation = e.GetComponent<EquipInformation>();

                equipInfos.Add(equipInformation);

                CurrencyEquipTypeInformation currencyEquipInformation = GetCurrencyEquipInfor(equip.equipType);

                equipInformation.Init(equip.equipType.ToString(), equip.equipType, equip.qualityLevel, equipInformation.isWeared);

                if (equip.isWeared)
                {
                    Transform parent = PlayerInformation.instance.GetParentEquipCurrency(equip.equipType);
                    equipInformation.Wearing(parent);
                    currencyEquipInformation.Wearing();
                }
                else
                {
                    NotWearing(equipInformation);
                }

            }

            SelectPlayer(playerType);
            inventory.Sort();
        }

        public void DisableFrameSelect()
        {
            foreach (var playerInShop in playerInShops)
            {
                playerInShop.DisableFrameSelect();
            }
        }

        public PlayerInShop GetPlayerInShop(GameController.PlayerType playerType)
        {
            foreach (var player in playerInShops)
            {
                if (player.playerInShopData.playerType == playerType) return player;
            }
            return null;
        }

        public void SelectPlayer(GameController.PlayerType playerType)
        {
            bool isContain = false;
            for (int i = 0; i < playerModels.Count; i++)
            {
                if (playerModels[i].playerType == playerType)
                {
                    isContain = true;

                    playerModels[i].gameObject.SetActive(true);

                    playerModel = playerModels[i].gameObject;
                }
                else
                {
                    playerModels[i].gameObject.SetActive(false);
                }
            }
            if (!isContain)
            {
                GameObject pre = GetPrePlayer(playerType);
                if (pre != null)
                {
                    GameObject p = Instantiate(pre, GameController.instance.playerViewContainer);
                    PlayerModelInShop playerModelInShop = p.GetComponent<PlayerModelInShop>();

                    playerModelInShop.playerType = playerType;
                    playerModels.Add(playerModelInShop);

                    playerModel = p;
                }
                else
                {
                    Debug.LogError("!");
                }
            }

            playerModel.transform.rotation = Quaternion.identity;
        }

        public GameController.PlayerType GetCurrentPlayerModelType()
        {
            foreach (var model in playerModels)
            {
                if (model.gameObject.activeSelf) return model.playerType;
            }
            return GameController.PlayerType.None;
        }

        public struct AttackAndHealth
        {
            public int attack;
            public int health;

            public AttackAndHealth(int attackValue, int healthValue)
            {
                attack = attackValue;
                health = healthValue;
            }

            public static AttackAndHealth operator +(AttackAndHealth a, AttackAndHealth b)
            {
                return new AttackAndHealth(a.attack + b.attack, a.health + b.health);
            }

            public static AttackAndHealth Zero
            {
                get
                {
                    return new AttackAndHealth(0, 0);
                }
            }
        }

        AttackAndHealth GetAttackAndHealthQuip(EquipType equipType, PlayerInformation.QualityLevel qualityLevel)
        {
            foreach (var equipData in equipDatas)
            {
                if (equipData.equipType == equipType && equipData.qualityLevel == qualityLevel) return new AttackAndHealth(equipData.damage, equipData.hp);
            }
            return new AttackAndHealth(0, 0);
        }

        public AttackAndHealth GetCurrentAttackAndHealth()
        {
            //PlayerIndexes playerIndexes = PlayerController.instance.player.playerIndexes;

            AttackAndHealth attackAndHealth = new AttackAndHealth(10, 100);

            if (currentArmor.equipInformation != null) attackAndHealth += GetAttackAndHealthQuip(currentArmor.equipInformation.equipType, currentArmor.equipInformation.qualityLevel);
            if (currentHat.equipInformation != null) attackAndHealth += GetAttackAndHealthQuip(currentHat.equipInformation.equipType, currentHat.equipInformation.qualityLevel);
            if (currentGlass.equipInformation != null) attackAndHealth += GetAttackAndHealthQuip(currentGlass.equipInformation.equipType, currentGlass.equipInformation.qualityLevel);
            if (currentRing.equipInformation != null) attackAndHealth += GetAttackAndHealthQuip(currentRing.equipInformation.equipType, currentRing.equipInformation.qualityLevel);
            if (currentShoe.equipInformation != null) attackAndHealth += GetAttackAndHealthQuip(currentShoe.equipInformation.equipType, currentShoe.equipInformation.qualityLevel);
            if (currentGlove.equipInformation != null) attackAndHealth += GetAttackAndHealthQuip(currentGlove.equipInformation.equipType, currentGlove.equipInformation.qualityLevel);

            return attackAndHealth;
        }

        public void OnClickButtonHeroAndEquip(ButtonHeroAndEquip buttonHeroAndEquip)
        {
            for (int i = 0; i < buttonHeroAndEquips.Length; i++)
            {
                if (buttonHeroAndEquips[i] == buttonHeroAndEquip)
                {
                    buttonHeroAndEquips[i].Enable();
                }
                else
                {
                    buttonHeroAndEquips[i].Disable();
                }
            }
        }

        public void SwapEquipCurrency(EquipInformation equipSelect)
        {
            if (inventory.GetTweening(equipSelect.equipType)) return;
            inventory.SetTweening(equipSelect.equipType, true);

            CurrencyEquipTypeInformation currencyEquipInformation = GetCurrencyEquipInfor(equipSelect.equipType);

            AttackAndHealth currentAttackAndHealth = GetCurrentAttackAndHealth();

            EquipInformation equipFake = inventory.GetFakeEquip(equipSelect.equipType);
            int indexOfFake = equipFake.transform.GetSiblingIndex();

            if (equipSelect.isWeared)
            {
                currencyEquipInformation.NotWearing();

                equipSelect.NotWearing();

                currencyEquipInformation.equipInformation = null;

                equipFake.transform.SetParent(inventory.transform);

                inventory.AddEquip(equipSelect);

                AttackAndHealth targetAttackAndHealth = GetCurrentAttackAndHealth();

                indexAttackAndHealth.UpdateAttackAndHealth(currentAttackAndHealth.attack, targetAttackAndHealth.attack, currentAttackAndHealth.health, targetAttackAndHealth.health);

                inventory.Sort();

                inventory.SetTweening(equipSelect.equipType, false);
            }
            else
            {
                Transform parent = PlayerInformation.instance.GetParentEquipCurrency(equipSelect.equipType);
                int indexOf = equipSelect.transform.GetSiblingIndex();

                Vector2 startPosition = equipSelect.transform.position;
                equipSelect.Wearing(parent);
                equipSelect.transform.position = startPosition;

                equipSelect.rectTransform.DOAnchorPos3D(new Vector3(0, 16.6f, 0), 0.4f).OnStart(delegate
                {
                    inventory.RemoveEquip(equipSelect);

                    equipFake.transform.SetParent(inventory.container);
                    equipFake.transform.SetSiblingIndex(indexOf);
                    equipFake.equipType = equipSelect.equipType;
                    equipFake.qualityLevel = equipSelect.qualityLevel;

                    equipSelect.canvas.overrideSorting = true;
                    equipSelect.raycaster.enabled = false;

                    equipSelect.rectTransform.DOScale(1.5f, 0.15f).SetEase(Ease.Linear).OnComplete(delegate
                    {
                        equipSelect.rectTransform.DOScale(1f, 0.25f).SetEase(Ease.Linear);
                    });
                }).OnComplete(delegate
                {
                    currencyEquipInformation.Wearing();

                    equipSelect.canvas.overrideSorting = false;
                    equipSelect.raycaster.enabled = true;

                    parent.DOKill();

                    equipFake.transform.SetParent(inventory.transform);

                    if (currencyEquipInformation.equipInformation != null)
                    {
                        inventory.AddEquip(currencyEquipInformation.equipInformation);

                        currencyEquipInformation.equipInformation.NotWearing();
                    }

                    currencyEquipInformation.equipInformation = equipSelect;

                    inventory.Sort();

                    inventory.SetTweening(equipSelect.equipType, false);
                    parent.DOScale(0.9f, 0.05f).SetEase(Ease.Linear).OnComplete(delegate
                    {
                        parent.DOScale(1f, 0.15f).SetEase(Ease.Linear);
                    });

                    AttackAndHealth targetAttackAndHealth = GetCurrentAttackAndHealth();

                    indexAttackAndHealth.UpdateAttackAndHealth(currentAttackAndHealth.attack, targetAttackAndHealth.attack, currentAttackAndHealth.health, targetAttackAndHealth.health);

                }).SetEase(Ease.InQuad);
            }
            SaveEquips();
        }

        void NotWearing(EquipInformation equipInformation)
        {
            equipInformation.NotWearing();

            inventory.AddEquip(equipInformation);
        }

        void SaveEquips()
        {
            List<Equip> equips = new List<Equip>();

            foreach (EquipInformation equip in equipInfos)
            {
                equips.Add(equip.GetEquip());
            }

            GameManager.instance.Equipments = equips;
        }

        public void ShowEquip()
        {
            if (equipParent.activeSelf) return;

            playerModel.transform.rotation = Quaternion.identity;

            GameController.PlayerType playerType = GameManager.instance.CurrentPlayer;

            PlayerInformation.instance.SelectPlayer(playerType);
            PlayerInformation.instance.ReloadIndexLevel(playerType);

            background.DOKill();

            background.DOAnchorPosX(0f, 0.25f).SetEase(Ease.Linear);

            equipParent.SetActive(true);
            heroParent.SetActive(false);

            inventory.DisableFakeEquips();
        }

        public void ShowHero()
        {
            if (heroParent.activeSelf) return;

            playerModel.transform.rotation = Quaternion.identity;

            background.DOKill();

            background.DOAnchorPosX(-265f, 0.25f).SetEase(Ease.Linear);

            equipParent.SetActive(false);
            heroParent.SetActive(true);

            DisableFrameSelect();

            scrollRectIndex.normalizedPosition = new Vector2(0, 1);
        }

        public void Dance()
        {
            isDancing = !isDancing;

            GetPlayerModelInShop(GetCurrentPlayerModelType()).Dance(isDancing);
        }
    }
    public class Equip
    {
        public EquipType equipType;
        public PlayerInformation.QualityLevel qualityLevel;

        public bool isWeared;

        public Equip(EquipType equipType, PlayerInformation.QualityLevel qualityLevel, bool isWeared)
        {
            this.equipType = equipType;
            this.qualityLevel = qualityLevel;
            this.isWeared = isWeared;
        }
    }


    public class PlayerData
    {
        public int level;
        public bool isUnlocked;
        public int amountPiece;
    }

    public class WeaponData
    {
        public int weaponLevel;
        public int armorLevel;
        public int hatLevel;
        public int necklaceLevel;
        public int ringLevel;
        public int shoeLevel;
    }
}

