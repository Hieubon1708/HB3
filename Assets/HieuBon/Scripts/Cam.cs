using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Hunter
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
        float defaultSize1 = 20.60969f;
        float defaultSize2 = 17.47027f;
        float elevator = 29.86284f;

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

        public void Init(Player player)
        {
            cinemachineCam.Follow = player.transform;
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
            DOVirtual.Float(cinemachineCam.m_Lens.FieldOfView, defaultSize2, time, (v) =>
            {
                cinemachineCam.m_Lens.FieldOfView = v;
            });
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

        public void ShakeCancel()
        {
            CancelInvoke(nameof(StartShakeCam));
        }

        public void ElevatorMoveUp(float time)
        {
            camEnd = DOVirtual.Float(cinemachineCam.m_Lens.FieldOfView, elevator, time, (v) =>
            {
                cinemachineCam.m_Lens.FieldOfView = v;
            });
        }
    }
}
