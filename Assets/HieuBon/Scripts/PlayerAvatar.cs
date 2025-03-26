using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class PlayerAvatar : MonoBehaviour
    {
        public static PlayerAvatar instance;

        Button button;

        public GameObject panelAvatar;
        public GameObject panelNameEdit;

        public Image frame;
        public Image avatar;

        public TextMeshProUGUI playerName;

        public InputField inputField;

        public AvatarFrameInfo[] avatarFrameInfos;
        public AvatarInfo[] avatarInfos;

        public Transform containerAvatar;
        public Transform containerAvatarFrame;

        public Sprite[] spritesAvatar;
        public Sprite[] spritesFrame;

        public GameObject scrollAvatar;
        public GameObject scrollFrame;

        public TextMeshProUGUI attack;
        public TextMeshProUGUI damage;

        public void Start()
        {
            playerName.text = GameManager.instance.PlayerName;

            FrameUpdate();
            AvatarUpdate();
        }

        public void AddAvatar(AvatarType avatarType)
        {
            List<AvatarType> avatarsUnlock = GameManager.instance.AvatarsUnlock;
            avatarsUnlock.Add(avatarType);

            GameManager.instance.AvatarsUnlock = avatarsUnlock;

            AvatarUpdate();
        }

        public void AddFrame(FrameType frameType)
        {
            List<FrameType> framesUnlock = GameManager.instance.FramesUnlock;
            framesUnlock.Add(frameType);

            GameManager.instance.FramesUnlock = framesUnlock;

            FrameUpdate();
        }

        void AvatarUpdate()
        {
            AvatarType currentAvatar = GameManager.instance.CurrentAvatar;

            List<AvatarType> avatarsUnlock = GameManager.instance.AvatarsUnlock;

            foreach (var avatar in avatarInfos)
            {
                avatar.IsUnlock(avatarsUnlock.Contains(avatar.avatarType));
                avatar.IsUse(avatar.avatarType == currentAvatar);
            }
        }

        void UpdateIndex()
        {

        }

        void FrameUpdate()
        {
            FrameType currentFrame = GameManager.instance.CurrentFrame;

            List<FrameType> framesUnlock = GameManager.instance.FramesUnlock;

            foreach (var avatarFrame in avatarFrameInfos)
            {
                avatarFrame.IsUnlock(framesUnlock.Contains(avatarFrame.frameType));
                avatarFrame.IsUse(avatarFrame.frameType == currentFrame);
            }
        }

        public void SelectAvatar(AvatarType avatarType)
        {
            avatar.sprite = spritesAvatar[(int)avatarType];
        }

        public void SelectFrame(FrameType frameType)
        {
            frame.sprite = spritesFrame[(int)frameType];
        }

        public enum FrameType
        {
            Frame1, Frame2
        }

        public enum AvatarType
        {
            Avatar1, Avatar2
        }

        public void Awake()
        {
            instance = this;

            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);

            avatarFrameInfos = containerAvatarFrame.GetComponentsInChildren<AvatarFrameInfo>();
            avatarInfos = containerAvatar.GetComponentsInChildren<AvatarInfo>();
        }

        void OnClick()
        {
            ShowAvatar();
            ShowPopup();
        }

        void ShowPopup()
        {
            panelAvatar.SetActive(true);
        }

        public void EditName()
        {
            panelNameEdit.SetActive(true);
        }

        public void SubmitNameChange()
        {
            GameManager.instance.PlayerName = inputField.text;
            playerName.text = inputField.text;
            panelNameEdit.SetActive(false);
            PlayerInformation.instance.textNofication.ShowText("Your name has been changed");
        }

        public void ShowAvatar()
        {
            scrollAvatar.SetActive(true);
            scrollFrame.SetActive(false);
        }

        public void ShowFrame()
        {
            scrollAvatar.SetActive(false);
            scrollFrame.SetActive(true);
        }

        public void HidePopup()
        {
            panelAvatar.SetActive(false);
        }
    }
}