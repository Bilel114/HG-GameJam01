using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManagerSO", menuName = "Scriptable Objects/InputManagerSO")]
public class InputManagerSO : ScriptableObject, GameInput.IGameplayActions
{
    // Gameplay
    public event Action<Vector2> MoveEvent = delegate { };
    public event Action<Vector2> MoveAnalogEvent = delegate { };
    public event Action InteractEvent = delegate { };
    public event Action InteractCancelledEvent = delegate { };
    public event Action DodgeEvent = delegate { };
    public event Action DodgeCancelledEvent = delegate { };
    public event Action ShieldEvent = delegate { };
    public event Action ShieldCancelledEvent = delegate { };
    public event Action PauseEvent = delegate { };

    private GameInput _gameInput;

    public void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            _gameInput.Gameplay.SetCallbacks(this);
        }
    }

    //public void OnDisable()
    //{
    //    _gameInput.Gameplay.RemoveCallbacks(this);
    //}

    public void OnMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
            case InputActionPhase.Canceled:
                MoveEvent.Invoke(context.ReadValue<Vector2>());
                break;
        }
    }

    public void OnMoveAnalog(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
            case InputActionPhase.Canceled:
                MoveAnalogEvent.Invoke(context.ReadValue<Vector2>());
                break;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                InteractEvent.Invoke();
                break;

            case InputActionPhase.Canceled:
                InteractCancelledEvent.Invoke();
                break;
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                DodgeEvent.Invoke();
                break;

            case InputActionPhase.Canceled:
                DodgeCancelledEvent.Invoke();
                break;
        }
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                ShieldEvent.Invoke();
                break;

            case InputActionPhase.Canceled:
                ShieldCancelledEvent.Invoke();
                break;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent.Invoke();
        }
    }
}
