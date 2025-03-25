using DG.Tweening;
using TigerForge;
using UnityEngine;

namespace Hunter
{
    public class PlayerTouchMovement : MonoBehaviour
    {
        [SerializeField]
        private Vector2 JoystickSize = new Vector2(300, 300);
        [SerializeField]
        private FloatingJoystick Joystick;
        [SerializeField]

        public Vector2 MovementAmount;

        public RectTransform canvas;

        Player player;
        public CanvasGroup canvasGroup;

        public void Init(Player player)
        {
            this.player = player;
        }

        public void HandleFingerMove()
        {
            if (player == null || LevelController.instance.players.Count == 0 || UIInGame.instance.layerCover.raycastTarget || canvasGroup.alpha == 0) return;
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            Vector2 clickPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, UIInGame.instance.virtualCam.camUI, out clickPosition);
            Vector2 touchPos = new Vector2(clickPosition.x, clickPosition.y + canvas.sizeDelta.y / 2);
            if (Vector2.Distance(
                touchPos,
                    Joystick.RectTransform.anchoredPosition
                ) > maxMovement)
            {
                knobPosition = (
                    touchPos - Joystick.RectTransform.anchoredPosition
                    ).normalized
                    * maxMovement;
            }
            else
            {
                knobPosition = touchPos - Joystick.RectTransform.anchoredPosition;
            }
            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;
        }

        public void ShowTouch()
        {
            canvasGroup.DOFade(1f, 0.5f);
        }

        public void HideTouch()
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 0;
        }

        public void HandleLoseFinger()
        {
            Joystick.Knob.anchoredPosition = Vector2.zero;
            MovementAmount = Vector2.zero;
            Joystick.RectTransform.anchoredPosition = new Vector2(0, 350);
        }

        public void Play()
        {
            GameManager.instance.FistTimeShowUIWeapon = 1;
            /*WeaponType weaponType = (WeaponType)EventManager.GetData(EventVariables.ChooseEquipment);
            if (weaponType != WeaponType.None)
            {
                GameManager.instance.Weapon = (int)weaponType;
                GameController.WeaponType w = GameController.instance.GetWeaponType((int)weaponType);
                LevelController.instance.AddWeapon(w);
            }
            UIManager.instance.UILevelProgress.Show();*/
            PlayerController.instance.handTutorial.PlayHand();
        }

        public void HandleFingerDown()
        {
            if (player == null) return;
            if (PlayerController.instance.handTutorial.canvasGroup.alpha != 0)
            {
                //UIManager.instance.UIHome.Hide();
                PlayerController.instance.handTutorial.StopHand();
                ShowTouch();
                player.health.DelayHideHealth();
                UIInGame.instance.Play();
            }
            Vector2 clickPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, UIInGame.instance.virtualCam.camUI, out clickPosition);
            MovementAmount = Vector2.zero;
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(new Vector3(clickPosition.x, clickPosition.y + canvas.sizeDelta.y / 2));
        }

        private Vector2 ClampStartPosition(Vector2 StartPosition)
        {
            /*if (StartPosition.x < JoystickSize.x / 2)
            {
                StartPosition.x = JoystickSize.x / 2;
            }
            if (StartPosition.y < JoystickSize.y / 2)
            {
                StartPosition.y = JoystickSize.y / 2;
            }
            else if (StartPosition.x > Screen.width - JoystickSize.x / 2)
            {
                StartPosition.x = Screen.width - JoystickSize.x / 2;
            }
            else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
            {
                StartPosition.y = Screen.height - JoystickSize.y / 2;
            }*/
            return StartPosition;
        }

        public Vector3 scaledMovement;

        private void Update()
        {
            if (player == null || LevelController.instance.players.Count == 0 || !player.navMeshAgent.enabled) return;
            scaledMovement = player.navMeshAgent.speed * Time.deltaTime * new Vector3(MovementAmount.x, 0, MovementAmount.y);
            player.navMeshAgent.Move(scaledMovement);
            Vector3 lookAt = player.lookAt.transform.position + scaledMovement;
            player.transform.LookAt(lookAt);
            player.animator.SetFloat("Speed", Mathf.Clamp01(MovementAmount.magnitude == 0 ? player.navMeshAgent.velocity.magnitude : MovementAmount.magnitude));
        }

        public void OnDestroy()
        {
            canvasGroup.DOKill();
        }
    }
}
