using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Goossyaa
{
    public class TouchControls_ManipulateCharacter : MonoBehaviour
    {
        public static TouchControls_ManipulateCharacter Instance;

        public float camRotationFactor = 0.03F;

        public bool cameraInvertX;

        public bool cameraInvertY;

        [Header("Debug:")] 
        public Vector2 moveInput;

        public Vector3 rotateInput;

        public float moveInputX;

        public float moveInputY;

        public bool isJumpPressed;

        public bool isFirePressed;

        public bool isAutoAttackPressed;

        private bool _isComputer = true;

       [SerializeField] private TouchController _touchController;

       public TouchController TouchController => _touchController;

       public event Action OnAttacked;
       
        private void Awake()
        {
            Instance = this;
            _touchController = FindObjectOfType<TouchController>();
        }

        private void OnEnable() {
            // if(!ReInput.isReady) return;
            //
            // Player player = ReInput.players.GetPlayer(0);
            // if(player == null) return;
            //
            // // Subscribe to input events
            // player.AddInputEventDelegate(OnMoveReceivedX, UpdateLoopType.Update, InputActionEventType.AxisActive, "Horizontal");
            // player.AddInputEventDelegate(OnMoveReceivedX, UpdateLoopType.Update, InputActionEventType.AxisInactive, "Horizontal");
            // player.AddInputEventDelegate(OnMoveReceivedY, UpdateLoopType.Update, InputActionEventType.AxisActive, "Vertical");
            // player.AddInputEventDelegate(OnMoveReceivedY, UpdateLoopType.Update, InputActionEventType.AxisInactive, "Vertical");
            //
            // //Rot x
            // player.AddInputEventDelegate(OnRotationReceivedX, UpdateLoopType.Update, InputActionEventType.AxisActive, "RotateHorizontal");
            // player.AddInputEventDelegate(OnRotationReceivedX, UpdateLoopType.Update, InputActionEventType.AxisInactive, "RotateHorizontal");
            //
            // //Rot y
            // player.AddInputEventDelegate(OnRotationReceivedY, UpdateLoopType.Update, InputActionEventType.AxisActive, "RotateVertical");
            // player.AddInputEventDelegate(OnRotationReceivedY, UpdateLoopType.Update, InputActionEventType.AxisInactive, "RotateVertical");
            //
            // // Jump
            // player.AddInputEventDelegate(OnJumpPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Jump");
            // player.AddInputEventDelegate(OnJumpReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Jump");
            //
            //
            // // Auto Attack
            // player.AddInputEventDelegate(OnAutoAttackPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "AutoAttack");
            // player.AddInputEventDelegate(OnAutoAttackReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "AutoAttack");
            //
            // // Attack
            // player.AddInputEventDelegate(FirePressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed,
            //     "Fire");
            // player.AddInputEventDelegate(FireReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased,
            //     "Fire");


           
        }

        private void Start()
        {
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForEndOfFrame();
            
            if (DeviceData.IsMobile)
            {
                // // Attack
                //
                // _touchController.AttackButton.OnDown.AddListener(FirePressed);
                // _touchController.AttackButton.OnClick.AddListener(FireReleased);
                //
                // Jump
                _touchController.JumpButton.Clicked += OnJumpPressed;
                _touchController.JumpButton.StopClicked += OnJumpReleased;
            }
        }


        private void OnDisable() {
            // if(!ReInput.isReady) return;
            //
            // Player player = ReInput.players.GetPlayer(0);
            // if(player == null) return;
            //
            // // Unsubscribe from input events
            // player.RemoveInputEventDelegate(OnMoveReceivedX);
            // player.RemoveInputEventDelegate(OnMoveReceivedY);
            //
            //
            // // Camera rotation
            // player.RemoveInputEventDelegate(OnRotationReceivedX);
            // player.RemoveInputEventDelegate(OnRotationReceivedY);
            // // Jump
            // player.RemoveInputEventDelegate(OnJumpPressed);
            // player.RemoveInputEventDelegate(OnJumpReleased);
        }

        public void SetMouseInput(bool isComputer) => 
            _isComputer = isComputer;

        private void Update()
        {
           // return;

           if (_isComputer == false)
           {
               //Move
               Vector3 moveVector = _touchController.JoystickController.Direction; //(Vector3.right * _touchController.JoystickController.Horizontal + Vector3.forward * _touchController.JoystickController.Vertical);

               moveInputX = moveVector.x;
               moveInputY = moveVector.y;
               
               //Rot

               Vector2 cameraRot = _touchController.TouchpadController.Direction;

               cameraRotX = cameraRot.x;
               cameraRotY = cameraRot.y;
           }

           return;
           
           if (Input.GetMouseButtonUp(0) && _isComputer)
           {
               FireReleased();
           }

           if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
           {
               return;
           }

           if (Input.GetMouseButtonDown(0) && _isComputer)
           {
               FirePressed();
           }
        }




        public float cameraRotX;

        public float cameraRotY;

        public float GetHorizontalCameraInput()
        {
            var val = -cameraRotX * camRotationFactor;
            if (cameraInvertX) val = -val;
            return val;
        }

        public float GetVerticalCameraInput()
        {
            var val = cameraRotY * camRotationFactor;
            if (cameraInvertY) val = -val;
            return val;
        }


        private void OnJumpPressed()
        {
            Debug.Log("OnJumpPressed");
            isJumpPressed = true;
        }


        private void OnJumpReleased()
        {
            Debug.Log("OnJumpReleased");
            isJumpPressed = false;
        }

        //

        // private void OnAutoAttackPressed(InputActionEventData data)

        // {

        //     Debug.LogError("OnAutoAttackPressed");

        //     isAutoAttackPressed = true;

        // }

        //

        // private void OnAutoAttackReleased(InputActionEventData data)

        // {

        //     Debug.LogError("OnAutoAttackReleased");

        //     isAutoAttackPressed = false;

        // }


        public bool GetIsAutoAttackPressed()
        {
            return isAutoAttackPressed;
        }

        private void FirePressed()
        {
            isFirePressed = true;
            OnAttacked?.Invoke();
        }


        private void FireReleased()
        {
            isFirePressed = false;
        }


        public bool GetIsFirePressed()
        {
            return isFirePressed;
        }

        public float GetHorizontalMovementInput()
        {
            return moveInputX;
        }

        public float GetVerticalMovementInput()
        {
            return moveInputY;
        }

        public bool GetIsJumpingPressed()
        {
            return isJumpPressed;
        }

        // Move player

        private void OnMoveReceived(Vector2 move) {
            
        }
    }
}
