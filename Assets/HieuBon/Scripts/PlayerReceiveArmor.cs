using UnityEngine;

namespace HieuBon
{
    public class PlayerReceiveArmor : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gameObject.SetActive(false);
                PlayerController.instance.player.playerIndexes.ArmorRegen();
            }
        }
    }
}