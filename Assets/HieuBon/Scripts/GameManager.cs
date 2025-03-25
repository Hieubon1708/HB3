using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using static Hunter.GameController;
using System.Collections.Generic;
using static Hunter.PlayerInformation;
using Newtonsoft.Json.Linq;

namespace Hunter
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public PlayerIndexes characterDataSO;

        private void Awake()
        {
            instance = this;
            PlayerPrefs.DeleteAll();
        }

        public int Level
        {
            get
            {
                return PlayerPrefs.GetInt("Level", 1);
            }
            set
            {
                PlayerPrefs.SetInt("Level", value);
            }
        }

        public int FistTimeShowUIWeapon
        {
            get
            {
                return PlayerPrefs.GetInt("FistTimeShowUIWeapon", 0);
            }
            set
            {
                PlayerPrefs.SetInt("FistTimeShowUIWeapon", value);
            }
        }

        public int Weapon
        {
            get
            {
                return PlayerPrefs.GetInt("Weapon", 4);
            }
            set
            {
                PlayerPrefs.SetInt("Weapon", value);
            }
        }

        public float PercentBlood
        {
            get
            {
                return PlayerPrefs.GetFloat("Blood", 100);
            }
            set
            {
                PlayerPrefs.SetFloat("Blood", value);
            }
        }

        public PlayerType CurrentPlayer
        {
            get
            {
                return (PlayerType)PlayerPrefs.GetInt("CurrentPlayer", (int)PlayerType.Megamon);
            }
            set
            {
                PlayerPrefs.SetInt("CurrentPlayer", (int)value);
            }
        }

        public List<Equip> Equipments
        {
            get
            {
                string txt = PlayerPrefs.GetString("Equipments", string.Empty);
                if (!string.IsNullOrEmpty(txt))
                {
                    return JsonConvert.DeserializeObject<List<Equip>>(txt);
                }

                return new List<Equip>();
            }
            set
            {
                string txt = JsonConvert.SerializeObject(value);
                PlayerPrefs.SetString("Equipments", txt);
            }
        }

        public PlayerData GetPlayerData(GameController.PlayerType playerType)
        {
            string txt = PlayerPrefs.GetString(playerType.ToString(), string.Empty);
            if (!string.IsNullOrEmpty(txt))
            {
                return JsonConvert.DeserializeObject<PlayerData>(txt);
            }

            return new PlayerData();
        }

        
        public void SetPlayerData(PlayerData playerData)
        {
            string txt = JsonConvert.SerializeObject(playerData);
            PlayerPrefs.SetString(CurrentPlayer.ToString(), txt);
        }

        public WeaponData WeaponData
        {
            get
            {
                string txt = PlayerPrefs.GetString("WeaponData", string.Empty);
                if (!string.IsNullOrEmpty(txt))
                {
                    return JsonConvert.DeserializeObject<WeaponData>(txt);
                }

                return new WeaponData();
            }
            set
            {
                string txt = JsonConvert.SerializeObject(value);
                PlayerPrefs.SetString(CurrentPlayer.ToString(), txt);
            }
        }

        /*public Dictionary<CharacterType, CharacterSaveData> CharacterSaveDatas
        {
            get
            {
                string txt = PlayerPrefs.GetString("CharacterSaveDatas", string.Empty);
                if (!string.IsNullOrEmpty(txt))
                {
                    return JsonConvert.DeserializeObject<Dictionary<CharacterType, CharacterSaveData>>(txt);
                }

                return new Dictionary<CharacterType, CharacterSaveData>();
            }
            set
            {
                string txt = JsonConvert.SerializeObject(value);
                PlayerPrefs.SetString("CharacterSaveDatas", txt);
            }
        }

        public int GetCharaterHP(CharacterType character, int level)
        {
            foreach (var characterData in characterDataSO.characterDatas)
            {
                if (characterData.character == character)
                {
                    if (level == 1)
                    {
                        return characterData.baseHP;
                    }
                    else
                    {
                        return Mathf.RoundToInt(characterData.baseHP * (1 + 0.12f * level));
                    }
                }
            }

            return 0;
        }*/
    }
}
