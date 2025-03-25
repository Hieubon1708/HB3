using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hunter
{
    public class IndexAttackAndHealth : MonoBehaviour
    {
        public TextMeshProUGUI attack;
        public TextMeshProUGUI health;

        public Color colorIncrease;
        public Color colorReduce;

        Tween tweenAttack;
        Tween tweenHealth;

        public void Start()
        {
            PlayerInformation.AttackAndHealth attackAndHealth = PlayerInformation.instance.GetCurrentAttackAndHealth();

            attack.text = attackAndHealth.attack.ToString();
            health.text = attackAndHealth.health.ToString();
        }

        public void UpdateAttackAndHealth(int currentAttack, int targetAttack, int currentHealth, int targetHealth)
        {
            if (currentAttack != targetAttack)
            {
                Color attackTargetColor = currentAttack < targetAttack ? colorIncrease : colorReduce;

                tweenAttack.Complete();

                tweenAttack = DOVirtual.Int(currentAttack, targetAttack, 0.5f, (v) =>
                {
                    attack.text = v.ToString() + (currentAttack < targetAttack ? " +" : " -");
                }).OnComplete(delegate
                {
                    attack.transform.parent.DOScale(1f, 0.15f);
                    attack.DOColor(Color.white, 0.15f);
                    attack.text = targetAttack.ToString();
                }).OnStart(delegate
                {
                    attack.transform.parent.DOScale(1.1f, 0.15f);
                    attack.DOColor(attackTargetColor, 0.15f);
                });
            }

            if (currentHealth != targetHealth)
            {
                Color healthTargetColor = currentHealth < targetHealth ? colorIncrease : colorReduce;

                tweenHealth.Complete();

                tweenHealth = DOVirtual.Int(currentHealth, targetHealth, 0.5f, (v) =>
                {
                    health.text = v.ToString() + (currentHealth < targetHealth ? " +" : " -");
                }).OnComplete(delegate
                {
                    health.DOColor(Color.white, 0.15f);
                    health.transform.parent.DOScale(1f, 0.15f);
                    health.text = targetHealth.ToString();
                }).OnStart(delegate
                {
                    health.transform.parent.DOScale(1.1f, 0.15f);
                    health.DOColor(healthTargetColor, 0.15f);
                });
            }
        }
    }
}
