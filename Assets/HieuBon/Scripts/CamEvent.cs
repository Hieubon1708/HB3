using UnityEngine;

namespace HieuBon
{
    public class CamEvent : MonoBehaviour
    {
        public void SetTimeScale(float time)
        {
            Time.timeScale = time;
        }
    }
}