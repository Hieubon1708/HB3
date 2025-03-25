using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class BotHealth : MonoBehaviour
    {
        public Image healthDamagerBar;
        public Image healthBar;
        public Bot bot;
        public CanvasGroup canvasGroup;

        public void SubtractHp()
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 1;
            healthBar.DOComplete();
            healthDamagerBar.DOComplete();
            healthBar.DOFillAmount((float)bot.hp / bot.startHp, 0.25f);
            healthDamagerBar.DOFillAmount((float)bot.hp / bot.startHp, 0.25f).SetDelay(0.25f);
        }

        public void PlusHp()
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 1;
            healthBar.DOComplete();
            healthDamagerBar.DOComplete();
            healthBar.DOFillAmount((float)bot.hp / bot.startHp, 0.25f);
            healthDamagerBar.DOFillAmount((float)bot.hp / bot.startHp, 0.25f);
        }

        private void OnDestroy()
        {
            healthBar.DOKill();
            healthDamagerBar.DOKill();
        }

        public void LateUpdate()
        {
            transform.LookAt(new Vector3(transform.position.x, UIInGame.instance.virtualCam.cam.transform.position.y, UIInGame.instance.virtualCam.cam.transform.position.z));
        }
    }
}