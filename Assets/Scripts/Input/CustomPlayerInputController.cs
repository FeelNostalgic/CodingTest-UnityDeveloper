using System;
using CodingTest.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodingTest.Inputs
{
    public class CustomPlayerInputController : MonoBehaviour
    {
        #region Public Variables

        public Action OnRightClickDownUpdate { get; set; }
        public Action OnRightClickUpUpdate { get; set; }
        public Action OnLeftClickDownUpdate { get; set; }
        public Action OnLeftClickUpUpdate { get; set; }
        public Action<Vector2> OnPointerUpdate { get; set; }

        #endregion

        #region Private Variables

        private PlayerInput _playerInput;
        private InputActionMap _uiMap;
        private InputActionMap _emptyMap;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            ServiceContainer.AddInstance(this);
        }

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _uiMap = _playerInput.actions.FindActionMap("UI");
            _emptyMap = _playerInput.actions.FindActionMap("Empty");
        }

        #endregion

        #region Public Methods

        public void DisableUIMap()
        {
            _uiMap.Disable();
            _emptyMap.Enable();
        }

        public void EnableUIMap()
        {
            _uiMap.Enable();
            _emptyMap.Disable();
        }

        public void OnRightClick(InputValue input)
        {
            if (input.isPressed) OnRightClickDownUpdate?.Invoke();
            else OnRightClickUpUpdate?.Invoke();
        }

        public void OnPoint(InputValue input)
        {
            OnPointerUpdate?.Invoke(input.Get<Vector2>());
        }

        public void OnClick(InputValue input)
        {
            if (input.isPressed) OnLeftClickDownUpdate?.Invoke();
            else OnLeftClickUpUpdate?.Invoke();
        }

        #endregion
    }
}