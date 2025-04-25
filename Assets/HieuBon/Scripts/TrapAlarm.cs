using UnityEngine;

namespace HieuBon
{
    public class TrapAlarm : MonoBehaviour
    {
        public ParticleSystem ligtht;

        public void Alert()
        {
            ligtht.Play();
        }

        public void StopAlert()
        {
            ligtht.Stop();
        }
    }
}
