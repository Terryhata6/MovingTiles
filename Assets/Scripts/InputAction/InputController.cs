using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputController : MonoBehaviour
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

        private void TouchEnded(InputAction.CallbackContext ctx)
        {
            Debug.Log("Start");
        }

        private void TouchStarted(InputAction.CallbackContext ctx)
        {
            Debug.Log("Ended");
            //OnStartTouch?.Invoke();
        }
    }
}
