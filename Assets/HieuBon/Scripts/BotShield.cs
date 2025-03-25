using ACEPlay.Bridge;
using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class BotShield : BotSentry
    {
        public SpriteRenderer healthBar1;
        public SpriteRenderer healthBar2;
        Coroutine dodging;

        void StartDodging(Transform killer)
        {
            if (dodging == null)
            {
                dodging = StartCoroutine(Dodging(killer));
            }
        }

        void StopDodging()
        {
            if (dodging != null)
            {
                StopCoroutine(dodging);
                dodging = null;
            }
        }

        public override IEnumerator Attack(GameObject target)
        {
            Player player = LevelController.instance.GetPlayer(target);
            animator.SetTrigger("Aiming");
            animator.SetTrigger("Fire");
            yield return new WaitForSeconds(aiming * 1.5f);
            player.SubtractHp(damage, transform);
            weapon.Attack(player.transform);
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.ak47Gun, 0);
            yield return new WaitForSeconds(rateOfFire);
            StopAttack();
        }

        IEnumerator Dodging(Transform killer)
        {
            isDodging = true;
            isFind = true;
            navMeshAgent.isStopped = false;
            radarView.SetColor(false);
            ChangeSpeed(detectSpeed, rotateDetectSpeed);
            Vector3 dirOfAttack = (transform.position - killer.position).normalized;
            navMeshAgent.destination = transform.position + dirOfAttack * 2;
            animator.SetBool("Walking", true);
            animator.SetTrigger("Dodging");
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Walking", false);
            target = killer.gameObject;
            BridgeController.instance.Debug_Log("Dodging");
            isDodging = false;
            StopDodging();
        }

        public override void SubtractHp(int hp, Transform killer, bool isOnlyBurn)
        {
            if (this.hp <= 0) return;

            base.SubtractHp(hp, killer, isOnlyBurn);

            PlayBlood();
            StopAttack();
            StopProbe();
            if (healthBar2.color != Color.black)
            {
                healthBar2.color = Color.black;
                StartDodging(killer);
            }
            else
            {
                healthBar1.enabled = false;
                healthBar2.enabled = false;
                StopDodging();
            }
        }

        public override void InitBot()
        {
            base.InitBot();
            healthBar1.enabled = true;
            healthBar2.enabled = true;
            healthBar2.color = Color.white;
            StopDodging();
        }
    }
}
