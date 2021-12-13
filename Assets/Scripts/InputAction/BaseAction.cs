// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputAction/BaseAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @BaseAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @BaseAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BaseAction"",
    ""maps"": [
        {
            ""name"": ""Touch"",
            ""id"": ""9a18ab29-ceb2-406e-a6b0-90dc05bf800d"",
            ""actions"": [
                {
                    ""name"": ""FirstTouchBool"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2581b695-0f86-4dbe-9377-8a22781610b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FirstTouchPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""78874ac9-193e-454a-8d0d-3e8c4452c1af"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9675a72e-567d-4905-ac4f-6383b0a7c958"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FirstTouchBool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82c88614-c24a-425d-83b1-32c8cc51fc5d"",
                    ""path"": ""<Touchscreen>/primaryTouch/startPosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FirstTouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Touch
        m_Touch = asset.FindActionMap("Touch", throwIfNotFound: true);
        m_Touch_FirstTouchBool = m_Touch.FindAction("FirstTouchBool", throwIfNotFound: true);
        m_Touch_FirstTouchPosition = m_Touch.FindAction("FirstTouchPosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Touch
    private readonly InputActionMap m_Touch;
    private ITouchActions m_TouchActionsCallbackInterface;
    private readonly InputAction m_Touch_FirstTouchBool;
    private readonly InputAction m_Touch_FirstTouchPosition;
    public struct TouchActions
    {
        private @BaseAction m_Wrapper;
        public TouchActions(@BaseAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @FirstTouchBool => m_Wrapper.m_Touch_FirstTouchBool;
        public InputAction @FirstTouchPosition => m_Wrapper.m_Touch_FirstTouchPosition;
        public InputActionMap Get() { return m_Wrapper.m_Touch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchActions set) { return set.Get(); }
        public void SetCallbacks(ITouchActions instance)
        {
            if (m_Wrapper.m_TouchActionsCallbackInterface != null)
            {
                @FirstTouchBool.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchBool;
                @FirstTouchBool.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchBool;
                @FirstTouchBool.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchBool;
                @FirstTouchPosition.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchPosition;
                @FirstTouchPosition.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchPosition;
                @FirstTouchPosition.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchPosition;
            }
            m_Wrapper.m_TouchActionsCallbackInterface = instance;
            if (instance != null)
            {
                @FirstTouchBool.started += instance.OnFirstTouchBool;
                @FirstTouchBool.performed += instance.OnFirstTouchBool;
                @FirstTouchBool.canceled += instance.OnFirstTouchBool;
                @FirstTouchPosition.started += instance.OnFirstTouchPosition;
                @FirstTouchPosition.performed += instance.OnFirstTouchPosition;
                @FirstTouchPosition.canceled += instance.OnFirstTouchPosition;
            }
        }
    }
    public TouchActions @Touch => new TouchActions(this);
    public interface ITouchActions
    {
        void OnFirstTouchBool(InputAction.CallbackContext context);
        void OnFirstTouchPosition(InputAction.CallbackContext context);
    }
}
