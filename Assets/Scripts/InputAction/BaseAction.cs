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
                    ""name"": ""FirstTouch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2581b695-0f86-4dbe-9377-8a22781610b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""FirstTouchPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""78874ac9-193e-454a-8d0d-3e8c4452c1af"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseDown"",
                    ""type"": ""PassThrough"",
                    ""id"": ""960cba0f-3bf1-41e0-8590-503287a5adbe"",
                    ""expectedControlType"": ""Button"",
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
                    ""groups"": ""TouchScreen"",
                    ""action"": ""FirstTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2194d00c-15a0-4ced-b7c7-b617f7625154"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""TouchScreen"",
                    ""action"": ""FirstTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82c88614-c24a-425d-83b1-32c8cc51fc5d"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""TouchScreen"",
                    ""action"": ""FirstTouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4be5be13-4a91-402f-8855-468ba6a3d891"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""TouchScreen"",
                    ""action"": ""FirstTouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0b6011b-15f2-4474-a230-184e2ff091f9"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""TouchScreen"",
            ""bindingGroup"": ""TouchScreen"",
            ""devices"": []
        }
    ]
}");
        // Touch
        m_Touch = asset.FindActionMap("Touch", throwIfNotFound: true);
        m_Touch_FirstTouch = m_Touch.FindAction("FirstTouch", throwIfNotFound: true);
        m_Touch_FirstTouchPosition = m_Touch.FindAction("FirstTouchPosition", throwIfNotFound: true);
        m_Touch_MouseDown = m_Touch.FindAction("MouseDown", throwIfNotFound: true);
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
    private readonly InputAction m_Touch_FirstTouch;
    private readonly InputAction m_Touch_FirstTouchPosition;
    private readonly InputAction m_Touch_MouseDown;
    public struct TouchActions
    {
        private @BaseAction m_Wrapper;
        public TouchActions(@BaseAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @FirstTouch => m_Wrapper.m_Touch_FirstTouch;
        public InputAction @FirstTouchPosition => m_Wrapper.m_Touch_FirstTouchPosition;
        public InputAction @MouseDown => m_Wrapper.m_Touch_MouseDown;
        public InputActionMap Get() { return m_Wrapper.m_Touch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchActions set) { return set.Get(); }
        public void SetCallbacks(ITouchActions instance)
        {
            if (m_Wrapper.m_TouchActionsCallbackInterface != null)
            {
                @FirstTouch.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouch;
                @FirstTouch.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouch;
                @FirstTouch.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouch;
                @FirstTouchPosition.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchPosition;
                @FirstTouchPosition.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchPosition;
                @FirstTouchPosition.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnFirstTouchPosition;
                @MouseDown.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnMouseDown;
                @MouseDown.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnMouseDown;
                @MouseDown.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnMouseDown;
            }
            m_Wrapper.m_TouchActionsCallbackInterface = instance;
            if (instance != null)
            {
                @FirstTouch.started += instance.OnFirstTouch;
                @FirstTouch.performed += instance.OnFirstTouch;
                @FirstTouch.canceled += instance.OnFirstTouch;
                @FirstTouchPosition.started += instance.OnFirstTouchPosition;
                @FirstTouchPosition.performed += instance.OnFirstTouchPosition;
                @FirstTouchPosition.canceled += instance.OnFirstTouchPosition;
                @MouseDown.started += instance.OnMouseDown;
                @MouseDown.performed += instance.OnMouseDown;
                @MouseDown.canceled += instance.OnMouseDown;
            }
        }
    }
    public TouchActions @Touch => new TouchActions(this);
    private int m_TouchScreenSchemeIndex = -1;
    public InputControlScheme TouchScreenScheme
    {
        get
        {
            if (m_TouchScreenSchemeIndex == -1) m_TouchScreenSchemeIndex = asset.FindControlSchemeIndex("TouchScreen");
            return asset.controlSchemes[m_TouchScreenSchemeIndex];
        }
    }
    public interface ITouchActions
    {
        void OnFirstTouch(InputAction.CallbackContext context);
        void OnFirstTouchPosition(InputAction.CallbackContext context);
        void OnMouseDown(InputAction.CallbackContext context);
    }
}
