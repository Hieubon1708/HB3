using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HieuBon
{
    public class UIAvatarFrameInfo : MonoBehaviour
    {
        public PlayerAvatar.FrameType frameType;

        Button button;

        public GameObject frameUnlock;
        public GameObject frameUse;

        public void Awake()
        {
            button = GetComponent<Button>();
        }

        public void IsUnlock(bool isUnlock)
        {
            frameUnlock.SetActive(!isUnlock);
        }
        
        public void IsUse(bool isUse)
        {
            frameUse.SetActive(isUse);
        }

        void OnClick()
        {
            if (frameUnlock.activeSelf) return;


        }
    }
}
