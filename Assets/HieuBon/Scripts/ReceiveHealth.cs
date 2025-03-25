using UnityEngine;

namespace Hunter
{
    public class ReceiveHealth : MonoBehaviour
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