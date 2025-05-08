using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace HieuBon
{
    public class Cam : MonoBehaviour
    {
        public Camera cam;
        public Camera camUI;

        public CinemachineVirtualCamera cinemachineCam;
        private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
        CinemachineTransposer cinemachineTransposer;

        public float y1;
        public float y2;
        public float time;
        Tween camEnd;
        float defaultSize1 = 20.5f;
        float defaultSize2 = 17.5f;
        float elevator = 30;

        void Awake()
        {
            float aspectRatio = 1080f / 1920f;
            float newAspectRatio = (float)Screen.width / (float)Screen.height;
            defaultSize1 *= newAspectRatio / aspectRatio;
            defaultSize2 *= newAspectRatio / aspectRatio;
            elevator *= newAspectRatio / aspectRatio;
            cinemachineBasicMultiChannelPerlin = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineTransposer = cinemachineCam.GetCinemachineComponent<CinemachineTransposer>();
        }

        public void Init(Transform target)
        {
            cinemachineCam.Follow = target;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public void ResetCam()
        {
            camEnd.Kill();
            cinemachineCam.m_Lens.FieldOfView = defaultSize1;
            cinemachineTransposer.m_FollowOffset = new Vector3(cinemachineTransposer.m_FollowOffset.x, cinemachineTransposer.m_FollowOffset.y, y1);
        }

        public void CamStartZoom()
        {
            CamZoom(defaultSize2, time, 0f);
            DOVirtual.Float(cinemachineTransposer.m_FollowOffset.z, y2, time, (y) =>
            {
                cinemachineTransposer.m_FollowOffset = new Vector3(cinemachineTransposer.m_FollowOffset.x, cinemachineTransposer.m_FollowOffset.y, y);
            });
        }

        public void StartShakeCam(float strength)
        {
            ResetShake();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = strength;
            Invoke(nameof(StopShake), 0.25f);
        }

        void StopShake()
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }

        void ResetShake()
        {
            CancelInvoke(nameof(StartShakeCam));
            CancelInvoke(nameof(StopShake));
            StopShake();
        }

        public void CamZoom(float end, float duration, float delayCall)
        {
            camEnd = DOVirtual.Float(cinemachineCam.m_Lens.FieldOfView, end, duration, (v) =>
            {
                cinemachineCam.m_Lens.FieldOfView = v;
            }).SetDelay(delayCall);
        }

        public void ShakeCancel()
        {
            CancelInvoke(nameof(StartShakeCam));
        }
    }
}
