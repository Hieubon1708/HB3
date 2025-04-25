using ACEPlay.Bridge;
using UnityEngine;

namespace HieuBon
{
    public class BotListeningDistance : MonoBehaviour
    {
        public BotSentry bot;

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Scream") && !bot.isFind && bot.col.enabled)
            {
                bot.StartHear(other.gameObject);
                BridgeController.instance.Debug_Log("Enter " + other.transform.parent.name + " position " + other.transform.position);
            }
        }
    }
}
