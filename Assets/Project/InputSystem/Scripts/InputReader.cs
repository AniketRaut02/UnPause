using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DarkPixelGD.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName= "Input/InputReaderAsset")]
    public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions, InputSystem_Actions.IUIActions
    {
        private InputSystem_Actions _actions;

        //ContineousInputs
        public Vector2 MoveInput;
        public Vector2 LookInput;

        //Events and Handlers
        public event Action DashEvent;
        public event Action<bool> SprintEvent;
        public event Action<bool> CrouchEvent;
        public event Action AttackEvent;
        public event Action InteractEvent;
        public event Action NextEvent;
        public event Action PreviousEvent;

        public event Action PauseEvent;

        public void Initialize()
        {
            if (_actions == null)
            {
                _actions = new InputSystem_Actions();
                _actions.Player.SetCallbacks(this);
                _actions.UI.SetCallbacks(this);
            }
            EnablePlayerMap();
        }

        public void Terminate()
        {
            DisableAllMaps();
        }


        public void EnablePlayerMap()
        {
            _actions.UI.Disable();
            _actions.Player.Enable();
        }

        public void EnableUIMap()
        {
            _actions.Player.Disable();
            _actions.UI.Enable();
        }
        public void DisableAllMaps()
        {
            _actions.UI.Disable();
            _actions.Player.Disable();
        }

        //Playe Map Interfaces
        public void OnMove(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }
        public void OnLook(InputAction.CallbackContext ctx)
        {
            LookInput = ctx.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) {SprintEvent?.Invoke(true); }
            else if (ctx.canceled) { SprintEvent?.Invoke(false); }
        }

        public void OnDash(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) DashEvent?.Invoke();

        }
        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) { CrouchEvent?.Invoke(true); }
            else if (ctx.canceled) { CrouchEvent?.Invoke(false); }
        }
        public void OnAttack(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) AttackEvent?.Invoke();
        }
        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) InteractEvent?.Invoke();
        }
        public void OnNext(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) NextEvent?.Invoke();
        }
        public void OnPrevious(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) PreviousEvent?.Invoke();
        }
        public void OnPause(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) { PauseEvent?.Invoke(); }
        }


        //UI map Interfaces
        public void OnNavigate(InputAction.CallbackContext context) { }
        public void OnSubmit(InputAction.CallbackContext context) { }
        public void OnCancel(InputAction.CallbackContext context) { }
        public void OnPoint(InputAction.CallbackContext context) { }
        public void OnClick(InputAction.CallbackContext context) { }
        public void OnRightClick(InputAction.CallbackContext context) { }
        public void OnMiddleClick(InputAction.CallbackContext context) { }
        public void OnScrollWheel(InputAction.CallbackContext context) { }
        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
    }
}
