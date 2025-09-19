using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateIds { Normal, }

public class PlayerStateMachine : MonoBehaviour
{
    // dictionary for caching states
    private readonly Dictionary<PlayerStateIds, PlayerStateBase> _playerStates = new Dictionary<PlayerStateIds, PlayerStateBase>();
    // dictionary of all state types to make state creation easier (avoid big switch case statement)
    private readonly Dictionary<PlayerStateIds, System.Type> _playerStateTypes = new Dictionary<PlayerStateIds, System.Type>()
    {
        {PlayerStateIds.Normal, typeof(PlayerStateNormal) },
    };
    private readonly object[] _thisInstanceAsArray = new object[1];
    private PlayerStateBase _currentState;
    public PlayerStateBase CurrentState { get => _currentState; set => _currentState = value; }
    [SerializeField] private PlayerStateIds _currentStateId;
    public PlayerStateIds CurrentStateId { get => _currentStateId; }

    private CharacterController2D _charController;

    // analog movement input values
    private Vector2 _normalizedInput;
    private const float _ANALOG_ANGLE_SIN_COS = 0.3826f;

    // movement fields
    private Vector2 _moveVector;
    public Vector2 MoveVector { get => _moveVector; set => _moveVector = value; }
    [SerializeField] private float _moveSpeed = 2;
    public float MoveSpeed { get => _moveSpeed; }

    #region Input fields
    // input fields
    [SerializeField] private InputManagerSO _inputManager;

    private Vector2 _inputMove;
    private bool _isInputInteractPressed;
    private bool _isInputInteractHeld;
    private bool _isInputDodgePressed;
    private bool _isInputDodgeHeld;
    private bool _isInputShieldPressed;
    private bool _isInputShieldHeld;
    private bool _isInputPausePressed;

    public Vector2 InputMove { get => _inputMove; }
    public bool IsInputInteractPressed { get => _isInputInteractPressed; set => _isInputInteractPressed = value; }
    public bool IsInputInteractHeld { get => _isInputInteractHeld; }
    public bool IsInputDodgePressed { get => _isInputDodgePressed; set => _isInputDodgePressed = value; }
    public bool IsInputDodgeHeld { get => _isInputDodgeHeld; }
    public bool IsInputShieldPressed { get => _isInputShieldPressed; set => _isInputShieldPressed = value; }
    public bool IsInputShieldHeld { get => _isInputShieldHeld; }
    public bool IsInputPausePressed { get => _isInputPausePressed; set => _isInputPausePressed = value; }
    #endregion

    // debug fields
    public float Debug_TimeScale = 1;


    private void Awake()
    {
        Application.targetFrameRate = 60; // TODO move to game manager
        _thisInstanceAsArray[0] = this;
        _charController = GetComponent<CharacterController2D>();

        _currentState = GetState(PlayerStateIds.Normal);
        _currentStateId = PlayerStateIds.Normal;
        _currentState.EnterState();
    }

    private void OnEnable()
    {
        InitializeInput();
    }

    private void OnDisable()
    {
        ReleaseInput();
    }

    private void Update()
    {
        Time.timeScale = Debug_TimeScale;

        _currentState.CheckSwitchState();
        _currentState.UpdateState();

        MoveCharacter();
    }

    public PlayerStateBase GetState(PlayerStateIds stateId)
    {
        if (!_playerStates.ContainsKey(stateId))
        {
            // create a new instance of the proper state type (from _playerStateTypes)
            // and pass this PlayerStateMachine to its constructor
            PlayerStateBase newState = (PlayerStateBase)System.Activator.CreateInstance(_playerStateTypes[stateId], _thisInstanceAsArray);
            _playerStates.Add(stateId, newState);
        }

        return _playerStates[stateId];
    }

    public void SwitchState (PlayerStateIds newStateId)
    {
        _currentState.ExitState();
        _currentState = GetState(newStateId);
        _currentStateId = newStateId;
        _currentState.EnterState();
    }

    private void MoveCharacter ()
    {
        _charController.Move(_moveVector);
    }

    #region Input Methods
    private void InitializeInput()
    {
        _inputManager.MoveEvent += OnInputMove;
        _inputManager.MoveAnalogEvent += OnInputMoveAnalog;
        _inputManager.InteractEvent += OnInputInteractPressed;
        _inputManager.InteractCancelledEvent += OnInputInteractReleased;
        _inputManager.DodgeEvent += OnInputDodgePressed;
        _inputManager.DodgeCancelledEvent += OnInputDodgeReleased;
        _inputManager.ShieldEvent += OnInputShieldPressed;
        _inputManager.ShieldCancelledEvent += OnInputShieldReleased;
        _inputManager.PauseEvent += OnInputPausePressed;
    }

    private void ReleaseInput()
    {
        _inputManager.MoveEvent -= OnInputMove;
        _inputManager.MoveAnalogEvent -= OnInputMoveAnalog;
        _inputManager.InteractEvent -= OnInputInteractPressed;
        _inputManager.InteractCancelledEvent -= OnInputInteractReleased;
        _inputManager.DodgeEvent -= OnInputDodgePressed;
        _inputManager.DodgeCancelledEvent -= OnInputDodgeReleased;
        _inputManager.ShieldEvent -= OnInputShieldPressed;
        _inputManager.ShieldCancelledEvent -= OnInputShieldReleased;
        _inputManager.PauseEvent -= OnInputPausePressed;
}

    private void OnInputMove(Vector2 input)
    {
        _inputMove = input;
    }

    private void OnInputMoveAnalog(Vector2 input)
    {
        _inputMove = Vector2.zero;

        if (input.x == 0 && input.y == 0) // no key is pressed
        {
            return;
        }

        _normalizedInput = input.normalized;

        if (_normalizedInput.x > _ANALOG_ANGLE_SIN_COS) // right key is pressed
        {
            _inputMove.x = 1;
        }
        else if (_normalizedInput.x < -_ANALOG_ANGLE_SIN_COS) // left key is pressed
        {
            _inputMove.x = -1;
        }

        if (_normalizedInput.y > _ANALOG_ANGLE_SIN_COS) // up key is pressed
        {
            _inputMove.y = 1;
        }
        else if (_normalizedInput.y < -_ANALOG_ANGLE_SIN_COS) // down key is pressed
        {
            _inputMove.y = -1;
        }
    }

    private void OnInputInteractPressed()
    {
        _isInputInteractPressed = _isInputInteractHeld = true;
    }

    private void OnInputInteractReleased()
    {
        _isInputInteractPressed = _isInputInteractHeld = false;
    }

    private void OnInputDodgePressed()
    {
        _isInputDodgePressed = _isInputDodgeHeld = true;
    }

    private void OnInputDodgeReleased()
    {
        _isInputDodgePressed = _isInputDodgeHeld = false;
    }

    private void OnInputShieldPressed()
    {
        _isInputShieldPressed = _isInputShieldHeld = true;
    }

    private void OnInputShieldReleased()
    {
        _isInputShieldPressed = _isInputShieldHeld = false;
    }

    private void OnInputPausePressed()
    {
        _isInputPausePressed = true;
    }
    #endregion
}
