using UnityEngine;

namespace HieuBon
{
    public class PlayerReceiveHealth : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gameObject.SetActive(false);
                PlayerController.instance.player.playerIndexes.HealthRegen();
            }
        }
    }
}