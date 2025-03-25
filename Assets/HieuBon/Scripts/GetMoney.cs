using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Hunter
{
    public class GetMoney : MonoBehaviour
    {
        int currentTook;
        public TextMeshProUGUI amount;
        public CanvasGroup canvasGroup;

        public void TakeOn(int coin)
        {
            CancelInvoke(nameof(TakeOff));
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, 0.5f);
            currentTook += coin;
            amount.text = "+" + currentTook;
            Invoke(nameof(TakeOff), 1.5f);
        }

        public void TakeOff()
        {
            canvasGroup.DOFade(0f, 0.5f).OnComplete(delegate
            {
                currentTook = 0;
            });
        }

        public void LateUpdate()
        {
            transform.LookAt(new Vector3(transform.position.x, UIInGame.instance.virtualCam.cam.transform.position.y, UIInGame.instance.virtualCam.cam.transform.position.z));
        }
    }
}
