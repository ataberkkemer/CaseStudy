using Input.Abstract;
using Input.Events;
using Service;
using Service.Concrete;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Concrete
{
    public class PlayerInputController : MonoBehaviour
    {
        private bool _isInputActive;
        private Camera _mainCamera;
        private IRayCaster _rayCaster;

        private void Awake()
        {
            _rayCaster = GetComponent<IRayCaster>();
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            ServiceLocator.Get<EventService>().Register<InputStateEvent>(OnInputState);
        }

        private void OnDisable()
        {
            ServiceLocator.Get<EventService>().Unregister<InputStateEvent>(OnInputState);
        }

        private void OnInputState(InputStateEvent data)
        {
            _isInputActive = data.IsInputActive;
        }

        public void OnTap(InputAction.CallbackContext context)
        {
            if (!_isInputActive) return;
            if (!context.performed) return;

            var currentPosition = Pointer.current.position.ReadValue();
            var result = _rayCaster.Cast<IInputReceiver>(currentPosition);

            result?.ReceiveInput();
        }
    }
}