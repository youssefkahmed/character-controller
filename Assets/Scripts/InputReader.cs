using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CharacterControllers.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "CharacterControllers/InputReader")]
    public class InputReader : ScriptableObject, PlayerInput.ICharacterControlsActions, IInputReader
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<bool> Run = delegate { };
        public event UnityAction<bool> Jump = delegate { };

        private PlayerInput _playerInput;
        
        public Vector2 Direction { get; }
        
        public void EnablePlayerActions()
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInput();
                _playerInput.CharacterControls.SetCallbacks(this);
            }
            _playerInput.Enable();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Run?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Run?.Invoke(false);
                    break;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump?.Invoke(false);
                    break;
            }
        }
    }
    
    public interface IInputReader
    {
        Vector2 Direction { get; }
        void EnablePlayerActions();
    }
}
