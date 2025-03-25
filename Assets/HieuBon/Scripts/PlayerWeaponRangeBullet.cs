using UnityEngine;

namespace Hunter
{
    public abstract class PlayerWeaponRangeBullet : PlayerWeapon
    {
        public int amountBullet;
        public GameObject preBullet;
        public PlayerBullet[] bullets;
        public int indexBullet;
        public float speedBullet;
        public Transform startBullet;

        public override void Start()
        {
            bullets = new PlayerBullet[amountBullet];
            for (int i = 0; i < amountBullet; i++)
            {
                GameObject b = Instantiate(preBullet, LevelController.instance.pool);
                bullets[i] = b.GetComponent<PlayerBullet>();
                bullets[i].name = damage.ToString();
                b.SetActive(false);
            }
            base.Start();
        }
    }
}
