using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class BotDemolition : BotSentry
    {
        public GameObject preBoom;
        public GameObject boomOnHand;
        public int amount;
        int indexx;
        BotDemolitionWeapon[] demolitionWeapons;
        public Vector3 destination;
        public BotDemolitionWeapon onHand;
        public Transform hand;
        BotDemolitionWeapon demolitionWeapon;

        public override void Start()
        {
            demolitionWeapons = new BotDemolitionWeapon[amount];
            for (int i = 0; i < amount; i++)
            {
                GameObject b = Instantiate(preBoom, LevelController.instance.pool);
                b.SetActive(false);
                demolitionWeapons[i] = b.GetComponent<BotDemolitionWeapon>();
                demolitionWeapons[i].bot = this;
            }
        }

        public void Throw()
        {
            if (attack == null)
            {
                StopAttack();
                return;
            }
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.throwBoom, 0);

            destination = target.transform.position;
            demolitionWeapon = demolitionWeapons[indexx];

            demolitionWeapon.DOKill();
            boomOnHand.transform.localScale = Vector3.zero;
            Vector3 dir = (destination - transform.position).normalized;

            demolitionWeapon.transform.localRotation = Quaternion.identity;
            demolitionWeapon.transform.position = weapon.transform.position;
            demolitionWeapon.ResetWeapon();
            demolitionWeapon.gameObject.SetActive(true);
            demolitionWeapon.transform.DOJump(destination, 3, 1, rateOfFire).SetEase(Ease.Linear).OnComplete(delegate
            {
                demolitionWeapon.Timer();
            });
            demolitionWeapon.transform.DOLocalRotate(new Vector3(90, 0, 90), rateOfFire).SetEase(Ease.Linear);

            indexx++;
            if (indexx == demolitionWeapons.Length) indexx = 0;
            boomOnHand.transform.DOScale(4f, 0.25f).SetDelay(rateOfFire).OnComplete(delegate
            {
                StopAttack(); 
            }).SetEase(Ease.Linear);
        }

        public override void SubtractHp(int hp, Transform killer, bool isOnlyBurn)
        {
            base.SubtractHp(hp, killer, isOnlyBurn);

            onHand.rb.isKinematic = true;
            onHand.transform.SetParent(LevelController.instance.pool);
            onHand.transform.DOKill();
            onHand.transform.localScale = Vector3.one;
            onHand.transform.DOJump(killer.position, 3, 1, 0.75f).SetEase(Ease.Linear).OnComplete(delegate
            {
                onHand.Timer();
            });
            onHand.transform.DOLocalRotate(new Vector3(90, 0, 90), 0.75f).SetEase(Ease.Linear);
        }

        public override void InitBot()
        {
            onHand.transform.SetParent(hand);
            base.InitBot();
        }

        public override IEnumerator Attack(GameObject target)
        {
            animator.SetTrigger("Fire");
            yield return null;
        }

        private void OnDestroy()
        {
            boomOnHand.transform.DOKill();
        }
    }
}