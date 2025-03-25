using TigerForge;
using UnityEngine;

namespace Hunter
{
    public class QC : MonoBehaviour
    {
        public GameObject[] canvases;
        public CanvasGroup touch;
        bool isActive;
        int index = 1;

        public void Start()
        {
            //EventManager.StartListening(EventVariables.ShowHideUI, SHUI);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                for (int i = 0; i < canvases.Length; i++)
                {
                    canvases[i].SetActive(isActive);
                }
                touch.alpha = isActive ? 1 : 0.0000001f;
                //UIEconomy.instance.cashContainer.SetActive(isActive);
                isActive = !isActive;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                GameManager.instance.Weapon = index;
                index++;
                if (index == PlayerController.instance.weaponEquip.preWeapons.Length) index = 1;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                UIInGame.instance.Win(null, 0);
                UIInGame.instance.ChangeMap();
            }
        }

        public void SHUI()
        {
            /*for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].SetActive(UIManager.instance.isShowingUI);
            }
            touch.alpha = UIManager.instance.isShowingUI ? 1 : 0.0000001f;
            UIEconomy.instance.cashContainer.SetActive(UIManager.instance.isShowingUI);*/
        }
    }
}
