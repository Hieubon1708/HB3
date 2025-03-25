using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class PlayerHealth : MonoBehaviour
    {
        public Image healthDamagerBar;
        public Image healthBar;
        
        public Image armorDamagerBar;
        public Image armorBar;

        public Player player;
        public SkinnedMeshRenderer skinnedMeshRenderer;
        float blinkDuration = 0.1f;
        float blinkTimer;
        float blikIntensity = 2.5f;
        Color startColor;

        public void Start()
        {
            startColor = skinnedMeshRenderer.material.color;
        }

        public void SubtractHp()
        {
            blinkTimer = blinkDuration;

            armorBar.DOComplete();
            armorDamagerBar.DOComplete();

            healthBar.DOComplete();
            healthDamagerBar.DOComplete();

            if (armorBar.fillAmount > 0 )
            {
                armorBar.DOFillAmount((float)player.armor / player.startHp, 0.25f).OnComplete(delegate
                {
                    if(player.armor < 0)
                    {
                        healthBar.DOFillAmount((float)player.hp / player.startHp, 0.25f);
                    }
                });
                armorDamagerBar.DOFillAmount((float)player.armor / player.startHp, 0.25f).SetDelay(0.25f).OnComplete(delegate
                {
                    if (player.armor < 0)
                    {
                        healthDamagerBar.DOFillAmount((float)player.hp / player.startHp, 0.25f).SetDelay(0.25f);
                    }
                });
            }
            else
            {
                healthBar.DOFillAmount((float)player.hp / player.startHp, 0.25f);
                healthDamagerBar.DOFillAmount((float)player.hp / player.startHp, 0.25f).SetDelay(0.25f);
            }

            if ((float)player.hp / player.startHp <= 0) skinnedMeshRenderer.material.color = startColor;
        }

        public void PlusHp()
        {
            healthBar.DOComplete();
            healthDamagerBar.DOComplete();
            healthBar.DOFillAmount((float)player.hp / player.startHp, 0.25f);
            healthDamagerBar.DOFillAmount((float)player.hp / player.startHp, 0.25f);
            if ((float)player.hp / player.startHp > 0.3f) skinnedMeshRenderer.material.color = startColor;
        }
        
        public void PlusArmor()
        {
            armorBar.DOComplete();
            armorDamagerBar.DOComplete();
            armorBar.DOFillAmount((float)player.armor / player.startHp, 0.25f);
            armorDamagerBar.DOFillAmount((float)player.armor / player.startHp, 0.25f);
        }

        public void DelayHideHealth()
        {
            /*CancelInvoke(nameof(Hide));
            Invoke(nameof(Hide), 3f);*/
        }

        private void OnDestroy()
        {
            healthBar.DOKill();
            healthDamagerBar.DOKill();
        }

        private void Update()
        {
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blikIntensity) + 1f;
            skinnedMeshRenderer.material.color = Color.white * intensity;

            if (blinkTimer <= 0 && (float)player.hp / player.startHp <= 0.3f && player.hp > 0)
            {
                float time = Time.time;
                float value = 0.5f + Mathf.Sin(time * 10f) * 0.25f;
                skinnedMeshRenderer.material.color = new Color(1f, value, value, 1f);
            }
        }

        public void LateUpdate()
        {
            transform.LookAt(new Vector3(transform.position.x, UIInGame.instance.virtualCam.cam.transform.position.y, UIInGame.instance.virtualCam.cam.transform.position.z));
        }
    }
}