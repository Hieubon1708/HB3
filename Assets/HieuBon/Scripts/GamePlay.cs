using ACEPlay.Bridge;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Hunter
{
    public class GamePlay : MonoBehaviour
    {
        public TextMeshProUGUI textRemainingEnemy;
        public CanvasGroup remainingEnemy;
        public GameObject iconComplete;
        public GameObject panelLose;
        public GameObject panelWin;
        public GameObject frameRemainingEnemy;
        public int tempChapter;
        public int tempStage;
        public int tempWeapon;
        //public StageType tempStageType;

        public void Play()
        {
            //tempStageType = UIController.instance.GetStageType();
            //tempChapter = Manager.instance.Chapter;
            //tempStage = Manager.instance.Stage;
            iconComplete.SetActive(false);
            remainingEnemy.DOFade(1f, 0.5f).SetEase(Ease.Linear);
        }

        public void UpdateRemainingEnemy()
        {
            if (LevelController.instance.bots.Count > 0) textRemainingEnemy.text = LevelController.instance.bots.Count.ToString();
            else
            {
                EndDoor endDoor = LevelController.instance.GetComponentInChildren<EndDoor>();
                if (endDoor != null) endDoor.OpenDoor();
                textRemainingEnemy.text = "";
                iconComplete.SetActive(true);
            }
            textRemainingEnemy.DOKill();
            textRemainingEnemy.transform.DOScale(1.25f, 0.1f).OnComplete(delegate
            {
                textRemainingEnemy.transform.DOScale(1f, 0.25f);
            });
        }

        public void Lose()
        {
            //EventManager.EmitEvent(EventVariables.StopCountDownShowAdsInGame);
            BridgeController.instance.LogLevelFailWithParameter("stealk", GameManager.instance.Level);
            tempWeapon = GameManager.instance.Weapon;
            //Manager.instance.Stage = 1;
            GameManager.instance.PercentBlood = 0;
            GameManager.instance.Weapon = 4;
            //Manager.instance.RescuedCharacter = new List<Character>();
        }

        public void LoseProgress()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.button, 50);
            GameManager.instance.FistTimeShowUIWeapon = 0;
            //FadeManager.instance.BackHome(false, true);
        }

        public void Revival()
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.button, 50);
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                GameManager.instance.Weapon = tempWeapon;
                /*Manager.instance.Stage = tempStage;
                FadeManager.instance.FadeIn(delegate
                {
                    GameController.instance.LoadLevel(GameManager.instance.Level);
                    FadeManager.instance.FadeOut();
                });*/
            });
            BridgeController.instance.ShowRewarded("revival", e);
        }

        public void UpdateCoin(int coin)
        {
            if (coin == 0) return;
            coin = (int)(coin * 1);
            //EventManager.SetDataGroup(EventVariables.UpdateMission, MissionType.CollectMoney, coin);
            //EventManager.EmitEvent(EventVariables.UpdateMission);
            BridgeController.instance.LogEarnCurrency("money", coin, "kill_and_ground_stealk");
            PlayerController.instance.takeMoney.TakeOn(coin);
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.coin, 0);
            //UIEconomy.instance.AddCash(coin);
        }

        public void ChangeWeapon(int index)
        {
            /*GameController.WeaponType weaponType = GameController.WeaponType.Knife;
            if (index == 1) weaponType = GameController.WeaponType.Gun;
            for (int i = 0; i < LevelController.instance.players.Count; i++)
            {
                PlayerController.instance.weaponEquip.Equip(LevelController.instance.players[i], weaponType);
            }*/
        }

        public void LoadUI()
        {
            panelLose.SetActive(false);
            remainingEnemy.alpha = 0;
            UpdateRemainingEnemy();
        }
    }
}
