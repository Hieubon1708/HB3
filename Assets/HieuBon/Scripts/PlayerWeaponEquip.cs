using UnityEngine;

namespace Hunter
{
    public class PlayerWeaponEquip : MonoBehaviour
    {
        public GameObject[] preWeapons;
        public GameObject[] preWeaponsDefault;

        public void Equip(Player player, GameController.PlayerType playerType, GameController.WeaponType weaponType)
        {
            if (player.weapon != null)
            {
                Destroy(player.weapon.gameObject);
            }
            GameObject w = GetPreWeaponByIndex(playerType, weaponType);
            if (w)
            {
                GameObject weapon = Instantiate(w, player.hand);
                weapon.transform.localRotation = w.transform.localRotation;
                weapon.transform.localPosition = w.transform.localPosition;
                player.InitWeapon(weapon.GetComponent<PlayerWeapon>());
            }
            else
            {
                player.attackRangeCollider.radius = 0;
            }
        }

        GameObject GetPreWeaponByIndex(GameController.PlayerType playerType, GameController.WeaponType weaponType)
        {
            if (weaponType != GameController.WeaponType.Default)
            {
                for (int i = 0; i < preWeapons.Length; i++)
                {
                    if (i == (int)weaponType) return preWeapons[i];
                }
            }
            else
            {
                return preWeaponsDefault[(int)playerType];
            }
            return null;
        }
    }
}