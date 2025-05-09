using DG.Tweening;
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

        Vector2 joystickSize = new Vector2(300, 300);
        FloatingJoystick Joystick;
        Vector2 movementAmount;
        [HideInInspector]
        public Vector3 scaledMovement;
        float speed;
        float mulJoystick;

        public Vector3 Destination
        {
            set
            {
                player.navMeshAgent.SetDestination(value);
            }
        }

        public float AngularSpeed
        {
            set
            {
                player.navMeshAgent.angularSpeed = value;
            }
        }

        private void Awake()
        {
            instance = this;

            uIReceiveMoney = GetComponentInChildren<UIReceiveMoney>();
            Joystick = GetComponentInChildren<FloatingJoystick>();
            player = GetComponent<Player>();
        }

        public void PointerDown()
        {

            if (UIInGame.instance.handTutorial.canvasGroup.alpha != 0)
            {
                UIInGame.instance.handTutorial.StopHand();

                GameController.instance.gameState = GameController.GameState.Play;

                ShowTouch();

                UIInGame.instance.Play();
            }

            if (GameController.instance.gameState == GameController.GameState.Pause) return;

            Vector2 clickPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, UIInGame.instance.virtualCam.camUI, out clickPosition);
            movementAmount = Vector2.zero;
            Joystick.RectTransform.sizeDelta = joystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(new Vector3(clickPosition.x, clickPosition.y + canvas.sizeDelta.y / 2));
        }

        public void Drag()
        {
            if (GameController.instance.gameState == GameController.GameState.Pause) return;

            Vector2 knobPosition;
            Vector2 clickPosition = Vector2.zero;

            float maxMovement = joystickSize.x / 2f;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, UIInGame.instance.virtualCam.camUI, out clickPosition);
            Vector2 touchPos = new Vector2(clickPosition.x, clickPosition.y + canvas.sizeDelta.y / 2);

            if (Vector2.Distance(touchPos, Joystick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (touchPos - Joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = touchPos - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            movementAmount = knobPosition / maxMovement;
        }

        public void PointerUp()
        {
            Joystick.Knob.anchoredPosition = Vector2.zero;
            movementAmount = Vector2.zero;
            Joystick.RectTransform.anchoredPosition = new Vector2(0, 350);
        }

        private void Update()
        {
            if (GameController.instance.gameState == GameController.GameState.Play)
            {
                float speedOfFrame = player.navMeshAgent.speed * Time.deltaTime;

                Vector3 dir = Vector3.zero;

                bool isKilling = player.isComeCloser;

                if (isKilling)
                {
                    if(Vector3.Distance(player.bots[0].transform.position, transform.position) > 1f)
                    {
                        dir = (player.bots[0].transform.position - transform.position).normalized * mulJoystick;
                        dir.y = 0;
                    }
                }
                else
                {
                    dir = new Vector3(movementAmount.x, 0f, movementAmount.y);
                }

                scaledMovement = speedOfFrame * dir;

                player.navMeshAgent.Move(scaledMovement);

                transform.LookAt(player.isComeCloser ? new Vector3(player.bots[0].transform.position.x, transform.position.y, player.bots[0].transform.position.z) : transform.position + scaledMovement);

                if (!player.isComeCloser)
                {
                    Vector3 total = speedOfFrame * (new Vector3(movementAmount.x, 0f, movementAmount.y) * 1f).normalized;
                    mulJoystick = movementAmount.magnitude;
                    speed = 0.6f + (scaledMovement.magnitude / total.magnitude) * 0.4f;
                }

                player.animator.SetFloat("Speed", speed);
            }

            if (GameController.instance.gameState == GameController.GameState.Pause)
            {
                player.animator.SetFloat("Speed", player.navMeshAgent.velocity.magnitude);
            }
        }

        public void ShowTouch()
        {
            canvasGroup.DOFade(1f, 0.5f);
        }

        public void HideTouch()
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 0;

            scaledMovement = Vector3.zero;
        }

        public void Pause()
        {
            PlayerController.instance.HideTouch();
            PlayerController.instance.AngularSpeed = 500;

            GameController.instance.gameState = GameController.GameState.Pause;
        }

        public void Resume()
        {
            PlayerController.instance.ShowTouch();
            PlayerController.instance.AngularSpeed = 0;

            GameController.instance.gameState = GameController.GameState.Play;
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

        private Vector2 ClampStartPosition(Vector2 StartPosition)
        {
            /*if (StartPosition.x < joystickSize.x / 2)
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
            }*/
            return StartPosition;
        }
    }
}
