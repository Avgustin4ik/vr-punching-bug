namespace PunchingBag.Code.Services.InputService
{
    using System;
    using MageSurvivor.Code.Core.Abstract.Service;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public interface IInputService
    {
        event System.Action ActionButton;
    }
    
    public class InputService : Service, IInputService
    {
        private readonly KeyBinding _keyBinding;

        public InputService(KeyBinding keyBinding)
        {
            _keyBinding = keyBinding;
            BindControls();
        }

        private void BindControls()
        {
            _keyBinding.ActionButton.action.canceled += ctx => OnActionButton();
        }

        private void OnActionButton()
        {
            ActionButton?.Invoke();
        }

        ~InputService()
        {
            Debug.Log("InputService Destructor");
        }

        public event Action ActionButton;
    }

    [Serializable]
    public struct KeyBinding
    {
        public InputActionReference ActionButton;
    }
    
}