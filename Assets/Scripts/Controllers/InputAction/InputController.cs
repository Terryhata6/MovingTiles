using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using Core.UtilitsSpace;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem.UI;

namespace Core
{
    [DefaultExecutionOrder(-1)]
    public class InputController : Singleton<InputController>
    {
        #region Events
        public delegate void StartTouch(Vector3 position, float time);
        public event StartTouch OnStartTouch;
        
        public delegate void EndTouch(Vector3 position, float time);
        public event EndTouch OnEndTouch;

        public delegate void SwipeDirection(SwipeDirections direction);
        public event SwipeDirection OnGetSwipe;

        #endregion


        [SerializeField] private InputSystemUIInputModule _inputSystemModule;
        [SerializeField] private EventSystem _eventSystem;
        private Camera _mainCamera;
        private BaseAction _baseActions;

        private void Awake()
        {
            _baseActions = new BaseAction();
            _mainCamera = Camera.main;
            if (_inputSystemModule == null)
            {
                _inputSystemModule = GetComponent<InputSystemUIInputModule>();
            }
            if (_eventSystem == null)
            {
                _eventSystem = GetComponent<EventSystem>();
            }
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
            _baseActions.Touch.FirstTouch.started += ctx => TouchStarted(ctx);
            _baseActions.Touch.FirstTouch.canceled += ctx => TouchEnded(ctx);
        }

        private void TouchStarted(InputAction.CallbackContext ctx)
        {
            if (_eventSystem.IsPointerOverGameObject())
            {
                Debug.Log($"PointerOverGameObject");
            }
            else
            {
                OnStartTouch?.Invoke(Utilits.GetPointFromCamera(_mainCamera,_baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>()),(float)ctx.startTime);
            }
        }
        
        private void TouchEnded(InputAction.CallbackContext ctx)
        {
            if (_eventSystem.IsPointerOverGameObject())
            {
                Debug.Log($"Selecting");
            }
            else
            {
                 //Enter-alt - потом убери, это для теста
                OnEndTouch?.Invoke(Utilits.GetPointFromCamera(_mainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>()),(float)ctx.time);
            }
            GameEvents.Instance.EndTouch();
        }

        public void GetSwipe(SwipeDirections direction)
        {
            OnGetSwipe?.Invoke(direction);
        }

        public Vector3 TouchPosition()
        {
            return Utilits.GetPointFromCamera(_mainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>());
        }
        
        public Vector3 TouchPosition(out RaycastHit hit, LayerMask layerMask)
        {
            return Utilits.GetPointFromCamera(_mainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>(), out hit, layerMask);
        }
    }
}
