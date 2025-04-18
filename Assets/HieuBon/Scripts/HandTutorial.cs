using DG.Tweening;
using UnityEngine;

namespace Hunter
{
    public class HandTutorial : MonoBehaviour
    {
        public RectTransform hand;
        public RectTransform[] path;
        public CanvasGroup canvasGroup;
        Vector3[] paths;

        public void Awake()
        {
            DOVirtual.DelayedCall(0.5f, delegate
            {
                if (paths == null || paths.Length == 0)
                {
                    paths = new Vector3[path.Length];
                    for (int i = 0; i < paths.Length; i++)
                    {
                        paths[i] = path[i].position;
                    }
                }
            });
        }

        public void PlayHand()
        {
            float time = 0;

            if (paths == null || paths.Length == 0)
            {
                time = 0.5f;

                paths = new Vector3[path.Length];
                for (int i = 0; i < paths.Length; i++)
                {
                    paths[i] = path[i].position;
                }
            }

            DOVirtual.DelayedCall(time, delegate
            {
                hand.position = paths[0];
                hand.DOPath(paths, 3f, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
                Show();
            });
        }

        public void Hide()
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 0f;
        }

        public void Show()
        {
            UIInGame.instance.layerCover.raycastTarget = false;
            canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear);
        }

        public void StopHand()
        {
            hand.DOKill();
            canvasGroup.DOKill();
            canvasGroup.alpha = 0f;
        }
    }
}
