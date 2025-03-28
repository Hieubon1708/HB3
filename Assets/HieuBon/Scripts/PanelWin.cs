using ACEPlay.Bridge;
using DG.Tweening;
using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hunter
{
    public class PanelWin : MonoBehaviour
    {
        public TextMeshProUGUI textDollar;
        public TextMeshProUGUI x;

        public Image iconDollar;

        public RectTransform[] dollarChildren;
        public RectTransform arrow;

        public Vector2[] dirs;
        public float[] distances;
        int mul;

        public RectTransform progress;
        public GameObject objTangle;
        public GameObject objStealthNormal;
        public GameObject objStealthElite;
        public GameObject objStealthBonus;
        public GameObject objStealthBoss;
        public Slider levelProgress;
        public HorizontalLayoutGroup gridProgress;
        private List<GameObject> spawnedStages = new List<GameObject>();

        int dollarAward = 30;

        public void OnEnable()
        {
            ACEPlay.Native.NativeAds.instance.DisplayNativeAds(true);
            BridgeController.instance.ShowBannerCollapsible();
            BridgeController.instance.ShowMRECs();
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.win, 0);
            if (gridProgress.transform.childCount > 0)
            {
                for (int i = 0; i < gridProgress.transform.childCount; i++)
                {
                    Destroy(gridProgress.transform.GetChild(i).gameObject);
                }
            }
            spawnedStages.Clear();
            DOVirtual.DelayedCall(0.1f, delegate
            {
                ShowLevelProgress();
            });
            textDollar.text = 0.ToString();
            textDollar.color = Color.white;
            iconDollar.enabled = true;
        }

        public void OnDisable()
        {
            arrow.DOKill();
            BridgeController.instance.HideMRECs();
            ACEPlay.Native.NativeAds.instance.DisplayNativeAds(false);
            BridgeController.instance.HideBannerCollapsible();
        }

        public void Watch()
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                dollarAward *= mul;
                textDollar.text = dollarAward.ToString();
                TextDollarReduce();
            });
            BridgeController.instance.ShowRewarded("x" + mul + "dollar", e);
        }

        public void TextDollarIncrease()
        {
            textDollar.transform.DOScale(1.35f, 0.1f).SetEase(Ease.Linear);
            textDollar.transform.DOScale(1f, 0.1f).SetEase(Ease.Linear).SetDelay(0.5f);
            DOVirtual.Int(0, dollarAward, 0.25f, (d) =>
            {
                textDollar.text = d.ToString();
            });
        }

        public void TextDollarReduce()
        {
            UIInGame.instance.layerCover.raycastTarget = true;
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.button, 50);
            arrow.DOKill();
            /*List<float> tempDistances = new List<float>(distances);
            List<float> randomDistances = new List<float>();
            while (tempDistances.Count > 0)
            {
                int indexRandom = Random.Range(0, tempDistances.Count);
                randomDistances.Add(tempDistances[indexRandom]);
                tempDistances.RemoveAt(indexRandom);
            }*/
            //EventManager.SetDataGroup(EventVariables.UpdateMission, MissionType.CollectMoney, dollarAward);
            //EventManager.EmitEvent(EventVariables.UpdateMission);
            textDollar.transform.DOScale(1.35f, 0.1f).SetEase(Ease.Linear);
            textDollar.transform.DOScale(1f, 0.1f).SetEase(Ease.Linear).SetDelay(0.5f);
            DOVirtual.Int(dollarAward, 0, 0.25f, (c) =>
            {
                textDollar.text = c.ToString();
            }).OnComplete(delegate
            {
                //UIEconomy.instance.AddCash(dollarAward, iconDollar.transform);
                iconDollar.enabled = false;
                textDollar.DOFade(0f, 0.1f).SetEase(Ease.Linear);
                /*for (int i = 0; i < dollarChildren.Length; i++)
                {
                    int index = i;
                    dollarChildren[index].DOLocalMove(dollarChildren[index].anchoredPosition + randomDistances[index] * dirs[index] * 35, 0.25f).OnComplete(delegate
                    {
                        dollarChildren[index].DOMove(dollarTarget.position, 0.25f).SetEase(Ease.Linear).SetDelay(Random.Range(0.05f, 0.35f));
                    });
                }*/
            });
            DOVirtual.DelayedCall(2.75f, delegate
            {
                UIInGame.instance.ChangeMap();
            });
        }

        public void LaunchProgress()
        {
            arrow.DOLocalMoveX(380f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).OnUpdate(delegate
            {
                if (arrow.anchoredPosition.x >= -86 && arrow.anchoredPosition.x <= 86)
                {
                    if (mul != 5) AudioController.instance.PlaySoundNVibrate(AudioController.instance.barXReward, 0);
                    mul = 5;
                    x.text = "x5";
                }
                else if (arrow.anchoredPosition.x >= -202 && arrow.anchoredPosition.x < -86 || arrow.anchoredPosition.x <= 202 && arrow.anchoredPosition.x > 86)
                {
                    if (mul != 4) AudioController.instance.PlaySoundNVibrate(AudioController.instance.barXReward, 0);
                    mul = 4;
                    x.text = "x4";
                }
                else if (arrow.anchoredPosition.x >= -321 && arrow.anchoredPosition.x < -202 || arrow.anchoredPosition.x <= 321 && arrow.anchoredPosition.x > 202)
                {
                    if (mul != 3) AudioController.instance.PlaySoundNVibrate(AudioController.instance.barXReward, 0);
                    mul = 3;
                    x.text = "x3";
                }
                else
                {
                    if (mul != 2) AudioController.instance.PlaySoundNVibrate(AudioController.instance.barXReward, 0);
                    mul = 2;
                    x.text = "x2";
                }
            });
        }

        public void ShowLevelProgress()
        {
            /*int chapter = Mathf.Min(UIController.instance.gamePlay.tempChapter, Manager.instance.levelDataSO.chapters.Count);
            int stage = UIController.instance.gamePlay.tempStage;

            Chapter currentChapter = Manager.instance.levelDataSO.chapters[chapter - 1];

            levelProgress.minValue = 1;
            levelProgress.maxValue = currentChapter.stages.Count;
            levelProgress.value = stage;

            float sizeX = Mathf.Min(currentChapter.stages.Count + 1, 6) * 100f;
            progress.sizeDelta = new Vector2(sizeX, progress.sizeDelta.y);

            gridProgress.spacing = sizeX / (currentChapter.stages.Count - 1);

            foreach (var stageType in currentChapter.stages)
            {
                GameObject newObj;
                switch (stageType)
                {
                    case StageType.Tangle:
                        newObj = objTangle.Spawn(gridProgress.transform);
                        newObj.transform.localScale = Vector3.one;
                        spawnedStages.Add(newObj);
                        break;
                    case StageType.StealthNormal:
                        newObj = objStealthNormal.Spawn(gridProgress.transform);
                        newObj.transform.localScale = Vector3.one;
                        spawnedStages.Add(newObj);
                        break;
                    case StageType.StealthElite:
                        newObj = objStealthElite.Spawn(gridProgress.transform);
                        newObj.transform.localScale = Vector3.one;
                        spawnedStages.Add(newObj);
                        break;
                    case StageType.StealthBoss:
                        newObj = objStealthBoss.Spawn(gridProgress.transform);
                        newObj.transform.localScale = Vector3.one;
                        spawnedStages.Add(newObj);
                        break;
                    case StageType.StealthBonus:
                        newObj = objStealthBonus.Spawn(gridProgress.transform);
                        newObj.transform.localScale = Vector3.one;
                        spawnedStages.Add(newObj);
                        break;
                }
            }

            for (int i = 0; i < spawnedStages.Count; i++)
            {
                spawnedStages[i].transform.localScale = Vector3.one;

                if (i < (stage - 1))
                {
                    spawnedStages[i].GetComponentInChildren<Image>().color = Color.green;
                }
                else if (i == (stage - 1))
                {
                    spawnedStages[i].GetComponentInChildren<Image>().color = Color.yellow;
                }
                else
                {
                    spawnedStages[i].GetComponentInChildren<Image>().color = Color.white;
                }
            }

            spawnedStages[stage - 1].transform.DOScale(1.2f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);*/
        }
    }
}