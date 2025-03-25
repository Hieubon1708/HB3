using ACEPlay.Bridge;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hunter
{
    public class UIInGame : MonoBehaviour
    {
        public static UIInGame instance;

        [HideInInspector]
        public GamePlay gamePlay;
        public Cam virtualCam;
        public Animation glow;
        public Animation camAni;
        public Image layerCover;
        int[] pieces = new int[] { 6, 4, 0, 15, 3, 4, 0, 7, 4, 0, 0, 0, 4 };
        public Sprite[] iconCharacters;
        public Image[] iconCharacter;
        public RectTransform[] piece;
        public TextMeshProUGUI[] amountPiece;

        private void Awake()
        {
            instance = this;

            gamePlay = GetComponentInChildren<GamePlay>();
        }

        private void Start()
        {
            //EventManager.StartListening(EventVariables.ChooseEquipment, PlayerController.instance.playerTouchMovement.Play);
        }

        public void Lose()
        {
            gamePlay.Lose();
            layerCover.raycastTarget = true;
            DOVirtual.DelayedCall(2.5f, delegate
            {
                layerCover.raycastTarget = false;
                //gamePlay.panelLose.SetActive(true);
            });
        }

        public void Win(System.Action onDone, float timeDelay)
        {
            /*StageType stageType = GetStageType();

            int[] piece = new int[2];
            if (stageType != StageType.StealthBonus)
            {
                if (Manager.instance.Chapter <= 2)
                {
                    piece[0] = pieces[GameManager.instance.LevelStealk - 1];
                    this.piece[0].gameObject.SetActive(true);
                    iconCharacter[0].sprite = iconCharacters[(int)Character.PickyPiggy];
                    CharacterManager.instance.AddPuzzle(Character.PickyPiggy, piece[0]);
                    if (piece[0] == 0)
                    {
                        int randomCharacter = Random.Range(0, GameController.instance.preplayers.Length);
                        Character character = GameController.instance.GetCharacter(randomCharacter);
                        piece[0] = Random.Range(2, 5);
                        iconCharacter[0].sprite = iconCharacters[(int)character];
                        CharacterManager.instance.AddPuzzle(character, piece[0]);
                    }
                }
                else if (Manager.instance.Chapter > 2 && Manager.instance.Chapter <= 4)
                {
                    piece[0] = pieces[GameManager.instance.LevelStealk - 1];
                    this.piece[0].gameObject.SetActive(true);
                    iconCharacter[0].sprite = iconCharacters[(int)Character.Hoppy];
                    CharacterManager.instance.AddPuzzle(Character.Hoppy, piece[0]);
                    if (piece[0] == 0)
                    {
                        int randomCharacter = Random.Range(0, GameController.instance.preplayers.Length);
                        Character character = GameController.instance.GetCharacter(randomCharacter);
                        piece[0] = Random.Range(2, 5);
                        iconCharacter[0].sprite = iconCharacters[(int)character];
                        CharacterManager.instance.AddPuzzle(character, piece[0]);
                    }

                }
                else
                {
                    Debug.Log(">>>>> Chapter 4");
                    int randomO = Random.Range(0, 2);

                    Dictionary<Character, CharacterSaveData> characterSaveDatas = CharacterManager.instance.CharacterSaveDatas;
                    List<int> nums = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

                    for (int i = 0; i <= randomO; i++)
                    {
                        this.piece[i].gameObject.SetActive(true);
                        int randomCharacter = Random.Range(0, nums.Count);
                        Character character = GameController.instance.GetCharacter(nums[randomCharacter]);
                        Debug.Log(character.ToString());

                        if (stageType == StageType.StealthNormal || stageType == StageType.StealthElite)
                        {
                            if (randomO == 0)
                            {
                                if (!characterSaveDatas[character].unlocked) piece[i] = Random.Range(5, 8);
                                else piece[i] = Random.Range(1, 3);
                            }
                            else if (randomO == 1)
                            {
                                if (i == 0)
                                {
                                    int total = Random.Range(7, 11);
                                    piece[0] = Random.Range(4, total / 2 + 1);
                                    piece[1] = total - piece[0];
                                }
                                if (characterSaveDatas[character].unlocked) piece[i] = 1;
                            }
                        }
                        else if (stageType == StageType.StealthBoss)
                        {
                            if (randomO == 0)
                            {
                                if (!characterSaveDatas[character].unlocked) piece[i] = Random.Range(7, 11);
                                else piece[i] = Random.Range(2, 4);
                            }
                            else if (randomO == 1)
                            {
                                if (i == 0)
                                {
                                    int total = Random.Range(9, 12);
                                    piece[0] = Random.Range(5, total / 2 + 1);
                                    piece[1] = total - piece[0];
                                }
                                if (characterSaveDatas[character].unlocked) piece[i] = Random.Range(1, 3);
                            }
                        }
                        iconCharacter[i].sprite = iconCharacters[(int)character];
                        CharacterManager.instance.AddPuzzle(GameController.instance.GetCharacter(randomCharacter), piece[i]);

                        nums.Remove(nums[randomCharacter]);
                    }
                }
            }

            PlayerController.instance.player.SetHp();
            EventManager.EmitEvent(EventVariables.StopCountDownShowAdsInGame);
            BridgeController.instance.LogLevelCompleteWithParameter("stealth", GameManager.instance.LevelStealk);
            Manager.instance.Stage++;

            if (stageType == StageType.StealthBonus)
            {
                onDone.Invoke();
                return;
            }
            BridgeController.instance.Debug_Log("Piece " + piece);
            ShowPiece(onDone, piece[0], piece[1], timeDelay);*/
        }

        void ShowPiece(System.Action onDone, int amountPiece1, int amountPiece2, float timeDelay)
        {
            amountPiece[0].text = "";
            amountPiece[1].text = "";

            piece[0].DOScale(1f, 0.25f).SetEase(Ease.OutBack).OnComplete(delegate
            {
                amountPiece[0].transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
                DOVirtual.Int(0, amountPiece1, 0.25f, (v) =>
                {
                    amountPiece[0].text = "X" + v;
                }).OnComplete(delegate
                {
                    if (amountPiece2 == 0)
                    {
                        piece[0].DOScale(0f, 0.25f).SetEase(Ease.InBack).SetDelay(0.35f).OnComplete(delegate
                        {
                            piece[0].gameObject.SetActive(false);
                            onDone.Invoke();
                        });
                    }
                });
            }).SetDelay(timeDelay).OnStart(delegate
            {
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.pieceReward, 0);
            });

            if (amountPiece2 == 0) return;
            piece[1].DOScale(1f, 0.25f).SetEase(Ease.OutBack).OnComplete(delegate
            {
                amountPiece[1].transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
                DOVirtual.Int(0, amountPiece2, 0.25f, (v) =>
                {
                    amountPiece[1].text = "X" + v;
                }).OnComplete(delegate
                {
                    piece[0].DOScale(0f, 0.25f).SetEase(Ease.InBack).SetDelay(0.35f);
                    piece[1].DOScale(0f, 0.25f).SetEase(Ease.InBack).SetDelay(0.35f).OnComplete(delegate
                    {
                        piece[0].gameObject.SetActive(false);
                        piece[1].gameObject.SetActive(false);
                        onDone.Invoke();
                    });
                });
            }).SetDelay(timeDelay + 0.5f).OnStart(delegate
            {
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.pieceReward, 0);
            });
        }

        public void Play()
        {
            //EventManager.EmitEvent(EventVariables.CountDownShowAdsInGame);
            LevelController.instance.Play();
            virtualCam.CamStartZoom();
            gamePlay.Play();
            if (boss != null)
            {
                GameObject h = boss.transform.Find("Health").gameObject;
                h.SetActive(true);
            }
        }

        public void LoadUI(bool isElevator)
        {
            gamePlay.LoadUI();
            layerCover.raycastTarget = true;
            virtualCam.ResetCam();
            PlayerController.instance.playerTouchMovement.HideTouch();

            /*StageType stageType = GetStageType();
            if (!isElevator && stageType != StageType.StealthBoss)
            {
                UIManager.instance.ShowUIHome();
                if (GameManager.instance.FistTimeShowUIWeapon == 0)
                {
                    UIManager.instance.UILevelProgress.Hide();
                    DOVirtual.DelayedCall(0.5f, delegate
                    {
                        UIManager.instance.ShowUIChooseEquipment();
                    }).SetUpdate(true);
                }
                else
                {
                    PlayerController.instance.handTutorial.PlayHand();
                }
            }
            else
            {
                UIManager.instance.UIHome.Hide();
            }
            gamePlay.frameRemainingEnemy.SetActive(stageType != StageType.StealthBonus);*/

            PlayerController.instance.handTutorial.PlayHand();
        }

        /*public StageType GetStageType()
        {
            int chapter = Mathf.Min(Manager.instance.Chapter - 1, Manager.instance.levelDataSO.chapters.Count - 1);
            int stage = Manager.instance.Stage - 1;
            return Manager.instance.levelDataSO.chapters[chapter].stages[stage];
        }*/

        public void ChangeMap()
        {
            /*StageType stageType = GetStageType();
            if (stageType == StageType.Tangle)
            {
                GameManager.instance.PercentBlood = 100;
                GameManager.instance.Weapon = 4;
                GameManager.instance.FistTimeShowUIWeapon = 0;
                FadeManager.instance.BackHome(true, true);
            }
            else
            {
                FadeManager.instance.FadeIn(() =>
                {
                    BridgeController.instance.PlayCount++;

                    UnityEvent e = new UnityEvent();
                    e.AddListener(() =>
                    {
                        GameController.instance.LoadLevel(GameManager.instance.Level);

                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            FadeManager.instance.FadeOut();
                        });
                    });

                    UnityEvent onDone = new UnityEvent();
                    onDone.AddListener(() =>
                    {
                        BridgeController.instance.PlayCount = 0;
                    });

                    BridgeController.instance.ShowInterstitial("stealth_win", e, onDone);

                    //if (BridgeController.instance.IsInterReady())
                    //{
                    //    BridgeController.instance.ShowInterstitial("stealth_win", e);
                    //}
                    //else
                    //{
                    //    e.Invoke();
                    //    BridgeController.instance.ShowBannerCollapsible();
                    //}
                });
            }*/
        }

        public void BossEnd()
        {
            System.Action action = () =>
            {
                layerCover.raycastTarget = false;
                gamePlay.panelWin.SetActive(true);
            };
            Win(action, 1.5f);
            PlayerController.instance.Win();
            GameManager.instance.PercentBlood = 100;
            GameManager.instance.Weapon = 4;
            GameManager.instance.FistTimeShowUIWeapon = 0;
        }

        Bot boss;

        public IEnumerator BossIntro()
        {
            layerCover.raycastTarget = true;
            PlayerController.instance.handTutorial.StopHand();
            boss = LevelController.instance.GetBoss();
            yield return new WaitForSeconds(0.5f);
            CinemachineVirtualCamera cam = boss.GetComponentInChildren<CinemachineVirtualCamera>();
            cam.Priority = 100;
            yield return new WaitForSeconds(2f);
            cam.Priority = 1;
            yield return new WaitForSeconds(1f);
            DOVirtual.DelayedCall(0.5f, delegate
            {
                layerCover.raycastTarget = false;
            });
            //UIManager.instance.ShowUIHome();
            PlayerController.instance.handTutorial.PlayHand();
        }

        public void HitEffect()
        {
            ResetHitEffect();
            glow.Play();
        }

        public void HitCancel()
        {
            glow.Stop();
        }

        void ResetHitEffect()
        {
            glow.Stop();
        }
    }
}