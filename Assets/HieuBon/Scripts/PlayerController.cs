using DG.Tweening;
using TigerForge;
using UnityEngine;

namespace HieuBon
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        public RectTransform canvas;
        public CanvasGroup canvasGroup;

        [HideInInspector]
        public UIReceiveMoney uIReceiveMoney;
        [HideInInspector]
        public Player player;

        Vector2 joystickSize;
        FloatingJoystick Joystick;
        Vector2 movementAmount;
        Vector3 scaledMovement;

        private void Awake()
        {
            instance = this;

            uIReceiveMoney = GetComponentInChildren<UIReceiveMoney>();
            Joystick = GetComponentInChildren<FloatingJoystick>();
            player = GetComponent<Player>();
        }

        public void HandleFingerMove()
        {
            if (GameController.instance.isTouch == GameController.IsTouch.No) return;

            Vector2 knobPosition;
            Vector2 clickPosition = Vector2.zero;

            float maxMovement = joystickSize.x / 2f;

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
            movementAmount = knobPosition / maxMovement;
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
            movementAmount = Vector2.zero;
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
            UIInGame.instance.handTutorial.PlayHand();
        }

        public void HandleFingerDown()
        {
            if (GameController.instance.isTouch == GameController.IsTouch.No) return;
            if (UIInGame.instance.handTutorial.canvasGroup.alpha != 0)
            {
                UIInGame.instance.handTutorial.StopHand();

                ShowTouch();

                UIInGame.instance.Play();
            }
            Vector2 clickPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, UIInGame.instance.virtualCam.camUI, out clickPosition);
            movementAmount = Vector2.zero;
            Joystick.RectTransform.sizeDelta = joystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(new Vector3(clickPosition.x, clickPosition.y + canvas.sizeDelta.y / 2));
        }

        private Vector2 ClampStartPosition(Vector2 StartPosition)
        {
            if (StartPosition.x < joystickSize.x / 2)
            {
                StartPosition.x = joystickSize.x / 2;
            }
            if (StartPosition.y < joystickSize.y / 2)
            {
                StartPosition.y = joystickSize.y / 2;
            }
            else if (StartPosition.x > Screen.width - joystickSize.x / 2)
            {
                StartPosition.x = Screen.width - joystickSize.x / 2;
            }
            else if (StartPosition.y > Screen.height - joystickSize.y / 2)
            {
                StartPosition.y = Screen.height - joystickSize.y / 2;
            }
            return StartPosition;
        }

        private void Update()
        {
            scaledMovement = player.navMeshAgent.speed * Time.deltaTime * new Vector3(movementAmount.x, 0, movementAmount.y);
            player.navMeshAgent.Move(scaledMovement);
            Vector3 lookAt = player.lookAt.transform.position + scaledMovement;
            player.transform.LookAt(lookAt);

            Vector3 speedMovement = player.navMeshAgent.speed * 0.0115f * new Vector3(movementAmount.x, 0, movementAmount.y);
            player.animator.SetFloat("Speed", Mathf.Clamp01(movementAmount.magnitude == 0 ? player.navMeshAgent.velocity.magnitude : speedMovement.magnitude * 25f));
        }

        public void Win()
        {
            HandleLoseFinger();
        }

        public void Lose()
        {
            HandleLoseFinger();
        }
    }
}
