using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    [SerializeField] private GameObject background;
    [SerializeField] private Transform shape;
    [SerializeField] private GameObject raycast;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    public void Fade(Action onDoneFadeIn = null, Action onDoneFadeOut = null, float delayFadeIn = 0f, float delayFadeOut = 0.5f)
    {
        raycast.SetActive(true);
        background.SetActive(true);
        shape.localScale = Vector3.one * 30f;

        shape.DOScale(0f, 0.5f).SetEase(Ease.OutQuad).SetDelay(delayFadeIn).OnComplete(() =>
        {
            onDoneFadeIn?.Invoke();
            shape.DOScale(30f, 0.5f).SetEase(Ease.InQuad).SetDelay(delayFadeOut).OnComplete(() =>
            {
                onDoneFadeOut?.Invoke();

                background.SetActive(false);
                raycast.SetActive(false);
            }).SetUpdate(true);
        }).SetUpdate(true);
    }

    public void FadeIn(Action onDone = null)
    {
        raycast.SetActive(true);
        background.SetActive(true);
        shape.localScale = Vector3.one * 30f;

        shape.DOScale(0f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            onDone?.Invoke();
        }).SetUpdate(true);
    }

    public void FadeOut(Action onDone = null)
    {
        raycast.SetActive(true);
        background.SetActive(true);
        shape.localScale = Vector3.zero;

        shape.DOScale(30f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.5f).OnComplete(() =>
        {
            background.SetActive(false);
            raycast.SetActive(false);
            onDone?.Invoke();
        }).SetUpdate(true);
    }

    public void LoadScene(int id, Action onDoneFadeIn = null, bool isWinTangle = false)
    {
        ACEPlay.Bridge.BridgeController.instance.PlayCount++;

        FadeIn(() =>
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                onDoneFadeIn?.Invoke();
                StartCoroutine(LoadAsyncGame(id));
            });

            UnityEvent onDone = new UnityEvent();
            onDone.AddListener(() =>
            {
                ACEPlay.Bridge.BridgeController.instance.PlayCount = 0;
            });

            ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("tangle_win", e, onDone);

            //if (isWinTangle)
            //{
                //if (ACEPlay.Bridge.BridgeController.instance.IsInterReady())
                //{
                //    ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("tangle_win", e);
                //}
                //else
                //{
                //    e.Invoke();
                //    ACEPlay.Bridge.BridgeController.instance.ShowBannerCollapsible();
                //}
            //}
            //else
            //{
            //    e.Invoke();
            //}
            
        });
    }

    public void LoadSceneOnStart(int id, Action onDoneFadeIn = null)
    {
        raycast.SetActive(true);
        background.SetActive(true);
        shape.localScale = Vector3.zero;
        onDoneFadeIn?.Invoke();
        StartCoroutine(LoadAsyncGame(id));
    }

    IEnumerator LoadAsyncGame(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            float progress = operation.progress;
            if (progress == 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        FadeOut();
    }
}
