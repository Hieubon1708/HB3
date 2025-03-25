using UnityEngine;

namespace Hunter
{
    public static class UpgradeEnemy
    {
        public static void MoveSpeed(ref float current)
        {
            float ratio = 0.1f; // mỗi 10 level tăng lên 1
            float max = 3;
            current += Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }

        public static void MoveSpeedDetect(ref float current)
        {
            float ratio = 0.1f; // mỗi 10 level tăng lên 1
            float max = 3;
            current += Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }

        public static void RotateSpeed(ref float current)
        {
            float ratio = 0.05f; // mỗi 10 level tăng lên 0.5
            float max = 3;
            current += Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }

        public static void RotateSpeedDetect(ref float current)
        {
            float ratio = 0.05f; // mỗi 10 level tăng lên 0.5
            float max = 3;
            current += Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }
        
        public static void BulletSpeed(ref float current)
        {
            float ratio = 0.1f; // mỗi 10 level tăng lên 1
            float max = 7;
            current += Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }

        public static void DistanceFinding(ref float current)
        {
            float ratio = 0.1f; // mỗi 10 level tăng lên 1
            float max = 3;
            current += Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }

        public static void RateOfFire(ref float current)
        {
            float ratio = 0.01f; // mỗi 10 level giảm đi 0.1
            float max = 0.25f;
            current -= Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }

        public static void TransitionDurationAiming(ref float current)
        {
            float ratio = 0.01f; // mỗi 10 level giảm đi 0.1
            float max = 0.1f;
            current -= Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }

        public static void TimeDelayPoint(ref float current)
        {
            float ratio = 0.01f; // mỗi 10 level giảm đi 0.1
            float max = 0.35f;
            current -= Mathf.Clamp(GameManager.instance.Level * ratio, 0, max);
        }
    }
}
