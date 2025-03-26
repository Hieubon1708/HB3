using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using static Hunter.GameController;
using System.Collections.Generic;
using static Hunter.PlayerInformation;
using Newtonsoft.Json.Linq;
using System.Text;

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

        public bool IsActiveMusic
        {
            get
            {
                return PlayerPrefs.GetInt("Music", 1) == 1;
            }
            set
            {
                //AudioController.instance.IsPlayMusic();
                PlayerPrefs.SetInt("Music", value ? 1 : 0);
            }
        }

        public bool IsAtiveSound
        {
            get
            {
                return PlayerPrefs.GetInt("Sound", 1) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("Sound", value ? 1 : 0);
            }
        }

        public bool IsActiveVibrate
        {
            get
            {
                return PlayerPrefs.GetInt("Vibrate", 1) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("Vibrate", value ? 1 : 0);
            }
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
        
        public PlayerAvatar.AvatarType CurrentAvatar
        {
            get
            {
                return (PlayerAvatar.AvatarType)PlayerPrefs.GetInt("CurrentAvatar", (int)PlayerAvatar.AvatarType.Avatar1);
            }
            set
            {
                PlayerPrefs.SetInt("CurrentAvatar", (int)value);
            }
        }
        
        public PlayerAvatar.FrameType CurrentFrame
        {
            get
            {
                return (PlayerAvatar.FrameType)PlayerPrefs.GetInt("CurrentFrame", (int)PlayerAvatar.FrameType.Frame1);
            }
            set
            {
                PlayerPrefs.SetInt("CurrentFrame", (int)value);
            }
        }

        public string PlayerName
        {
            get
            {
                if (!PlayerPrefs.HasKey("PlayerName"))
                {
                    return GenerateRandomString(7);
                }
                return PlayerPrefs.GetString("PlayerName");
            }
            set
            {
                PlayerPrefs.SetString("PlayerName", value);
            }
        }

        public List<PlayerAvatar.AvatarType> AvatarsUnlock
        {
            get
            {
                string txt = PlayerPrefs.GetString("AvatarsUnlock", string.Empty);
                if (!string.IsNullOrEmpty(txt))
                {
                    return JsonConvert.DeserializeObject<List<PlayerAvatar.AvatarType>>(txt);
                }

                List<PlayerAvatar.AvatarType> avatars = new List<PlayerAvatar.AvatarType>();

                avatars.Add(PlayerAvatar.AvatarType.Avatar1);

                return avatars;
            }
            set
            {
                string txt = JsonConvert.SerializeObject(value);
                PlayerPrefs.SetString("AvatarsUnlock", txt);
            }
        }

        public List<PlayerAvatar.FrameType> FramesUnlock
        {
            get
            {
                string txt = PlayerPrefs.GetString("FramesUnlock", string.Empty);
                if (!string.IsNullOrEmpty(txt))
                {
                    return JsonConvert.DeserializeObject<List<PlayerAvatar.FrameType>>(txt);
                }
                List<PlayerAvatar.FrameType> frames = new List<PlayerAvatar.FrameType>();

                frames.Add(PlayerAvatar.FrameType.Frame1);

                return frames;
            }
            set
            {
                string txt = JsonConvert.SerializeObject(value);
                PlayerPrefs.SetString("FramesUnlock", txt);
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
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[Random.Range(0, chars.Length)]);
            }

            return stringBuilder.ToString();
        }
    }
}
