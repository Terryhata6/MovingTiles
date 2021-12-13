using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.UtilitsSpace;
using UnityEngine.Diagnostics;

namespace Core
{
    public class InputController : Singleton<InputController>
    {
        #region Events

        public delegate void StartTouch(Vector2 position, float time);

        public event StartTouch OnStartTouch;
        public delegate void EndTouch(Vector2 position, float time);

        public event StartTouch OnEndTouch;

        #endregion



        private Camera _mainCamera;
        private BaseAction _baseActions;

        private void Awake()
        {
            _baseActions = new BaseAction();
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _baseActions.Enable();
        }

        private void OnDisable()
        {
            _baseActions.Disable();
        }

        private void Start()
        {
            _baseActions.Touch.FirstTouchPosition.started += ctx => TouchStarted(ctx);
            _baseActions.Touch.FirstTouchPosition.canceled += ctx => TouchEnded(ctx);
        }

        

        private void TouchStarted(InputAction.CallbackContext ctx)
        {
            Debug.Log("Started");
            OnStartTouch?.Invoke(Utilits.ScreenToWorld(_mainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2 >()), (float)ctx.startTime);
            
        }
        
        private void TouchEnded(InputAction.CallbackContext ctx)
        {
            Debug.Log("Ended");
            OnStartTouch?.Invoke(Utilits.ScreenToWorld(_mainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2 >()), (float)ctx.startTime);
        }

        public Vector2 TouchPosition()
        {
            return Utilits.ScreenToWorld(_mainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>());
        }
    }
}
